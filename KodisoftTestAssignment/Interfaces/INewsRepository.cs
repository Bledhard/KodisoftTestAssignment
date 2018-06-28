using KodisoftTestAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Interfaces
{
    public interface INewsRepository
    {
        List<FeedCollection> GetUserFeedCollections(string userId);
        List<Feed> GetAllFeeds();
        FeedCollection GetFeedCollection(int id);
        Feed GetFeed(int id);
        bool Contains(int feedCollectionId, int feedId);
        bool Contains(string title, string userId);
        bool Contains(string feedUrl);
        void Add(FeedCollection feedCollection);
        void Add(Feed feed);
        void Add(int feedCollectionId, int feedId);
        void Remove(FeedCollection feedCollection);
        void Remove(Feed feed);
        int SaveChanges();
    }
}
