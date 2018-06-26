using KodisoftTestAssignment.Enumerators;

namespace KodisoftTestAssignment.Models
{
    public class RssFeed : Feed
    {
        public RssFeed()
            : base()
        {
            FeedType = FeedType.RSS;
        }
    }
}
