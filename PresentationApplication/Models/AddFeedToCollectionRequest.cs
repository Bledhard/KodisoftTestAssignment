using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationApplication.Models
{
    public class AddFeedToCollectionRequest
    {
        public int FeedId { get; set; }
        public int FeedCollectionId { get; set; }
    }
}
