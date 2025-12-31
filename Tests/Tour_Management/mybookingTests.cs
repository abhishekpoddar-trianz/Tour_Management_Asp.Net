using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class mybookingTests
    {
        [Fact]
        public void mybooking_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var myBooking = new mybooking();

            // Assert
            Assert.NotNull(myBooking);
        }

        [Fact]
        public void mybooking_ShouldInheritFromPage()
        {
            // Arrange
            var myBooking = new mybooking();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(myBooking);
        }

        [Fact]
        public void mybooking_PageLoad_MethodExists()
        {
            // Arrange
            var myBooking = new mybooking();

            // Act
            var method = myBooking.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void mybooking_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(mybooking);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void mybooking_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var myBooking = new mybooking();

            // Act
            var namespaceName = myBooking.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void mybooking_PageLoad_ShouldNotThrowException()
        {
            // Arrange
            var myBooking = new mybooking();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => myBooking.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(myBooking, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void mybooking_ClassName_ShouldBemybooking()
        {
            // Arrange
            var myBooking = new mybooking();

            // Act
            var className = myBooking.GetType().Name;

            // Assert
            Assert.Equal("mybooking", className);
        }
    }
}
