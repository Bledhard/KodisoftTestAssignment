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

            if(_dbContext.FeedCollections.First(fc => fc.ID == request.FeedCollectionId)
                .FeedCollectionFeeds.Any(fcf => fcf.FeedID == request.FeedId))
            {
                throw new ArgumentException("This feed collection already contains this feed.");
            }

            var feedCollection = _dbContext.FeedCollections.FirstOrDefault(fc => fc.ID == request.FeedCollectionId);
            var feed = _dbContext.Feeds.FirstOrDefault(f => f.ID == request.FeedId);

            _dbContext.Add(new FeedCollectionFeed { Feed = feed, FeedCollection = feedCollection });
            _dbContext.SaveChanges();
        }

        public int CreateFeedCollection(CreateFeedCollectionRequest request)
        {
            _logger.LogInformation("User " + request.UserId + " called method CreateFeedCollection(string)");

            if (_dbContext.FeedCollections.Any(fc => fc.Title == request.Title && fc.UserID == request.UserId))
            {
                _logger.LogError("CreateFeedCollection threw ArgumentException: User #" + request.UserId +
                    " has already created Feed Collection with title \"" + request.Title + "\"");
                throw new ArgumentException("This user has already created Feed Collection with this title.");
            }

            var feedCollection = new FeedCollection
            {
                Title = request.Title,
                UserID = request.UserId
            };
            _dbContext.FeedCollections.Add(feedCollection);
            var n = _dbContext.SaveChanges();
            if (n > 0)
            {
                _cache.Set("fc_" + feedCollection.ID, feedCollection, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });
            }

            return feedCollection.ID;
        }

        public List<FeedCollection> GetUserFeedCollections(GetUserFeedCollectionsRequest request)
        {
            _logger.LogInformation("User " + request.UserId + " called method GetUserFeedCollections()");

            return _dbContext.FeedCollections.Where(fc => fc.UserID == request.UserId).ToList();
        }

        public FeedCollection GetFeedCollection(GetFeedCollectionRequest request)
        {
            _logger.LogInformation("User " + request.UserId + " called method GetFeedCollection()");

            if (!_cache.TryGetValue("fc_" + request.FeedCollectionID, out FeedCollection feedCollection))
            {
                feedCollection = _dbContext.FeedCollections
                                           .Include(e => e.FeedCollectionFeeds)
                                           .ThenInclude(e => e.Feed)
                                           .FirstOrDefault(fc => fc.ID == request.FeedCollectionID);
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
