using System;
using Xunit;
using System.Reflection;
using Tour_Management;

namespace Tour_Management.Tests
{
    public class MybookingTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var mybooking = new mybooking();

            // Assert
            Assert.NotNull(mybooking);
        }

        [Fact]
        public void Mybooking_ShouldInheritFromPage()
        {
            // Arrange & Act
            var mybooking = new mybooking();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(mybooking);
        }

        [Fact]
        public void Page_Load_ShouldNotThrowException_WhenCalledWithValidArguments()
        {
            // Arrange
            var mybooking = new mybooking();
            var sender = new object();
            var e = new EventArgs();

            // Act & Assert
            var exception = Record.Exception(() => mybooking.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(mybooking, new object[] { sender, e }));

            Assert.Null(exception);
        }

        [Fact]
        public void Page_Load_ShouldHandleNullSender()
        {
            // Arrange
            var mybooking = new mybooking();
            object sender = null;
            var e = new EventArgs();

            // Act & Assert
            var exception = Record.Exception(() => mybooking.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(mybooking, new object[] { sender, e }));

            Assert.Null(exception);
        }

        [Fact]
        public void Page_Load_ShouldHandleNullEventArgs()
        {
            // Arrange
            var mybooking = new mybooking();
            var sender = new object();
            EventArgs e = null;

            // Act & Assert
            var exception = Record.Exception(() => mybooking.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(mybooking, new object[] { sender, e }));

            Assert.Null(exception);
        }

        [Fact]
        public void Page_Load_MethodSignature_ShouldMatchEventHandlerPattern()
        {
            // Arrange
            var mybooking = new mybooking();
            var method = mybooking.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var parameters = method?.GetParameters();

            // Assert
            Assert.NotNull(parameters);
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Mybooking_TypeName_ShouldBeMybooking()
        {
            // Arrange & Act
            var mybooking = new mybooking();

            // Assert
            Assert.Equal("mybooking", mybooking.GetType().Name);
        }

        [Fact]
        public void Mybooking_Namespace_ShouldBeTourManagement()
        {
            // Arrange & Act
            var mybooking = new mybooking();

            // Assert
            Assert.Equal("Tour_Management", mybooking.GetType().Namespace);
        }

        [Fact]
        public void Mybooking_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(mybooking);

            // Act
            var isPublic = type.IsPublic;

            // Assert
            Assert.True(isPublic);
        }

        [Fact]
        public void Mybooking_ShouldNotBeAbstract()
        {
            // Arrange
            var type = typeof(mybooking);

            // Act
            var isAbstract = type.IsAbstract;

            // Assert
            Assert.False(isAbstract);
        }

        [Fact]
        public void Mybooking_ShouldHavePageLoadMethod()
        {
            // Arrange
            var type = typeof(mybooking);

            // Act
            var method = type.GetMethod("Page_Load", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Page_Load_ReturnType_ShouldBeVoid()
        {
            // Arrange
            var type = typeof(mybooking);
            var method = type.GetMethod("Page_Load", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var returnType = method?.ReturnType;

            // Assert
            Assert.Equal(typeof(void), returnType);
        }

        [Fact]
        public void Mybooking_ShouldHaveDefaultConstructor()
        {
            // Arrange
            var type = typeof(mybooking);

            // Act
            var constructor = type.GetConstructor(Type.EmptyTypes);

            // Assert
            Assert.NotNull(constructor);
        }

        [Fact]
        public void Mybooking_ShouldBePartialClass()
        {
            // Arrange
            var type = typeof(mybooking);

            // Act
            var isPartial = type.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false).Length == 0;

            // Assert
            Assert.True(isPartial);
        }
    }
}
