using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationApplication.Models
{
    public class Feed
    {
        public int ID { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public FeedType FeedType { get; set; }

        [JsonIgnore]
        public ICollection<FeedCollectionFeed> FeedCollectionFeeds { get; } = new List<FeedCollectionFeed>();

        [NotMapped]
        public List<Item> Items { get; set; }

        public Feed()
        {
            Link = "";
            Title = "";
        }
    }

    public enum FeedType
    {
        RSS,
        Atom
    }
}
