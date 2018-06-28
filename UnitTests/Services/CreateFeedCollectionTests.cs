using KodisoftTestAssignment.Enumerators;
using KodisoftTestAssignment.Interfaces;
using KodisoftTestAssignment.Models;
using KodisoftTestAssignment.Requests;
using KodisoftTestAssignment.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class CreateFeedCollectionTests
    {
        private Mock<INewsRepository> NewsRepoMock;
        private Mock<ILogger<NewsServices>> LoggerMock;
        private Mock<IMemoryCache> CacheMock;
        private NewsServices Service;

        public CreateFeedCollectionTests()
        {
            NewsRepoMock = new Mock<INewsRepository>();
            LoggerMock = new Mock<ILogger<NewsServices>>();
            CacheMock = new Mock<IMemoryCache>();
            Service = new NewsServices(NewsRepoMock.Object, LoggerMock.Object, CacheMock.Object);
        }

        [TestMethod]
        public void ValidTitle_ReturnFeedCollectionId()
        {
            // Arrange
            var userId = "first";
            var title = "FeedCollectionTestTitle";
            var feedCollections = new List<FeedCollection>();

            NewsRepoMock
                .Setup(repo => repo.Add(It.IsAny<FeedCollection>()))
                .Callback<FeedCollection>(feedCollections.Add);

            var request = new CreateFeedCollectionRequest
            {
                UserId = userId,
                Title = title
            };

            // Act
            var fcID = Service.CreateFeedCollection(request);

            // Assert
            Assert.AreEqual(fcID, feedCollections[0].ID);
            NewsRepoMock.Verify(repo => repo.Add(feedCollections[0]));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "CreateFeedCollection: method didn't throw expected exception, test failed.")]
        public void InvalidTitle_ThrowArgumentException()
        {
            // Arrange
            var userId = "first";
            var feedCollections = new List<FeedCollection>();

            NewsRepoMock
                .Setup(repo => repo.Add(It.IsAny<FeedCollection>()))
                .Callback<FeedCollection>(feedCollections.Add);

            var request = new CreateFeedCollectionRequest
            {
                UserId = userId
            };

            // Act
            var fcID = Service.CreateFeedCollection(request);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateWaitObjectException),
            "CreateFeedCollection: method didn't throw expected exception, test failed.")]
        public void DuplicateTitle_ThrowArgumentException()
        {
            // Arrange
            var userId = "first";
            var title = "FeedCollectionTestTitle";
            var feedCollections = new List<FeedCollection>();

            NewsRepoMock
                .Setup(repo => repo.Add(It.IsAny<FeedCollection>()))
                .Callback<FeedCollection>(feedCollections.Add);

            NewsRepoMock
                .Setup(repo => repo.Contains(title, userId))
                .Returns(true);

            var request = new CreateFeedCollectionRequest
            {
                UserId = userId,
                Title = title
            };

            // Act
            var fcID = Service.CreateFeedCollection(request);

            // Assert
            Assert.Fail();
        }
    }
}
