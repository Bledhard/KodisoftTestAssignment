using KodisoftTestAssignment.Enumerators;
using System;

namespace KodisoftTestAssignment.Interfaces
{
    public interface IFeed
    {
        int ID { get; set; }
        string Link { get; set; }
        string Title { get; set; }
        string Content { get; set; }
        DateTime PublishDate { get; set; }
        FeedType FeedType { get; set; }
    }
}
