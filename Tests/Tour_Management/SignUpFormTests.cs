using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class SignUpFormTests
    {
        [Fact]
        public void SignUpForm_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var signUpForm = new SignUpForm();

            // Assert
            Assert.NotNull(signUpForm);
        }

        [Fact]
        public void SignUpForm_ShouldInheritFromPage()
        {
            // Arrange
            var signUpForm = new SignUpForm();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(signUpForm);
        }

        [Fact]
        public void SignUpForm_PageLoad_MethodExists()
        {
            // Arrange
            var signUpForm = new SignUpForm();

            // Act
            var method = signUpForm.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void SignUpForm_RegisterClick_MethodExists()
        {
            // Arrange
            var signUpForm = new SignUpForm();

            // Act
            var method = signUpForm.GetType().GetMethod("Register_Click",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void SignUpForm_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void SignUpForm_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var signUpForm = new SignUpForm();

            // Act
            var namespaceName = signUpForm.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void SignUpForm_PageLoad_ShouldNotThrowException()
        {
            // Arrange
            var signUpForm = new SignUpForm();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => signUpForm.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(signUpForm, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void SignUpForm_RegisterClick_HasTwoParameters()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var method = type.GetMethod("Register_Click",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
        }

        [Fact]
        public void SignUpForm_ClassName_ShouldBeSignUpForm()
        {
            // Arrange
            var signUpForm = new SignUpForm();

            // Act
            var className = signUpForm.GetType().Name;

            // Assert
            Assert.Equal("SignUpForm", className);
        }
    }
}
