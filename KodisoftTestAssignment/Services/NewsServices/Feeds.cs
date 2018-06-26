using KodisoftTestAssignment.Models;
using KodisoftTestAssignment.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace KodisoftTestAssignment.Services
{
    public partial class NewsServices
    {
        public Feed GetFeed(int id)
        {
            return _dbContext.Feeds.FirstOrDefault(f => f.ID == id);
        }

        public void AddFeed(string url, FeedType feedType)
        {
            switch (feedType)
            {
                case FeedType.RSS:
                    AddRssFeed(url);
                    break;
                case FeedType.Atom:
                    AddAtomFeed(url);
                    break;
                default:
                    throw new NotSupportedException(string.Format("{0} is not supported", feedType.ToString()));
            }
        }

        public void AddRssFeed(string url)
        {
            XDocument doc = XDocument.Load(url);
            var feed = from item in doc.Root.Elements()
                       select new RssFeed
                       {
                           Link = item.Elements().First(i => i.Name.LocalName == "link").Attribute("href").Value,
                           Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                       };
            _dbContext.Feeds.Add(feed.FirstOrDefault());
        }

        public void AddAtomFeed(string url)
        {
            XDocument doc = XDocument.Load(url);
            var feed = from item in doc.Root.Elements()
                       select new AtomFeed
                       {
                           Link = item.Elements().First(i => i.Name.LocalName == "link").Attribute("href").Value,
                           Title = item.Elements().First(i => i.Name.LocalName == "title").Value,
                       };

        }

        public IEnumerable<Item> ParseItems(string url, FeedType feedType)
        {
            switch (feedType)
            {
                case FeedType.RSS:
                    return ParseRssItems(url);
                case FeedType.Atom:
                    return ParseAtomItems(url);
                default:
                    throw new NotSupportedException(string.Format("{0} is not supported", feedType.ToString()));
            }
        }

        public virtual IEnumerable<Item> ParseAtomItems(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                var entries = from item in doc.Root.Elements().Where(i => i.Name.LocalName == "entry")
                              select new Item
                              {
                                  Content = item.Elements().First(i => i.Name.LocalName == "content").Value,
                                  Link = item.Elements().First(i => i.Name.LocalName == "link").Attribute("href").Value,
                                  PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "published").Value),
                                  Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                              };
                return entries.ToList();
            }
            catch
            {
                return new List<Item>();
            }
        }

        public virtual IEnumerable<Item> ParseRssItems(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                              select new Item
                              {
                                  Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
                                  Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
                                  PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "pubDate").Value),
                                  Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                              };
                return entries.ToList();
            }
            catch
            {
                return new List<Item>();
            }
        }

        private DateTime ParseDate(string date)
        {
            DateTime result;
            if (DateTime.TryParse(date, out result))
                return result;
            else
                return DateTime.MinValue;
        }
    }
}
