using KodisoftTestAssignment.Enumerators;
using KodisoftTestAssignment.Interfaces;
using KodisoftTestAssignment.Models;
using KodisoftTestAssignment.Requests;
using KodisoftTestAssignment.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class AddFeedToCollectionTests
    {
        private Mock<INewsRepository> NewsRepoMock;
        private Mock<ILogger<NewsServices>> LoggerMock;
        private Mock<IMemoryCache> CacheMock;
        private NewsServices Service;

        public AddFeedToCollectionTests()
        {
            NewsRepoMock = new Mock<INewsRepository>();
            LoggerMock = new Mock<ILogger<NewsServices>>();
            CacheMock = new Mock<IMemoryCache>();
            Service = new NewsServices(NewsRepoMock.Object, LoggerMock.Object, CacheMock.Object);
        }

        [TestMethod]
        public void ValidFeed_ValidFeedCollection_Success()
        {
            // Arrange
            var userId = "first";
            var feedCollectionTitle = "First_Testing_FC";
            var feedLink = "testing link 1 for feed 1";

            var feedCollections = new List<FeedCollection>
            {
                new FeedCollection { ID = 1, Title = feedCollectionTitle, UserID = userId }
            };

            var feed = new Feed { ID = 1, Link = feedLink, FeedType = FeedType.Atom };

            NewsRepoMock
                .Setup(repo => repo.GetUserFeedCollections(userId))
                .Returns(feedCollections);

            NewsRepoMock
                .Setup(repo => repo.GetFeedCollection(1))
                .Returns(feedCollections[0]);

            NewsRepoMock
                .Setup(repo => repo.GetFeed(1))
                .Returns(feed);

            var request = new AddFeedToCollectionRequest
            {
                UserId = userId,
                FeedCollectionId = feedCollections[0].ID,
                FeedId = feed.ID
            };

            // Act
            Service.AddFeedToCollection(request);

            // Assert
            NewsRepoMock.Verify(repo => repo.Add(feed.ID, feedCollections[0].ID));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "AddFeedToCollection: method didn't throw expected exception, test failed.")]
        public void InvalidFeedCollection_ThrowArgumentException()
        {
            // Arrange
            var userId = "first";
            var feedLink = "testing link 1 for feed 1";

            var feeds = new List<Feed>
            {
                new Feed { ID = 1, Link = feedLink, FeedType = FeedType.Atom}
            };

            NewsRepoMock
                .Setup(repo => repo.GetUserFeedCollections(userId))
                .Returns(new List<FeedCollection>());

            NewsRepoMock
                .Setup(repo => repo.GetFeed(1))
                .Returns(feeds[0]);

            var request = new AddFeedToCollectionRequest
            {
                UserId = userId,
                FeedCollectionId = 1,
                FeedId = feeds[0].ID
            };

            // Act
            Service.AddFeedToCollection(request);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "AddFeedToCollection: method didn't throw expected exception, test failed.")]
        public void InvalidFeed_ThrowArgumentException()
        {
            // Arrange
            var userId = "first";
            var feedCollectionTitle = "First_Testing_FC";

            var feedCollections = new List<FeedCollection>
            {
                new FeedCollection { ID = 1, Title = feedCollectionTitle, UserID = userId }
            };

            NewsRepoMock
                .Setup(repo => repo.GetUserFeedCollections(userId))
                .Returns(feedCollections);

            NewsRepoMock
                .Setup(repo => repo.GetFeed(1))
                .Returns(new Feed());

            var request = new AddFeedToCollectionRequest
            {
                UserId = userId,
                FeedCollectionId = feedCollections[0].ID,
                FeedId = 1
            };

            // Act
            Service.AddFeedToCollection(request);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateWaitObjectException),
            "AddFeedToCollection: method didn't throw expected exception, test failed.")]
        public void FeedCollectionContainsFeed_ThrowArgumentException()
        {
            // Arrange
            var userId = "first";
            var feedCollectionTitle = "First_Testing_FC";
            var feedLink = "testing link 1 for feed 1";

            var feedCollections = new List<FeedCollection>
            {
                new FeedCollection { ID = 1, Title = feedCollectionTitle, UserID = userId }
            };

            var feeds = new List<Feed>
            {
                new Feed { ID = 1, Link = feedLink, FeedType = FeedType.Atom}
            };

            NewsRepoMock
                .Setup(repo => repo.Contains(1, 1))
                .Returns(true);

            NewsRepoMock
                .Setup(repo => repo.GetFeedCollection(feedCollections[0].ID))
                .Returns(feedCollections[0]);

            NewsRepoMock
                .Setup(repo => repo.GetFeed(feeds[0].ID))
                .Returns(feeds[0]);

            var request = new AddFeedToCollectionRequest
            {
                UserId = userId,
                FeedCollectionId = feeds[0].ID,
                FeedId = feeds[0].ID
            };

            // Act
            Service.AddFeedToCollection(request);

            // Assert
            Assert.Fail();
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "AddFeedToCollection: method didn't throw expected exception, test failed.")]
        public void InvalidFeedCollectionID_ThrowArgumentException()
        {
            // Arrange
            var userId = "first";
            var request = new AddFeedToCollectionRequest
            {
                UserId = userId,
                FeedId = 1
            };

            // Act
            Service.AddFeedToCollection(request);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "AddFeedToCollection: method didn't throw expected exception, test failed.")]
        public void InvalidFeedId_ThrowArgumentException()
        {
            // Arrange
            var userId = "first";
            var request = new AddFeedToCollectionRequest
            {
                UserId = userId,
                FeedCollectionId = 1,
            };

            // Act
            Service.AddFeedToCollection(request);

            // Assert
            Assert.Fail();
        }
    }
}
