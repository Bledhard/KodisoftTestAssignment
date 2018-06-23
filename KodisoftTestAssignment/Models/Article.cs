using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Models
{
    public class Article
    {
        public RssFeed Source { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        [JsonProperty("urlToImage")]
        public string ImageUrl { get; set; }

        [JsonProperty("publishedAt")]
        public DateTime Published { get; set; }
        
        public static Article DeserialiseJson(string json)
        {
            Article article = JsonConvert.DeserializeObject<Article>(json);

            System.Diagnostics.Debug.WriteLine(article);

            return article;
        }
    }
}
