using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WebCalendar.Domain.Aggregate.Occurrence;

namespace WebCalendar.Tests.Services.OccurrenceService
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class OccurrenceServiceTests
    {
        private const int OCCURRENCE_ID = 1;
        private const int FAKE_OCCURRENCE_ID = 0;
        private const int UPDATED_COUNT = 2;
        private IKernel kernel;
        private readonly Mock<IOccurrenceRepository> occurrenceRepositoryMock = new Mock<IOccurrenceRepository>();

        [TestInitialize]
        public void Init()
        {
            this.kernel = new StandardKernel();
            this.kernel.Bind<IOccurrenceRepository>().ToConstant(this.occurrenceRepositoryMock.Object);
        }

        [TestMethod]
        public void Create_ValidOccurrence_Created()
        {
            //Arrange
            var newOccurrence = new OccurrenceBuilder().Build();
            var service = this.kernel.Get<WebCalendar.Services.OccurrenceService>();
            //Act
            service.Create(newOccurrence);
            //Assert
            VerifyCreateOccurrence(Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullOccurrence_ExceptionThrown()
        {
            //Arrange
            Occurrence newOccurrence = null;
            var service = this.kernel.Get<WebCalendar.Services.OccurrenceService>();
            //Act
            service.Create(newOccurrence);
        }

        [TestMethod]
        public void AddOccurrences_ValidListOfOccurrences_Added()
        {
            //Arrange
            var occurrences = new OccurrenceBuilder().BuildList();
            var service = this.kernel.Get<WebCalendar.Services.OccurrenceService>();
            //Act
            service.AddOccurrences(occurrences);
            //Assert
            VerifyAddListOfOccurrences(Times.Exactly(occurrences.Count));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddOccurrences_NullListOfOccurrences_ExceptionThrown()
        {
            //Arrange
            List<Occurrence> occurrences = null;
            var service = this.kernel.Get<WebCalendar.Services.OccurrenceService>();
            //Act
            service.AddOccurrences(occurrences);
        }

        [TestMethod]
        public void Get_ValidOccurrence_ReturnOccurrence()
        {
            //Arrange
            Occurrence expected = new OccurrenceBuilder().Build();
            SetUpRepository(expected);
            var service = this.kernel.Get<WebCalendar.Services.OccurrenceService>();
            //Act
            var actual = service.Get(OCCURRENCE_ID);
            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Get_NullOccurrence_ExceptionThrown()
        {
            //Arrange
            Occurrence occurrence = null;
            SetUpRepository(occurrence);
            var service = this.kernel.Get<WebCalendar.Services.OccurrenceService>();
            //Act
            var actual = service.Get(OCCURRENCE_ID);
        }

        [TestMethod]
        public void GetOccurrences_ValidEntities_ReturnListOfOccurrences()
        {
            //Arrange
            var expected = new OccurrenceBuilder().BuildList();
            SetUpListRepository(expected);
            var service = this.kernel.Get<WebCalendar.Services.OccurrenceService>();
            //Act
            var actual = service.GetOccurrences;
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 1);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Update_ValidOccurrence_Updated()
        {
            //Arrange
            var occurrence = new OccurrenceBuilder().Build();
            SetUpRepository(occurrence);
            var service = this.kernel.Get<WebCalendar.Services.OccurrenceService>();
            //Act
            occurrence.Count = UPDATED_COUNT;
            service.Update(occurrence);
            var actual = service.Get(OCCURRENCE_ID);
            //Assert
            Assert.AreEqual(UPDATED_COUNT, occurrence.Count);
            Assert.AreEqual(occurrence, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Update_NullOccurrence_ExceptionThrown()
        {
            //Arrange
            Occurrence occurrence = null;
            SetUpRepository(occurrence);
            var service = this.kernel.Get<WebCalendar.Services.OccurrenceService>();
            //Act
            occurrence.Count = UPDATED_COUNT;
            service.Update(occurrence);
        }

        [TestMethod]
        public void Delete_ValidOccurrence_Deleted()
        {
            //Arrange
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.OccurrenceService>();
            //Act
            service.Delete(OCCURRENCE_ID);
            //Assert
            VerifyDeleteOccurrence(OCCURRENCE_ID, Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_NotExistOccurrence_ExceptionThrown()
        {
            //Arrange
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.OccurrenceService>();
            //Act
            service.Delete(FAKE_OCCURRENCE_ID);
        }

        private void SetUpRepository()
        {
            this.occurrenceRepositoryMock
                 .Setup(m => m.Entities)
                 .Returns(new Occurrence[] { new OccurrenceBuilder().Build() }.AsQueryable());
        }
        private void SetUpListRepository(List<Occurrence> list)
        {
            this.occurrenceRepositoryMock
                 .Setup(m => m.Entities).Returns(list.AsQueryable());
        }

        private void VerifyDeleteOccurrence(int occurrenceID, Times times)
        {
            this.occurrenceRepositoryMock.Verify(m => m.Delete(It.Is<int>(id => id == occurrenceID)), times, "Can't delete");
        }

        private void SetUpRepository(Occurrence expected)
        {
            this.occurrenceRepositoryMock
                .Setup(m => m.Entities)
                .Returns(new Occurrence[] { expected }.AsQueryable());
        }

        private void VerifyAddListOfOccurrences(Times times)
        {
            this.occurrenceRepositoryMock.Verify(
               m => m.Add(It.IsAny<Occurrence>()), times, "Can't add list");
        }

        private void VerifyCreateOccurrence(Times times)
        {
            this.occurrenceRepositoryMock.Verify(
               m => m.Add(It.IsAny<Occurrence>()), times, "Occurrence was not created");
        }
    }
}
