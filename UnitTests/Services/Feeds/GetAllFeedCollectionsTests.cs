using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.EntityFrameworkCore;
using KodisoftTestAssignment.Models;
using System.Linq;

namespace UnitTests.Services.Feeds
{
    [TestClass]
    public class GetAllFeedCollectionsTests
    {
        [TestMethod]
        public void MethodReturnedDesired()
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
            var FCs = service.GetAllFeedCollections();

            // Assert
            Assert.IsNotNull(FCs);
            Assert.AreEqual(3, FCs.Count);
            Assert.AreEqual("First_Testing_FC", FCs[0].Title);
            Assert.AreEqual("Second_Testing_FC", FCs[1].Title);
            Assert.AreEqual("Third_Testing_FC", FCs[2].Title);
        }
    }
}
