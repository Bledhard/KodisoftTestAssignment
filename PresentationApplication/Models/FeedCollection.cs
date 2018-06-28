using Newtonsoft.Json;
using System.Collections.Generic;

namespace PresentationApplication.Models
{
    public class FeedCollection
    {
        public int ID { get; set; }

        [JsonIgnore]
        public string UserID { get; set; }

        public string Title { get; set; }

        [JsonProperty("Feeds")]
        public ICollection<FeedCollectionFeed> FeedCollectionFeeds { get; } = new List<FeedCollectionFeed>();
    }
}
