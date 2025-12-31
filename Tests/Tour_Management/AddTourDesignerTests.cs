using System;
using Xunit;
using System.Reflection;
using Tour_Management;

namespace Tour_Management.Tests
{
    public class AddTourDesignerTests
    {
        [Fact]
        public void AddTour_ShouldHaveForm1Field()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("form1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.HtmlControls.HtmlForm), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveTourNameField()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("tour_name", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHavePlaceField()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("place", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveDaysField()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("days", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveLocationsField()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("locations", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveFileUpload1Field()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("FileUpload1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.FileUpload), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHavePriceField()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("price", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveTourInfoField()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("tour_info", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveRegisterButtonField()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("Register", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveResetButtonField()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("Reset", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveL1LabelField()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("l1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveLabel1Field()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("Label1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveLabel2Field()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("Label2", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveLabel3Field()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("Label3", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveLabel4Field()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("Label4", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveLabel5Field()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("Label5", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void AddTour_ShouldHaveRegularExpressionValidator1Field()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var field = type.GetField("RegularExpressionValidator1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.RegularExpressionValidator), field.FieldType);
        }

        [Fact]
        public void AddTour_ProtectedFields_ShouldNotBeNull()
        {
            // Arrange
            var type = typeof(AddTour);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotEmpty(fields);
            Assert.True(fields.Length >= 16);
        }
    }
}
