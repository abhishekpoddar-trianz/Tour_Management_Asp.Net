using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class MainProfilePageTests
    {
        [Fact]
        public void MainProfilePage_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var mainProfile = new MainProfilePage();

            // Assert
            Assert.NotNull(mainProfile);
        }

        [Fact]
        public void MainProfilePage_ShouldInheritFromPage()
        {
            // Arrange
            var mainProfile = new MainProfilePage();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(mainProfile);
        }

        [Fact]
        public void MainProfilePage_PageLoad_MethodExists()
        {
            // Arrange
            var mainProfile = new MainProfilePage();

            // Act
            var method = mainProfile.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void MainProfilePage_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(MainProfilePage);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void MainProfilePage_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var mainProfile = new MainProfilePage();

            // Act
            var namespaceName = mainProfile.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void MainProfilePage_PageLoad_ShouldNotThrowException()
        {
            // Arrange
            var mainProfile = new MainProfilePage();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => mainProfile.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(mainProfile, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void MainProfilePage_ClassName_ShouldBeMainProfilePage()
        {
            // Arrange
            var mainProfile = new MainProfilePage();

            // Act
            var className = mainProfile.GetType().Name;

            // Assert
            Assert.Equal("MainProfilePage", className);
        }
    }
}
