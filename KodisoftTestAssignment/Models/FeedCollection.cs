using KodisoftTestAssignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Models
{
    public class FeedCollection
    {
        public string Name { get; set; }

        public List<IFeed> Feeds { get; set; }
    }
}
