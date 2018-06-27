using KodisoftTestAssignment.Models;
using KodisoftTestAssignment.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace KodisoftTestAssignment.Services
{
    public partial class NewsServices
    {
        private FeedType GetFeedType(string url)
        {
            XDocument doc = XDocument.Load(url);
            foreach (var element in doc.Elements())
            {
                if (element.Name.LocalName == "rss")
                {
                    return FeedType.RSS;
                }
                if (element.Name.LocalName == "feed")
                {
                    return FeedType.Atom;
                }
            }
            throw new ArgumentException("Invalid type of feed: " + url + ";");
        }

        public void AddFeed(string url)
        {
            try
            {
                var feedType = GetFeedType(url);

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
            catch (ArgumentException e)
            {
                _logger.LogError("Error thrown from GetFeedType(string): " + e.Message);
            }
        }

        private void AddRssFeed(string url)
        {
            XDocument doc = XDocument.Load(url);
            var feed = from item in doc.Root.Elements()
                       select new RssFeed
                       {
                           Link = url,
                           Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                       };
            _dbContext.Feeds.Add(feed.FirstOrDefault());
            _dbContext.SaveChanges();
        }

        private void AddAtomFeed(string url)
        {
            XDocument doc = XDocument.Load(url);
            var feed = new AtomFeed
                       {
                           Link = url,
                           Title = doc.Root.Elements().First(i => i.Name.LocalName == "title").Value,
                       };
            _dbContext.Feeds.Add(feed);
            _dbContext.SaveChanges();
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

        private IEnumerable<Item> ParseAtomItems(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                var entries = from item in doc.Root.Elements().Where(i => i.Name.LocalName == "entry")
                              select new Item
                              {
                                  Content = item.Elements().FirstOrDefault(i => i.Name.LocalName == "content")?.Value,
                                  Link = item.Elements().FirstOrDefault(i => i.Name.LocalName == "link")?.Attribute("href")?.Value,
                                  PublishDate = ParseDate(item.Elements().FirstOrDefault(i => i.Name.LocalName == "published")?.Value),
                                  Title = item.Elements().FirstOrDefault(i => i.Name.LocalName == "title")?.Value
                              };
                return entries.ToList();
            }
            catch
            {
                return new List<Item>();
            }
        }

        private IEnumerable<Item> ParseRssItems(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                              select new Item
                              {
                                  Content = item.Elements().FirstOrDefault(i => i.Name.LocalName == "description")?.Value,
                                  Link = item.Elements().FirstOrDefault(i => i.Name.LocalName == "link")?.Value,
                                  PublishDate = ParseDate(item.Elements().FirstOrDefault(i => i.Name.LocalName == "pubDate")?.Value),
                                  Title = item.Elements().FirstOrDefault(i => i.Name.LocalName == "title")?.Value
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
            if (DateTime.TryParse(date, out DateTime result))
                return result;
            else
                return DateTime.MinValue;
        }
    }
}
