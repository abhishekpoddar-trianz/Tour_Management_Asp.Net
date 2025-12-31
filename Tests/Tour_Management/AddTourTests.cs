using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class AddTourTests
    {
        [Fact]
        public void AddTour_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var addTour = new AddTour();

            // Assert
            Assert.NotNull(addTour);
        }

        [Fact]
        public void AddTour_ShouldInheritFromPage()
        {
            // Arrange
            var addTour = new AddTour();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(addTour);
        }

        [Fact]
        public void AddTour_PageLoad_ShouldNotThrowException()
        {
            // Arrange
            var addTour = new AddTour();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => addTour.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(addTour, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void AddTour_RegisterClick_MethodExists()
        {
            // Arrange
            var addTour = new AddTour();

            // Act
            var method = addTour.GetType().GetMethod("Register_Click",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void AddTour_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void AddTour_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var addTour = new AddTour();

            // Act
            var namespaceName = addTour.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void AddTour_ClassName_ShouldBeAddTour()
        {
            // Arrange
            var addTour = new AddTour();

            // Act
            var className = addTour.GetType().Name;

            // Assert
            Assert.Equal("AddTour", className);
        }

        [Fact]
        public void AddTour_RegisterClick_HasTwoParameters()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var method = type.GetMethod("Register_Click",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
        }
    }
}
