using System;
using System.Reflection;
using Xunit;

namespace Tour_Management.Tests
{
    public class AssemblyInfoTests
    {
        [Fact]
        public void Assembly_ShouldHaveAssemblyTitleAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var titleAttribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();

            // Assert
            Assert.NotNull(titleAttribute);
            Assert.Equal("Tour_Management", titleAttribute.Title);
        }

        [Fact]
        public void Assembly_ShouldHaveAssemblyProductAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var productAttribute = assembly.GetCustomAttribute<AssemblyProductAttribute>();

            // Assert
            Assert.NotNull(productAttribute);
            Assert.Equal("Tour_Management", productAttribute.Product);
        }

        [Fact]
        public void Assembly_ShouldHaveAssemblyCopyrightAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var copyrightAttribute = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();

            // Assert
            Assert.NotNull(copyrightAttribute);
            Assert.Contains("Copyright", copyrightAttribute.Copyright);
        }

        [Fact]
        public void Assembly_ShouldHaveAssemblyVersionAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var versionAttribute = assembly.GetCustomAttribute<AssemblyVersionAttribute>();

            // Assert
            Assert.NotNull(versionAttribute);
            Assert.Equal("1.0.0.0", versionAttribute.Version);
        }

        [Fact]
        public void Assembly_ShouldHaveAssemblyFileVersionAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var fileVersionAttribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();

            // Assert
            Assert.NotNull(fileVersionAttribute);
            Assert.Equal("1.0.0.0", fileVersionAttribute.Version);
        }

        [Fact]
        public void Assembly_ShouldHaveComVisibleAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var comVisibleAttribute = assembly.GetCustomAttribute<System.Runtime.InteropServices.ComVisibleAttribute>();

            // Assert
            Assert.NotNull(comVisibleAttribute);
            Assert.False(comVisibleAttribute.Value);
        }

        [Fact]
        public void Assembly_ShouldHaveGuidAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var guidAttribute = assembly.GetCustomAttribute<System.Runtime.InteropServices.GuidAttribute>();

            // Assert
            Assert.NotNull(guidAttribute);
            Assert.Equal("f45fb0dc-accf-4207-b390-344d54e2408e", guidAttribute.Value);
        }

        [Fact]
        public void Assembly_ShouldHaveAssemblyDescriptionAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var descriptionAttribute = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();

            // Assert
            Assert.NotNull(descriptionAttribute);
        }

        [Fact]
        public void Assembly_ShouldHaveAssemblyConfigurationAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var configAttribute = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();

            // Assert
            Assert.NotNull(configAttribute);
        }

        [Fact]
        public void Assembly_ShouldHaveAssemblyCompanyAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var companyAttribute = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();

            // Assert
            Assert.NotNull(companyAttribute);
        }

        [Fact]
        public void Assembly_ShouldHaveAssemblyTrademarkAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var trademarkAttribute = assembly.GetCustomAttribute<AssemblyTrademarkAttribute>();

            // Assert
            Assert.NotNull(trademarkAttribute);
        }

        [Fact]
        public void Assembly_ShouldHaveAssemblyCultureAttribute()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var cultureAttribute = assembly.GetCustomAttribute<AssemblyCultureAttribute>();

            // Assert
            Assert.NotNull(cultureAttribute);
        }

        [Fact]
        public void Assembly_Version_ShouldBeValid()
        {
            // Arrange
            var assembly = typeof(AddTour).Assembly;

            // Act
            var version = assembly.GetName().Version;

            // Assert
            Assert.NotNull(version);
            Assert.Equal(1, version.Major);
            Assert.Equal(0, version.Minor);
            Assert.Equal(0, version.Build);
            Assert.Equal(0, version.Revision);
        }
    }
}
