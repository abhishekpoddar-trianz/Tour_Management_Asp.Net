using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class DisplayToursTests
    {
        [Fact]
        public void DisplayTours_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var displayTours = new DisplayTours();

            // Assert
            Assert.NotNull(displayTours);
        }

        [Fact]
        public void DisplayTours_ShouldInheritFromPage()
        {
            // Arrange
            var displayTours = new DisplayTours();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(displayTours);
        }

        [Fact]
        public void DisplayTours_PageLoad_MethodExists()
        {
            // Arrange
            var displayTours = new DisplayTours();

            // Act
            var method = displayTours.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void DisplayTours_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(DisplayTours);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void DisplayTours_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var displayTours = new DisplayTours();

            // Act
            var namespaceName = displayTours.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void DisplayTours_PageLoad_ShouldNotThrowException()
        {
            // Arrange
            var displayTours = new DisplayTours();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => displayTours.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(displayTours, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void DisplayTours_ClassName_ShouldBeDisplayTours()
        {
            // Arrange
            var displayTours = new DisplayTours();

            // Act
            var className = displayTours.GetType().Name;

            // Assert
            Assert.Equal("DisplayTours", className);
        }
    }
}
