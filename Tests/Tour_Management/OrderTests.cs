using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class OrderTests
    {
        [Fact]
        public void Order_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var order = new Order();

            // Assert
            Assert.NotNull(order);
        }

        [Fact]
        public void Order_ShouldInheritFromPage()
        {
            // Arrange
            var order = new Order();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(order);
        }

        [Fact]
        public void Order_PageLoad_MethodExists()
        {
            // Arrange
            var order = new Order();

            // Act
            var method = order.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Order_BtnClick_MethodExists()
        {
            // Arrange
            var order = new Order();

            // Act
            var method = order.GetType().GetMethod("btn_click",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Order_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(Order);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void Order_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var order = new Order();

            // Act
            var namespaceName = order.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void Order_PageLoad_ShouldNotThrowException()
        {
            // Arrange
            var order = new Order();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => order.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(order, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void Order_BtnClick_ShouldHaveTwoParameters()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var method = type.GetMethod("btn_click",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            Assert.Equal(2, method.GetParameters().Length);
        }

        [Fact]
        public void Order_ClassName_ShouldBeOrder()
        {
            // Arrange
            var order = new Order();

            // Act
            var className = order.GetType().Name;

            // Assert
            Assert.Equal("Order", className);
        }
    }
}
