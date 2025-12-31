using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class userloginTests
    {
        [Fact]
        public void userlogin_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var userLogin = new userlogin();

            // Assert
            Assert.NotNull(userLogin);
        }

        [Fact]
        public void userlogin_ShouldInheritFromPage()
        {
            // Arrange
            var userLogin = new userlogin();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(userLogin);
        }

        [Fact]
        public void userlogin_PageLoad_MethodExists()
        {
            // Arrange
            var userLogin = new userlogin();

            // Act
            var method = userLogin.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void userlogin_BtnSubmit_MethodExists()
        {
            // Arrange
            var userLogin = new userlogin();

            // Act
            var method = userLogin.GetType().GetMethod("Btn_Submit",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void userlogin_BtnReg_MethodExists()
        {
            // Arrange
            var userLogin = new userlogin();

            // Act
            var method = userLogin.GetType().GetMethod("Btn_reg",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void userlogin_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void userlogin_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var userLogin = new userlogin();

            // Act
            var namespaceName = userLogin.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void userlogin_BtnSubmit_HasTwoParameters()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var method = type.GetMethod("Btn_Submit",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
        }

        [Fact]
        public void userlogin_BtnReg_HasTwoParameters()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var method = type.GetMethod("Btn_reg",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
        }

        [Fact]
        public void userlogin_ClassName_ShouldBeuserlogin()
        {
            // Arrange
            var userLogin = new userlogin();

            // Act
            var className = userLogin.GetType().Name;

            // Assert
            Assert.Equal("userlogin", className);
        }
    }
}
