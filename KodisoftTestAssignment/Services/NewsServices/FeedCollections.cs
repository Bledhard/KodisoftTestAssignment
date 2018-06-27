using KodisoftTestAssignment.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using KodisoftTestAssignment.Requests;
using Microsoft.EntityFrameworkCore;

namespace KodisoftTestAssignment.Services
{
    public partial class NewsServices
    {
        public void AddFeedToCollection(AddFeedToCollectionRequest request)
        {
            _logger.LogInformation("User " + request.UserId + " called method AddFeedToCollection(int, int)");

            var feed = _dbContext.Feeds.FirstOrDefault(f => f.ID == request.FeedId);
            var feedCollection = _dbContext.FeedCollections.FirstOrDefault(fc => fc.ID == request.CollectionId);

            _dbContext.Add(new FeedCollectionFeed { Feed = feed, FeedCollection = feedCollection });
            _dbContext.SaveChanges();
        }

        public int CreateFeedCollection(CreateFeedCollectionRequest request)
        {
            _logger.LogInformation("User " + request.UserId + " called method CreateFeedCollection(string)");

            var fc = new FeedCollection
            {
                Title = request.Title,
                UserID = request.UserId
            };
            _dbContext.FeedCollections.Add(fc);
            _dbContext.SaveChanges();

            return fc.ID;
        }

        public List<FeedCollection> GetUserFeedCollections(GetUserFeedCollectionsRequest request)
        {
            _logger.LogInformation("User " + request.UserId + " called method GetUserFeedCollections()");

            return _dbContext.FeedCollections.Where(fc => fc.UserID == request.UserId).ToList();
        }

        public FeedCollection GetFeedCollection(GetFeedCollectionRequest request)
        {
            _logger.LogInformation("User " + request.UserId + " called method GetFeedCollection()");

            var feedCollection = _dbContext.FeedCollections
                                           .Include(e => e.FeedCollectionFeeds)
                                           .ThenInclude(e => e.Feed)
                                           .FirstOrDefault(fc => fc.ID == request.FeedCollectionID);
            var feeds = feedCollection.FeedCollectionFeeds.Select(fcf => fcf.Feed).ToList();
            foreach (var feed in feeds)
            {
                feed.Items = ParseItems(feed.Link, feed.FeedType).ToList();
            }

            return feedCollection;
        }
    }
}
