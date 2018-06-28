using KodisoftTestAssignment.Models;
using KodisoftTestAssignment.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

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
            throw new FormatException("Invalid type of feed: " + url + ".");
        }

        public int AddFeed(string url)
        {
            try
            {
                if (_newsRepository.Contains(url))
                    {
                    throw new DuplicateWaitObjectException("This feed is already in database");
                }

                var feedType = GetFeedType(url);

                switch (feedType)
                {
                    case FeedType.RSS:
                        return AddRssFeed(url);
                    case FeedType.Atom:
                        return AddAtomFeed(url);
                    default:
                        throw new NotSupportedException(string.Format("{0} is not supported", feedType.ToString()));
                }
            }
            catch (NotSupportedException e)
            {
                _logger.LogError("AddFeed(string): threw NotSupportedException: " + e.Message);
                return 0;
            }
            catch (DuplicateWaitObjectException e)
            {
                _logger.LogError("AddFeed(string): threw DuplicateWaitObjectException: " + e.Message);
                return 0;
            }
            catch (FormatException e)
            {
                _logger.LogError("GetFeedType(string): threw FormatException: " + e.Message);
                return 0;
            }
        }

        private int AddRssFeed(string url)
        {
            XDocument doc = XDocument.Load(url);
            var feed = new RssFeed
            {
                Link = url,
                Title = doc.Root.Elements().Elements().First(i => i.Name.LocalName == "title").Value
            };
            _newsRepository.Add(feed);
            var n = _newsRepository.SaveChanges();

            if (n > 0)
            {
                _cache.Set("feed_" + feed.ID, feed, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });
            }

            return feed.ID;
        }

        private int AddAtomFeed(string url)
        {
            XDocument doc = XDocument.Load(url);
            var feed = new AtomFeed
            {
                Link = url,
                Title = doc.Root.Elements().First(i => i.Name.LocalName == "title").Value,
            };
            _newsRepository.Add(feed);
            var n = _newsRepository.SaveChanges();

            if (n > 0)
            {
                _cache.Set("feed_" + feed.ID, feed, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });
            }

            return feed.ID;
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
