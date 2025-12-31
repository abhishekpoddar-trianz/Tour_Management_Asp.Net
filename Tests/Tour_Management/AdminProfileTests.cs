using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class AdminProfileTests
    {
        [Fact]
        public void AdminProfile_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var adminProfile = new AdminProfile();

            // Assert
            Assert.NotNull(adminProfile);
        }

        [Fact]
        public void AdminProfile_ShouldInheritFromPage()
        {
            // Arrange
            var adminProfile = new AdminProfile();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(adminProfile);
        }

        [Fact]
        public void AdminProfile_PageLoad_MethodExists()
        {
            // Arrange
            var adminProfile = new AdminProfile();

            // Act
            var method = adminProfile.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void AdminProfile_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(AdminProfile);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void AdminProfile_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var adminProfile = new AdminProfile();

            // Act
            var namespaceName = adminProfile.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void AdminProfile_PageLoad_ShouldNotThrowException()
        {
            // Arrange
            var adminProfile = new AdminProfile();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => adminProfile.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(adminProfile, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void AdminProfile_ClassName_ShouldBeAdminProfile()
        {
            // Arrange
            var adminProfile = new AdminProfile();

            // Act
            var className = adminProfile.GetType().Name;

            // Assert
            Assert.Equal("AdminProfile", className);
        }
    }
}
