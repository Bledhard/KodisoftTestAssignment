﻿using Newtonsoft.Json;

namespace KodisoftTestAssignment.Models
{
    public class FeedCollectionFeed
    {
        [JsonIgnore]
        public int FeedCollectionID { get; set; }

        [JsonIgnore]
        public FeedCollection FeedCollection { get; set; }

        [JsonIgnore]
        public int FeedID { get; set; }

        public Feed Feed {get;set;}
    }
}
