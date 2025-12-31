using System;
using Xunit;
using Tour_Management;
using System.Web.UI;

namespace Tour_Management.Tests
{
    public class TourCrudTests
    {
        [Fact]
        public void TourCrud_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var tourCrud = new TourCrud();

            // Assert
            Assert.NotNull(tourCrud);
        }

        [Fact]
        public void TourCrud_ShouldInheritFromPage()
        {
            // Arrange
            var tourCrud = new TourCrud();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(tourCrud);
        }

        [Fact]
        public void TourCrud_PageLoad_MethodExists()
        {
            // Arrange
            var tourCrud = new TourCrud();

            // Act
            var method = tourCrud.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void TourCrud_RefreshData_MethodExists()
        {
            // Arrange
            var tourCrud = new TourCrud();

            // Act
            var method = tourCrud.GetType().GetMethod("refreshdata",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void TourCrud_Type_ShouldBePublicPartialClass()
        {
            // Arrange
            var type = typeof(TourCrud);

            // Act & Assert
            Assert.True(type.IsPublic);
            Assert.True(type.IsClass);
        }

        [Fact]
        public void TourCrud_Namespace_ShouldBeTourManagement()
        {
            // Arrange
            var tourCrud = new TourCrud();

            // Act
            var namespaceName = tourCrud.GetType().Namespace;

            // Assert
            Assert.Equal("Tour_Management", namespaceName);
        }

        [Fact]
        public void TourCrud_RefreshData_ShouldBePublic()
        {
            // Arrange
            var tourCrud = new TourCrud();

            // Act
            var method = tourCrud.GetType().GetMethod("refreshdata",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            Assert.True(method.IsPublic);
        }

        [Fact]
        public void TourCrud_ClassName_ShouldBeTourCrud()
        {
            // Arrange
            var tourCrud = new TourCrud();

            // Act
            var className = tourCrud.GetType().Name;

            // Assert
            Assert.Equal("TourCrud", className);
        }
    }
}
