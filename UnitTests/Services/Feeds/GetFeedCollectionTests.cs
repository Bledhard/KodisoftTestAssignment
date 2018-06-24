using KodisoftTestAssignment.Models;
using Microsoft.EntityFrameworkCore;
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
                new FeedCollection { ID = 1, Title = "First_Testing_FC" },
                new FeedCollection { ID = 2, Title = "Second_Testing_FC" },
                new FeedCollection { ID = 3, Title = "Third_Testing_FC" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<FeedCollection>>();

            mockSet.As<IQueryable<FeedCollection>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<FeedCollection>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<FeedCollection>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<FeedCollection>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MainAppDbContext>();
            mockContext.Setup(c => c.FeedCollections).Returns(mockSet.Object);

            var service = new KodisoftTestAssignment.Services.NewsServices(mockContext.Object);

            // Act
            var FC = service.GetFeedCollection(1);

            // Assert
            Assert.IsNotNull(FC);
            Assert.AreEqual("First_Testing_FC", FC.Title);
        }
    }
}
