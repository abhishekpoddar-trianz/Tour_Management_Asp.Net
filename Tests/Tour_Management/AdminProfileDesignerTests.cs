using System;
using Xunit;
using System.Reflection;
using Tour_Management;

namespace Tour_Management.Tests
{
    public class AdminProfileDesignerTests
    {
        [Fact]
        public void AdminProfile_ShouldBePartialClass()
        {
            // Arrange
            var type = typeof(AdminProfile);

            // Act
            var isPartial = type.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false).Length == 0;

            // Assert
            Assert.True(isPartial);
        }

        [Fact]
        public void AdminProfile_ShouldExist()
        {
            // Arrange & Act
            var type = typeof(AdminProfile);

            // Assert
            Assert.NotNull(type);
        }

        [Fact]
        public void AdminProfile_ShouldBeInTourManagementNamespace()
        {
            // Arrange
            var type = typeof(AdminProfile);

            // Act
            var ns = type.Namespace;

            // Assert
            Assert.Equal("Tour_Management", ns);
        }

        [Fact]
        public void AdminProfile_ShouldNotHaveExplicitFields()
        {
            // Arrange
            var type = typeof(AdminProfile);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            // Assert
            Assert.Empty(fields);
        }

        [Fact]
        public void AdminProfile_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(AdminProfile);

            // Act
            var isPublic = type.IsPublic;

            // Assert
            Assert.True(isPublic);
        }

        [Fact]
        public void AdminProfile_ShouldNotBeSealed()
        {
            // Arrange
            var type = typeof(AdminProfile);

            // Act
            var isSealed = type.IsSealed;

            // Assert
            Assert.False(isSealed);
        }

        [Fact]
        public void AdminProfile_ShouldNotBeAbstract()
        {
            // Arrange
            var type = typeof(AdminProfile);

            // Act
            var isAbstract = type.IsAbstract;

            // Assert
            Assert.False(isAbstract);
        }

        [Fact]
        public void AdminProfile_ShouldInheritFromPage()
        {
            // Arrange
            var type = typeof(AdminProfile);

            // Act
            var baseType = type.BaseType;

            // Assert
            Assert.NotNull(baseType);
            Assert.True(typeof(System.Web.UI.Page).IsAssignableFrom(type));
        }

        [Fact]
        public void AdminProfile_ShouldHaveDefaultConstructor()
        {
            // Arrange
            var type = typeof(AdminProfile);

            // Act
            var constructor = type.GetConstructor(Type.EmptyTypes);

            // Assert
            Assert.NotNull(constructor);
        }

        [Fact]
        public void AdminProfile_DesignerFile_ShouldNotDefineExplicitMembers()
        {
            // Arrange
            var type = typeof(AdminProfile);

            // Act
            var members = type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var explicitMembers = members.Where(m => m.DeclaringType == type && !m.Name.Contains("Page_Load")).ToArray();

            // Assert
            Assert.NotEmpty(members);
        }
    }
}
