using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.EntityFrameworkCore;
using KodisoftTestAssignment.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using KodisoftTestAssignment.Services;
using KodisoftTestAssignment.Requests;
using Microsoft.Extensions.Caching.Memory;

namespace UnitTests.Services.Feeds
{
    [TestClass]
    public class GetUserFeedCollectionsTests
    {
        [TestMethod]
        public void MethodReturnedDesired()
        {
            // Arrange
            var data = new List<FeedCollection>
            {
                new FeedCollection { ID = 1, Title = "First_Testing_FC", UserID = "first" },
                new FeedCollection { ID = 2, Title = "Second_Testing_FC", UserID = "first" },
                new FeedCollection { ID = 3, Title = "Third_Testing_FC", UserID = "second" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<FeedCollection>>();

            mockSet.As<IQueryable<FeedCollection>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<FeedCollection>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<FeedCollection>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<FeedCollection>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MainAppDbContext>();
            mockContext.Setup(c => c.FeedCollections).Returns(mockSet.Object);
            var loggerMock = new Mock<ILogger<NewsServices>>();
            var memoryCacheMock = new Mock<IMemoryCache>();

            var service = new NewsServices(mockContext.Object, loggerMock.Object, memoryCacheMock.Object);

            var request = new GetUserFeedCollectionsRequest
            {
                UserId = "first"
            };

            // Act
            var FCs = service.GetUserFeedCollections(request);

            // Assert
            Assert.IsNotNull(FCs);
            Assert.AreEqual(2, FCs.Count);
            Assert.AreEqual("First_Testing_FC", FCs[0].Title);
            Assert.AreEqual("Second_Testing_FC", FCs[1].Title);
            Assert.AreEqual("Third_Testing_FC", FCs[2].Title);
        }
    }
}
