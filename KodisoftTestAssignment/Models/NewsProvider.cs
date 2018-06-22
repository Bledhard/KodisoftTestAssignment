using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Models
{
    public class NewsProvider
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<Article> CachedArticles { get; set; }
    }
}
