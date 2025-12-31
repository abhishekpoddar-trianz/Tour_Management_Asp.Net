using System;
using Xunit;
using System.Reflection;
using Tour_Management;

namespace Tour_Management.Tests
{
    public class AllbookingTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var allbooking = new allbooking();

            // Assert
            Assert.NotNull(allbooking);
        }

        [Fact]
        public void Allbooking_ShouldInheritFromPage()
        {
            // Arrange & Act
            var allbooking = new allbooking();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(allbooking);
        }

        [Fact]
        public void Page_Load_ShouldNotThrowException_WhenCalledWithValidArguments()
        {
            // Arrange
            var allbooking = new allbooking();
            var sender = new object();
            var e = new EventArgs();

            // Act & Assert
            var exception = Record.Exception(() => allbooking.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(allbooking, new object[] { sender, e }));

            Assert.Null(exception);
        }

        [Fact]
        public void Page_Load_ShouldHandleNullSender()
        {
            // Arrange
            var allbooking = new allbooking();
            object sender = null;
            var e = new EventArgs();

            // Act & Assert
            var exception = Record.Exception(() => allbooking.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(allbooking, new object[] { sender, e }));

            Assert.Null(exception);
        }

        [Fact]
        public void Page_Load_ShouldHandleNullEventArgs()
        {
            // Arrange
            var allbooking = new allbooking();
            var sender = new object();
            EventArgs e = null;

            // Act & Assert
            var exception = Record.Exception(() => allbooking.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(allbooking, new object[] { sender, e }));

            Assert.Null(exception);
        }

        [Fact]
        public void Page_Load_MethodSignature_ShouldMatchEventHandlerPattern()
        {
            // Arrange
            var allbooking = new allbooking();
            var method = allbooking.GetType().GetMethod("Page_Load",
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
        public void Allbooking_TypeName_ShouldBeAllbooking()
        {
            // Arrange & Act
            var allbooking = new allbooking();

            // Assert
            Assert.Equal("allbooking", allbooking.GetType().Name);
        }

        [Fact]
        public void Allbooking_Namespace_ShouldBeTourManagement()
        {
            // Arrange & Act
            var allbooking = new allbooking();

            // Assert
            Assert.Equal("Tour_Management", allbooking.GetType().Namespace);
        }

        [Fact]
        public void Allbooking_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(allbooking);

            // Act
            var isPublic = type.IsPublic;

            // Assert
            Assert.True(isPublic);
        }

        [Fact]
        public void Allbooking_ShouldNotBeAbstract()
        {
            // Arrange
            var type = typeof(allbooking);

            // Act
            var isAbstract = type.IsAbstract;

            // Assert
            Assert.False(isAbstract);
        }

        [Fact]
        public void Allbooking_ShouldHavePageLoadMethod()
        {
            // Arrange
            var type = typeof(allbooking);

            // Act
            var method = type.GetMethod("Page_Load", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Page_Load_ReturnType_ShouldBeVoid()
        {
            // Arrange
            var type = typeof(allbooking);
            var method = type.GetMethod("Page_Load", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var returnType = method?.ReturnType;

            // Assert
            Assert.Equal(typeof(void), returnType);
        }

        [Fact]
        public void Allbooking_ShouldHaveDefaultConstructor()
        {
            // Arrange
            var type = typeof(allbooking);

            // Act
            var constructor = type.GetConstructor(Type.EmptyTypes);

            // Assert
            Assert.NotNull(constructor);
        }

        [Fact]
        public void Allbooking_ShouldBePartialClass()
        {
            // Arrange
            var type = typeof(allbooking);

            // Act
            var isPartial = type.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false).Length == 0;

            // Assert
            Assert.True(isPartial);
        }
    }
}
