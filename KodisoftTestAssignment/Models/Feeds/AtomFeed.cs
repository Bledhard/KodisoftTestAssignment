using KodisoftTestAssignment.Enumerators;
using System;

namespace KodisoftTestAssignment.Models
{
    public class AtomFeed : Feed
    {

        public AtomFeed()
            : base()
        {
            FeedType = FeedType.Atom;
        }
    }
}
