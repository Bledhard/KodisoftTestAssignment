using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Models
{
    public class FeedCollectionFeed
    {
        public int FeedCollectionID { get; set; }

        public FeedCollection FeedCollection { get; set; }

        public int FeedID { get; set; }

        public Feed Feed {get;set;}
    }
}
