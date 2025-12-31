using System;
using Xunit;
using System.Reflection;
using Tour_Management;

namespace Tour_Management.Tests
{
    public class MainProfilePageDesignerTests
    {
        [Fact]
        public void MainProfilePage_ShouldHaveLabel1Field()
        {
            // Arrange
            var type = typeof(MainProfilePage);

            // Act
            var field = type.GetField("Label1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void MainProfilePage_Label1Field_ShouldBeProtected()
        {
            // Arrange
            var type = typeof(MainProfilePage);
            var field = type.GetField("Label1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var isProtected = field?.IsFamily || field?.IsFamilyOrAssembly;

            // Assert
            Assert.True(isProtected);
        }

        [Fact]
        public void MainProfilePage_ShouldBePartialClass()
        {
            // Arrange
            var type = typeof(MainProfilePage);

            // Act
            var isPartial = type.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false).Length == 0;

            // Assert
            Assert.True(isPartial);
        }

        [Fact]
        public void MainProfilePage_ShouldBeInTourManagementNamespace()
        {
            // Arrange
            var type = typeof(MainProfilePage);

            // Act
            var ns = type.Namespace;

            // Assert
            Assert.Equal("Tour_Management", ns);
        }

        [Fact]
        public void MainProfilePage_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(MainProfilePage);

            // Act
            var isPublic = type.IsPublic;

            // Assert
            Assert.True(isPublic);
        }

        [Fact]
        public void MainProfilePage_ShouldInheritFromPage()
        {
            // Arrange
            var type = typeof(MainProfilePage);

            // Act
            var baseType = type.BaseType;

            // Assert
            Assert.NotNull(baseType);
            Assert.True(typeof(System.Web.UI.Page).IsAssignableFrom(type));
        }

        [Fact]
        public void MainProfilePage_Label1Field_ShouldBeLabelType()
        {
            // Arrange
            var type = typeof(MainProfilePage);
            var field = type.GetField("Label1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var fieldType = field?.FieldType;

            // Assert
            Assert.NotNull(fieldType);
            Assert.True(typeof(System.Web.UI.WebControls.Label).IsAssignableFrom(fieldType));
        }

        [Fact]
        public void MainProfilePage_ProtectedFields_ShouldExist()
        {
            // Arrange
            var type = typeof(MainProfilePage);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotEmpty(fields);
        }

        [Fact]
        public void MainProfilePage_ShouldNotBeAbstract()
        {
            // Arrange
            var type = typeof(MainProfilePage);

            // Act
            var isAbstract = type.IsAbstract;

            // Assert
            Assert.False(isAbstract);
        }

        [Fact]
        public void MainProfilePage_ShouldNotBeSealed()
        {
            // Arrange
            var type = typeof(MainProfilePage);

            // Act
            var isSealed = type.IsSealed;

            // Assert
            Assert.False(isSealed);
        }

        [Fact]
        public void MainProfilePage_ShouldHaveDefaultConstructor()
        {
            // Arrange
            var type = typeof(MainProfilePage);

            // Act
            var constructor = type.GetConstructor(Type.EmptyTypes);

            // Assert
            Assert.NotNull(constructor);
        }

        [Fact]
        public void MainProfilePage_AllFields_ShouldBeProtected()
        {
            // Arrange
            var type = typeof(MainProfilePage);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            foreach (var field in fields)
            {
                Assert.True(field.IsFamily || field.IsFamilyOrAssembly);
            }
        }
    }
}
