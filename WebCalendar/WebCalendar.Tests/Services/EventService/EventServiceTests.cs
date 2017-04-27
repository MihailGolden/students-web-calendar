using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WebCalendar.Domain.Aggregate.Calendar;
using WebCalendar.Domain.Aggregate.Event;
using WebCalendar.Tests.Services.CalendarService;

namespace WebCalendar.Tests.Services.EventService
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EventServiceTests
    {
        private const int EVENT_ID = 1;
        private const int CALENDAR_ID = 1;
        private const int FAKE_EVENT_ID = 0;
        private const string UPDATED_TITLE = "Start Js Courses";
        private IKernel kernel;
        private readonly Mock<ICalendarRepository> calendarRepositoryMock = new Mock<ICalendarRepository>();
        private readonly Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();

        [TestInitialize]
        public void Init()
        {
            this.kernel = new StandardKernel();
            this.kernel.Bind<ICalendarRepository>().ToConstant(this.calendarRepositoryMock.Object);
            this.kernel.Bind<IEventRepository>().ToConstant(this.eventRepositoryMock.Object);
        }

        [TestMethod]
        public void Create_ValidEvent_Created()
        {
            //Arrange
            var newEvent = new EventBuilder().Build();
            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            service.Create(newEvent);
            //Assert
            VerifyCreateEvent(Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullEvent_ExceptionThrown()
        {
            //Arrange
            Event newEvent = null;
            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            service.Create(newEvent);
        }

        [TestMethod]
        public void AddEvents_ValidListOfEvents_Added()
        {
            //Arrange
            var events = new EventBuilder().BuildList();
            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            service.AddEvents(events);
            //Assert
            VerifyAddListOfEvents(Times.Exactly(events.Count));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddEvents_NullListOfEvents_ExceptionThrown()
        {
            //Arrange
            List<Event> events = null;
            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            service.AddEvents(events);
        }

        [TestMethod]
        public void Get_ValidEvent_ReturnEvent()
        {
            //Arrange
            Event expected = new EventBuilder().Build();
            SetUpRepository(expected);
            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            var actual = service.Get(EVENT_ID);
            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Get_NullEvent_ExceptionThrown()
        {
            //Arrange
            Event ev = null;
            SetUpRepository(ev);
            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            var actual = service.Get(EVENT_ID);
        }

        [TestMethod]
        public void GetEventsFromCalendar_ValidCalendar_ReturnListOfEvents()
        {
            //Arrange
            var expected = new EventBuilder();
            SetUpRepository();
            SetUpCalendarRepository();

            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            var actual = service.GetEventsFromCalendar(CALENDAR_ID);
            //Assert
            VerifyGetEvents(Times.Once());
        }

        [TestMethod]
        public void GetEvents_ValidEntities_ReturnListOfEvents()
        {
            //Arrange
            var expected = new EventBuilder();
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            var actual = service.GetEvents;
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 1);
            Assert.ReferenceEquals(expected, actual[0]);
        }

        [TestMethod]
        public void Update_ValidEvent_Updated()
        {
            //Arrange
            var ev = new EventBuilder().Build();
            SetUpRepository(ev);
            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            ev.Title = UPDATED_TITLE;
            service.Update(ev);
            var actual = service.Get(EVENT_ID);
            //Assert
            Assert.AreEqual(UPDATED_TITLE, actual.Title);
            Assert.AreEqual(ev, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Update_NullEvent_ExceptionThrown()
        {
            //Arrange
            Event ev = null;
            SetUpRepository(ev);
            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            ev.Title = UPDATED_TITLE;
            service.Update(ev);
        }

        [TestMethod]
        public void Delete_ValidEvent_Deleted()
        {
            //Arrange
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            service.Delete(EVENT_ID);
            //Assert
            VerifyDeleteEvent(EVENT_ID, Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_NotExistEvent_ExceptionThrown()
        {
            //Arrange
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.EventService>();
            //Act
            service.Delete(FAKE_EVENT_ID);
        }

        private void SetUpRepository()
        {
            this.eventRepositoryMock
                 .Setup(m => m.Entities)
                 .Returns(new Event[] { new EventBuilder().Build() }.AsQueryable());
        }

        private void SetUpCalendarRepository()
        {
            this.calendarRepositoryMock.Setup(m => m.Entities)
     .Returns(new Calendar[] { new CalendarBuilder().Build() }.AsQueryable());
        }

        private void VerifyGetEvents(Times times)
        {
            this.eventRepositoryMock.Verify(m => m.GetEvents(It.IsAny<Calendar>()), times, "Can't call GetEvents");
        }

        private void VerifyDeleteEvent(int eventID, Times times)
        {
            this.eventRepositoryMock.Verify(m => m.Delete(It.Is<int>(id => id == eventID)), times, "Can't delete");
        }

        private void SetUpRepository(Event expected)
        {
            this.eventRepositoryMock
                .Setup(m => m.Entities)
                .Returns(new Event[] { expected }.AsQueryable());
        }

        private void VerifyAddListOfEvents(Times times)
        {
            this.eventRepositoryMock.Verify(
               m => m.Add(It.IsAny<Event>()), times, "Can't add list");
        }

        private void VerifyCreateEvent(Times times)
        {
            this.eventRepositoryMock.Verify(
               m => m.Add(It.IsAny<Event>()), times, "Event was not created");
        }
    }
}

