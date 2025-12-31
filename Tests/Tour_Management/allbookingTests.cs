using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class allbookingTests
    {
        [Fact]
        public void allbooking_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var allBooking = new allbooking();

            // Assert
            Assert.NotNull(allBooking);
        }

        [Fact]
        public void allbooking_ShouldInheritFromPage()
        {
            // Arrange
            var allBooking = new allbooking();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(allBooking);
        }

        [Fact]
        public void allbooking_PageLoad_MethodExists()
        {
            // Arrange
            var allBooking = new allbooking();

            // Act
            var method = allBooking.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void allbooking_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(allbooking);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void allbooking_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var allBooking = new allbooking();

            // Act
            var namespaceName = allBooking.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void allbooking_PageLoad_ShouldNotThrowException()
        {
            // Arrange
            var allBooking = new allbooking();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => allBooking.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(allBooking, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void allbooking_ClassName_ShouldBeallbooking()
        {
            // Arrange
            var allBooking = new allbooking();

            // Act
            var className = allBooking.GetType().Name;

            // Assert
            Assert.Equal("allbooking", className);
        }
    }
}
