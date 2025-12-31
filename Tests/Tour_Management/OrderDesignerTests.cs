using System;
using Xunit;
using System.Reflection;
using Tour_Management;

namespace Tour_Management.Tests
{
    public class OrderDesignerTests
    {
        [Fact]
        public void Order_ShouldHaveLabel1Field()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var field = type.GetField("Label1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void Order_ShouldHaveNameField()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var field = type.GetField("name", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void Order_ShouldHaveLabel3Field()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var field = type.GetField("Label3", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void Order_ShouldHaveCityField()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var field = type.GetField("city", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void Order_ShouldHaveLabel5Field()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var field = type.GetField("Label5", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void Order_ShouldHaveTourNameField()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var field = type.GetField("tour_name", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void Order_ShouldHaveLabel10Field()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var field = type.GetField("Label10", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void Order_ShouldHaveNumberField()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var field = type.GetField("number", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void Order_ShouldHaveBookButtonField()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var field = type.GetField("Book", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), field.FieldType);
        }

        [Fact]
        public void Order_ShouldHaveResetButtonField()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var field = type.GetField("Reset", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), field.FieldType);
        }

        [Fact]
        public void Order_ProtectedFields_ShouldNotBeNull()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotEmpty(fields);
            Assert.True(fields.Length >= 10);
        }

        [Fact]
        public void Order_AllFields_ShouldBeProtected()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            foreach (var field in fields)
            {
                Assert.True(field.IsFamily || field.IsFamilyOrAssembly);
            }
        }

        [Fact]
        public void Order_TextBoxFields_ShouldBeTextBoxType()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var nameField = type.GetField("name", BindingFlags.NonPublic | BindingFlags.Instance);
            var cityField = type.GetField("city", BindingFlags.NonPublic | BindingFlags.Instance);
            var tourNameField = type.GetField("tour_name", BindingFlags.NonPublic | BindingFlags.Instance);
            var numberField = type.GetField("number", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(nameField);
            Assert.NotNull(cityField);
            Assert.NotNull(tourNameField);
            Assert.NotNull(numberField);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), nameField.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), cityField.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), tourNameField.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), numberField.FieldType);
        }

        [Fact]
        public void Order_LabelFields_ShouldBeLabelType()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var label1 = type.GetField("Label1", BindingFlags.NonPublic | BindingFlags.Instance);
            var label3 = type.GetField("Label3", BindingFlags.NonPublic | BindingFlags.Instance);
            var label5 = type.GetField("Label5", BindingFlags.NonPublic | BindingFlags.Instance);
            var label10 = type.GetField("Label10", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(label1);
            Assert.NotNull(label3);
            Assert.NotNull(label5);
            Assert.NotNull(label10);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), label1.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), label3.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), label5.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), label10.FieldType);
        }

        [Fact]
        public void Order_ButtonFields_ShouldBeButtonType()
        {
            // Arrange
            var type = typeof(Order);

            // Act
            var bookButton = type.GetField("Book", BindingFlags.NonPublic | BindingFlags.Instance);
            var resetButton = type.GetField("Reset", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(bookButton);
            Assert.NotNull(resetButton);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), bookButton.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), resetButton.FieldType);
        }
    }
}
