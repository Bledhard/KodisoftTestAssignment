using System.Collections.Generic;

namespace KodisoftTestAssignment.Models
{
    public class FeedCollection
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        public string Title { get; set; }

        public ICollection<FeedCollectionFeed> FeedCollectionFeeds { get; } = new List<FeedCollectionFeed>();
    }
}
