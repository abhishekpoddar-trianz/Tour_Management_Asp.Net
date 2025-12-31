using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class AdminLogin2Tests
    {
        [Fact]
        public void AdminLogin2_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var adminLogin = new AdminLogin2();

            // Assert
            Assert.NotNull(adminLogin);
        }

        [Fact]
        public void AdminLogin2_ShouldInheritFromPage()
        {
            // Arrange
            var adminLogin = new AdminLogin2();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(adminLogin);
        }

        [Fact]
        public void AdminLogin2_PageLoad_MethodExists()
        {
            // Arrange
            var adminLogin = new AdminLogin2();

            // Act
            var method = adminLogin.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void AdminLogin2_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void AdminLogin2_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var adminLogin = new AdminLogin2();

            // Act
            var namespaceName = adminLogin.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void AdminLogin2_PageLoad_ShouldHaveTwoParameters()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var method = type.GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            Assert.Equal(2, method.GetParameters().Length);
        }

        [Fact]
        public void AdminLogin2_ClassName_ShouldBeAdminLogin2()
        {
            // Arrange
            var adminLogin = new AdminLogin2();

            // Act
            var className = adminLogin.GetType().Name;

            // Assert
            Assert.Equal("AdminLogin2", className);
        }
    }
}
