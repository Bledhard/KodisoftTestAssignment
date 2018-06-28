using KodisoftTestAssignment.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using KodisoftTestAssignment.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace KodisoftTestAssignment.Services
{
    public partial class NewsServices
    {
        public void AddFeedToCollection(AddFeedToCollectionRequest request)
        {
            _logger.LogInformation("User " + request.UserId + " called method AddFeedToCollection(int, int)");

            if(request.FeedId == 0 || _newsRepository.GetFeed(request.FeedId) == null)
            {
                _logger.LogWarning("AddFeedToCollection threw ArgumentException: User #" + request.UserId +
                    " gave improper feedId.");
                throw new ArgumentException("This feed doesn't exist.");
            }
            if(request.FeedCollectionId == 0 || _newsRepository.GetFeedCollection(request.FeedCollectionId) == null)
            {
                _logger.LogWarning("AddFeedToCollection threw ArgumentException: User #" + request.UserId +
                    " gave improper feedCollectionId.");
                throw new ArgumentException("This feed collection doesn't exist.");
            }
            if(_newsRepository.Contains(request.FeedCollectionId, request.FeedId))
            {
                _logger.LogWarning("AddFeedToCollection threw ArgumentException: User #" + request.UserId +
                    " tried to add Feed to Feed Collection 2nd time.");
                throw new DuplicateWaitObjectException("This feed collection already contains this feed.");
            }

            _newsRepository.Add(request.FeedCollectionId, request.FeedId);
            _newsRepository.SaveChanges();
        }

        public int CreateFeedCollection(CreateFeedCollectionRequest request)
        {
            _logger.LogInformation("User " + request.UserId + " called method CreateFeedCollection(string)");

            if(request.Title == "" || request.Title == null)
            {
                _logger.LogWarning("CreateFeedCollection threw ArgumentException: User #" + request.UserId +
                    " gave improper title for Feed Collection.");
                throw new ArgumentException("Improper title for Feed Collection.");
            }
            if (_newsRepository.Contains(request.Title, request.UserId))
            {
                _logger.LogError("CreateFeedCollection threw ArgumentException: User #" + request.UserId +
                    " has already created Feed Collection with title \"" + request.Title + "\"");
                throw new DuplicateWaitObjectException("This user has already created Feed Collection with this title.");
            }

            var feedCollection = new FeedCollection
            {
                Title = request.Title,
                UserID = request.UserId
            };
           _newsRepository.Add(feedCollection);
            var n = _newsRepository.SaveChanges();
            if (n > 0)
            {
                _cache.Set("fc_" + feedCollection.ID, feedCollection, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });
            }

            return feedCollection.ID;
        }

        public List<FeedCollection> GetUserFeedCollections(string userId)
        {
            _logger.LogInformation("User " + userId + " called method GetUserFeedCollections()");

            return _newsRepository.GetUserFeedCollections(userId);
        }

        public FeedCollection GetFeedCollection(GetFeedCollectionRequest request)
        {
            _logger.LogInformation("User " + request.UserId + " called method GetFeedCollection()");

            if (!_cache.TryGetValue("fc_" + request.FeedCollectionID, out FeedCollection feedCollection))
            {
                feedCollection = _newsRepository.GetFeedCollection(request.FeedCollectionID);
                if (feedCollection == null)
                {
                    var message = "There's no FeedCollection with ID=" + request.FeedCollectionID + ".";
                    _logger.LogError("GetFeedCollection threw ArgumentException: " + message);
                    throw new ArgumentException(message);
                }
                else
                {
                    // We don't want to store all items in DB, because they can be changed quite often
                    // But we want to have them in cache
                    var feeds = feedCollection.FeedCollectionFeeds.Select(fcf => fcf.Feed).ToList();
                    foreach (var feed in feeds)
                    {
                        feed.Items = ParseItems(feed.Link, feed.FeedType).ToList();
                    }

                    _cache.Set("fc_" + feedCollection.ID, feedCollection,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            return feedCollection;
        }
    }
}
