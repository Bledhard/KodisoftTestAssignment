using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Models
{
    public class Feed
    {
        public string Name { get; set; }

        public List<NewsProvider> NewsProviders { get; set; }
    }
}
