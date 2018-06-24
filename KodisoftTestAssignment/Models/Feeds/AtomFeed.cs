using KodisoftTestAssignment.Enumerators;
using KodisoftTestAssignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Models
{
    public class AtomFeed : IFeed
    {
        public int ID { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public FeedType FeedType { get; set; }
        public List<Article> CachedArticles { get; set; }

        public AtomFeed()
        {
            Link = "";
            Title = "";
            Content = "";
            PublishDate = DateTime.Today;
            FeedType = FeedType.Atom;
        }
    }
}
