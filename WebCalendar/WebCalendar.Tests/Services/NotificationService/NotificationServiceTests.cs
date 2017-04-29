using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WebCalendar.Domain.Aggregate.Event;
using WebCalendar.Domain.Aggregate.Notification;

namespace WebCalendar.Tests.Services.NotificationService
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class NotificationServiceTests
    {
        private const int NOTIFICATION_ID = 1;
        private const int EVENT_ID = 1;
        private const int FAKE_NOTIFICATION_ID = 0;
        private const string UPDATED_TYPE = "email";
        private IKernel kernel;
        private readonly Mock<INotificationRepository> notificationRepositoryMock = new Mock<INotificationRepository>();
        private readonly Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();

        [TestInitialize]
        public void Init()
        {
            this.kernel = new StandardKernel();
            this.kernel.Bind<INotificationRepository>().ToConstant(this.notificationRepositoryMock.Object);
            this.kernel.Bind<IEventRepository>().ToConstant(this.eventRepositoryMock.Object);
        }

        [TestMethod]
        public void Create_ValidNotification_Created()
        {
            //Arrange
            var newNotify = new NotificationBuilder().Build();
            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            service.Create(newNotify);
            //Assert
            VerifyCreateNotification(Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullNotification_ExceptionThrown()
        {
            //Arrange
            Notification newNotification = null;
            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            service.Create(newNotification);
        }

        [TestMethod]
        public void AddNotifications_ValidListOfNotifications_Added()
        {
            //Arrange
            var notifies = new NotificationBuilder().BuildList();
            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            service.AddNotifications(notifies);
            //Assert
            VerifyAddListOfNotifications(Times.Exactly(notifies.Count));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNotifications_NullListOfNotifications_ExceptionThrown()
        {
            //Arrange
            List<Notification> notifies = null;
            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            service.AddNotifications(notifies);
        }

        [TestMethod]
        public void GetNotificationFromEvent()
        {
            //Arrange
            var expected = new NotificationBuilder().Build();
            SetUpRepository(expected);
            SetUpEventRepository();

            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            var actual = service.GetNotificationFromEvent(EVENT_ID);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Get_ValidNotification_ReturnNotification()
        {
            //Arrange
            Notification expected = new NotificationBuilder().Build();
            SetUpRepository(expected);
            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            var actual = service.Get(NOTIFICATION_ID);
            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Get_NullNotification_ExceptionThrown()
        {
            //Arrange
            Notification notify = null;
            SetUpRepository(notify);
            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            var actual = service.Get(NOTIFICATION_ID);
        }


        [TestMethod]
        public void GetNotifications_ValidEntities_ReturnListOfNotifications()
        {
            //Arrange
            var expected = new NotificationBuilder();
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            var actual = service.GetNotifications;
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 1);
            Assert.ReferenceEquals(expected, actual[0]);
        }

        [TestMethod]
        public void Update_ValidNotification_Updated()
        {
            //Arrange
            var notify = new NotificationBuilder().Build();
            SetUpRepository(notify);
            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            notify.Type = UPDATED_TYPE;
            service.Update(notify);
            var actual = service.Get(NOTIFICATION_ID);
            //Assert
            Assert.AreEqual(UPDATED_TYPE, actual.Type);
            Assert.AreEqual(notify, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Update_NullNotification_ExceptionThrown()
        {
            //Arrange
            Notification notify = null;
            SetUpRepository(notify);
            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            notify.Type = UPDATED_TYPE;
            service.Update(notify);
        }

        [TestMethod]
        public void Delete_ValidNotification_Deleted()
        {
            //Arrange
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            service.Delete(NOTIFICATION_ID);
            //Assert
            VerifyDeleteNotification(NOTIFICATION_ID, Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_NotExistNotification_ExceptionThrown()
        {
            //Arrange
            SetUpRepository();
            var service = this.kernel.Get<WebCalendar.Services.NotificationService>();
            //Act
            service.Delete(FAKE_NOTIFICATION_ID);
        }

        private void SetUpRepository()
        {
            this.notificationRepositoryMock
                 .Setup(m => m.Entities)
                 .Returns(new Notification[] { new NotificationBuilder().Build() }.AsQueryable());
        }

        private void VerifyDeleteNotification(int notifyID, Times times)
        {
            this.notificationRepositoryMock.Verify(m => m.Delete(It.Is<int>(id => id == notifyID)), times, "Can't delete");
        }

        private void SetUpRepository(Notification expected)
        {
            this.notificationRepositoryMock
                .Setup(m => m.Entities)
                .Returns(new Notification[] { expected }.AsQueryable());
        }

        private void SetUpEventRepository()
        {
            this.eventRepositoryMock
                .Setup(m => m.Entities)
                .Returns(new Event[] { new Event { ID = 1, Title = "Go to Work" } }.AsQueryable());
        }

        private void VerifyAddListOfNotifications(Times times)
        {
            this.notificationRepositoryMock.Verify(
               m => m.Add(It.IsAny<Notification>()), times, "Can't add list");
        }

        private void VerifyCreateNotification(Times times)
        {
            this.notificationRepositoryMock.Verify(
               m => m.Add(It.IsAny<Notification>()), times, "Notification was not created");
        }
    }
}
