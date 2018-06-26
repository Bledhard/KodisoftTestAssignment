using KodisoftTestAssignment.Enumerators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KodisoftTestAssignment.Models
{
    public class Feed
    {
        public int ID { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public FeedType FeedType { get; set; }
        public ICollection<FeedCollectionFeed> FeedCollectionFeeds { get; } = new List<FeedCollectionFeed>();

        [NotMapped]
        public List<Item> Items { get; set; }

        public Feed()
        {
            Link = "";
            Title = "";
        }
    }
}
