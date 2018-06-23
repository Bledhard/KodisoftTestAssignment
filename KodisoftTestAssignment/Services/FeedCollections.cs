using KodisoftTestAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Services
{
    public partial class Services
    {
        public static void AddFeedToCollection(string collectionId, string feedId)
        {
            var feedCollection = GetFeedCollection(collectionId);
            var feed = GetFeed(feedId);
            feedCollection.Feeds.Add(feed);
        }

        public static List<FeedCollection> GetFeedCollections()
        {
            return new List<FeedCollection>();
        }

        public static FeedCollection GetFeedCollection(string id)
        {
            return new FeedCollection();
        }
    }
}
