using KodisoftTestAssignment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Models
{
    public class NewsRepository : INewsRepository
    {
        private readonly MainAppDbContext _dbContext;

        public NewsRepository(MainAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<FeedCollection> GetUserFeedCollections(string userId) =>
            _dbContext.FeedCollections.Where(fc => fc.UserID == userId).ToList();

        public List<Feed> GetAllFeeds() =>
            _dbContext.Feeds.ToList();

        public FeedCollection GetFeedCollection(int id) =>
            _dbContext.FeedCollections
                .Include(e => e.FeedCollectionFeeds)
                .ThenInclude(e => e.Feed)
                .FirstOrDefault(fc => fc.ID == id);

        public Feed GetFeed(int id) =>
            _dbContext.Feeds.First(f => f.ID == id);

        public bool Contains(int feedCollectionId, int feedId) =>
            _dbContext.FeedCollections.First(fc => fc.ID == feedCollectionId)
                   .FeedCollectionFeeds.Any(fcf => fcf.FeedID == feedId);

        public bool Contains(string title, string userId) =>
            _dbContext.FeedCollections.Any(fc => fc.Title == title && fc.UserID == userId);

        public bool Contains(string feedUrl) =>
            _dbContext.Feeds.Any(f => f.Link == feedUrl);

        public void Add(FeedCollection feedCollection) =>
            _dbContext.FeedCollections.Add(feedCollection);

        public void Add(Feed feed) =>
            _dbContext.Feeds.Add(feed);

        public void Add(int feedCollectionId, int feedId)
        {
            if (_dbContext.FeedCollections.First(fc => fc.ID == feedCollectionId)
                   .FeedCollectionFeeds.Any(fcf => fcf.FeedID == feedId))
            {
                throw new ArgumentException("This feed collection already contains this feed.");
            }

            var feedCollection = _dbContext.FeedCollections.FirstOrDefault(fc => fc.ID == feedCollectionId);
            var feed = _dbContext.Feeds.FirstOrDefault(f => f.ID == feedId);

            _dbContext.Add(new FeedCollectionFeed { Feed = feed, FeedCollection = feedCollection });
        }

        public void Remove(FeedCollection feedCollection) =>
            _dbContext.FeedCollections.Remove(feedCollection);

        public void Remove(Feed feed) =>
            _dbContext.Feeds.Remove(feed);

        public int SaveChanges() =>
            _dbContext.SaveChanges();
    }
}
