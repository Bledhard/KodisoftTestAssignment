using KodisoftTestAssignment.Models;
using KodisoftTestAssignment.Requests;
using KodisoftTestAssignment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Services.Feeds
{
    [TestClass]
    public class GetFeedCollectionTests
    {
        [TestMethod]
        public void ValidID_ReturnsFeedCollection()
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

            var service = new NewsServices(mockContext.Object, loggerMock.Object);

            var request = new GetFeedCollectionRequest
            {
                UserId = "first",
                FeedCollectionID = 1
            };

            // Act
            var FC = service.GetFeedCollection(request);

            // Assert
            Assert.IsNotNull(FC);
            Assert.AreEqual("First_Testing_FC", FC.Title);
        }
    }
}
