using KodisoftTestAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Services
{
    public partial class NewsServices
    {
        public void AddFeedToCollection(int collectionId, string feedId)
        {
            throw new NotImplementedException();
        }

        public int CreateFeedCollection(string title)
        {
            _context.FeedCollections.Add(new FeedCollection
            {
                Title = title
            });

            return _context.FeedCollections.FirstOrDefault(fc => fc.Title == title).ID;
        }

        public List<FeedCollection> GetAllFeedCollections()
        {
            return _context.FeedCollections.ToList();
        }

        public FeedCollection GetFeedCollection(int id)
        {
            return _context.FeedCollections.FirstOrDefault(fc => fc.ID == id);
        }
    }
}
