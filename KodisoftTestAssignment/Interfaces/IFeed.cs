using KodisoftTestAssignment.Enumerators;
using KodisoftTestAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Interfaces
{
    public interface IFeed
    {
        string Link { get; set; }
        string Title { get; set; }
        string Content { get; set; }
        DateTime PublishDate { get; set; }
        FeedType FeedType { get; set; }
        List<Article> CachedArticles { get; set; }
    }
}
