using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class usercrudTests
    {
        [Fact]
        public void usercrud_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var userCrud = new usercrud();

            // Assert
            Assert.NotNull(userCrud);
        }

        [Fact]
        public void usercrud_ShouldInheritFromPage()
        {
            // Arrange
            var userCrud = new usercrud();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(userCrud);
        }

        [Fact]
        public void usercrud_PageLoad_MethodExists()
        {
            // Arrange
            var userCrud = new usercrud();

            // Act
            var method = userCrud.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void usercrud_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(usercrud);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void usercrud_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var userCrud = new usercrud();

            // Act
            var namespaceName = userCrud.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void usercrud_PageLoad_ShouldNotThrowException()
        {
            // Arrange
            var userCrud = new usercrud();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => userCrud.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(userCrud, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void usercrud_ClassName_ShouldBeusercrud()
        {
            // Arrange
            var userCrud = new usercrud();

            // Act
            var className = userCrud.GetType().Name;

            // Assert
            Assert.Equal("usercrud", className);
        }
    }
}
