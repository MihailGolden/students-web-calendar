using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WebCalendar.Contracts;
using WebCalendar.Domain.Aggregate.Calendar;

namespace WebCalendar.Tests.Services.CalendarService
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CalendarServiceTests
    {
        private const int CALENDAR_ID = 1;
        private const int FAKE_CALENDAR_ID = 0;
        private const string USER_ID = "f36ef390-6b03-46e1-8138-33c534ff8de4";
        private const string UPDATED_TITLE = "Start Js Courses";
        private IKernel kernel;
        private readonly Mock<IUserService> userServiceMock = new Mock<IUserService>();
        private readonly Mock<ICalendarRepository> calendarRepositoryMock = new Mock<ICalendarRepository>();

        [TestInitialize]
        public void Init()
        {
            this.kernel = new StandardKernel();
            this.kernel.Bind<ICalendarRepository>().ToConstant(this.calendarRepositoryMock.Object);
            this.kernel.Bind<IUserService>().ToConstant(this.userServiceMock.Object);
        }

        [TestMethod]
        public void Create_ValidCalendar_Created()
        {
            //Arrange
            var newCalendar = new CalendarBuilder().Build();
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            service.Create(newCalendar);
            //Assert
            VerifyCreateCalendar(Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullCalendar_ExceptionThrown()
        {
            //Arrange
            Calendar newCalendar = null;
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            service.Create(newCalendar);
        }

        [TestMethod]
        public void AddCalendars_ValidListOfCalendars_Added()
        {
            //Arrange
            var cals = new CalendarBuilder().BuildList();
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            service.AddCalendars(cals);
            //Assert
            VerifyAddListOfCalendars(Times.Exactly(cals.Count));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddCalendars_NullListOfCalendars_ExceptionThrown()
        {
            //Arrange
            List<Calendar> cals = null;
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            service.AddCalendars(cals);
        }

        [TestMethod]
        public void Get_ValidCalendar_ReturnCalendar()
        {
            //Arrange
            Calendar expected = new CalendarBuilder().Build();
            SetUpRepository(expected);
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            var actual = service.Get(CALENDAR_ID);
            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Get_NullCalendar_ExceptionThrown()
        {
            //Arrange
            Calendar cal = null;
            SetUpRepository(cal);
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            var actual = service.Get(CALENDAR_ID);
        }

        [TestMethod]
        public void GetUserCalendars_ValidUser_ReturnListOfCalendars()
        {
            //Arrange
            var expected = new CalendarBuilder().WithUserID(USER_ID);
            SetUpUserService();
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            var actual = service.GetUserCalendars();
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 1);
            Assert.ReferenceEquals(expected, actual[0]);
        }

        [TestMethod]
        public void GetCalendars_ValidEntities_ReturnListOfCalendars()
        {
            //Arrange
            var expected = new CalendarBuilder().WithUserID(USER_ID);
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            var actual = service.GetCalendars;
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 1);
            Assert.ReferenceEquals(expected, actual[0]);
        }

        [TestMethod]
        public void Update_ValidCalendar_Updated()
        {
            //Arrange
            var cal = new CalendarBuilder().WithUserID(USER_ID);
            SetUpRepository(cal);
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            cal.Title = UPDATED_TITLE;
            service.Update(cal);
            var actual = service.Get(CALENDAR_ID);
            //Assert
            Assert.AreEqual(UPDATED_TITLE, actual.Title);
            Assert.AreEqual(cal, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Update_NullCalendar_ExceptionThrown()
        {
            //Arrange
            Calendar cal = null;
            SetUpRepository(cal);
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            cal.Title = UPDATED_TITLE;
            service.Update(cal);
        }

        [TestMethod]
        public void Delete_ValidCalendar_Deleted()
        {
            //Arrange
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            service.Delete(CALENDAR_ID);
            //Assert
            VerifyDeleteCalendar(CALENDAR_ID, Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_NotExistCalendar_ExceptionThrown()
        {
            //Arrange
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.CalendarService>();
            //Act
            service.Delete(FAKE_CALENDAR_ID);
        }

        private void SetUpUserService()
        {
            this.userServiceMock.Setup(m => m.GetUserID()).Returns(USER_ID);
        }

        private void SetUpRepository(Calendar cal)
        {
            this.calendarRepositoryMock
                .Setup(m => m.Entities)
                .Returns(new Calendar[] { cal }.AsQueryable());
        }

        private void SetUpRepository()
        {
            this.calendarRepositoryMock
                .Setup(m => m.Entities)
                .Returns(new Calendar[] { new CalendarBuilder().WithUserID(USER_ID) }.AsQueryable());
        }

        private void VerifyCreateCalendar(Times times)
        {
            this.calendarRepositoryMock.Verify(
                m => m.Add(It.IsAny<Calendar>()), times, "Calendar was not created");
        }
        private void VerifyAddListOfCalendars(Times times)
        {
            this.calendarRepositoryMock.Verify(
                m => m.Add(It.IsAny<Calendar>()), times, "Can't add list");
        }

        private void VerifyDeleteCalendar(int calendarID, Times time)
        {
            this.calendarRepositoryMock.Verify(m => m.Delete(It.Is<int>(id => id == calendarID)), time, "Can't delete");
        }
    }
}
