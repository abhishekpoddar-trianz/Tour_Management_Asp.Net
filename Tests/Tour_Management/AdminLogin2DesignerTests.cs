using System;
using Xunit;
using System.Reflection;
using Tour_Management;

namespace Tour_Management.Tests
{
    public class AdminLogin2DesignerTests
    {
        [Fact]
        public void AdminLogin2_ShouldHaveForm1Field()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var field = type.GetField("form1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.HtmlControls.HtmlForm), field.FieldType);
        }

        [Fact]
        public void AdminLogin2_ShouldHaveNameLabelField()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var field = type.GetField("name", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void AdminLogin2_ShouldHaveTextBox1Field()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var field = type.GetField("TextBox1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void AdminLogin2_ShouldHavePasswordLabelField()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var field = type.GetField("password", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void AdminLogin2_ShouldHaveTextBox2Field()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var field = type.GetField("TextBox2", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void AdminLogin2_ShouldHaveButton1Field()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var field = type.GetField("Button1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), field.FieldType);
        }

        [Fact]
        public void AdminLogin2_ProtectedFields_ShouldNotBeNull()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotEmpty(fields);
            Assert.True(fields.Length >= 6);
        }

        [Fact]
        public void AdminLogin2_AllFields_ShouldBeProtected()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            foreach (var field in fields)
            {
                Assert.True(field.IsFamily || field.IsFamilyOrAssembly);
            }
        }

        [Fact]
        public void AdminLogin2_Form1Field_ShouldBeHtmlForm()
        {
            // Arrange
            var type = typeof(AdminLogin2);
            var field = type.GetField("form1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var fieldType = field?.FieldType;

            // Assert
            Assert.NotNull(fieldType);
            Assert.True(typeof(System.Web.UI.HtmlControls.HtmlForm).IsAssignableFrom(fieldType));
        }

        [Fact]
        public void AdminLogin2_TextBoxFields_ShouldBeTextBoxType()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var textBox1 = type.GetField("TextBox1", BindingFlags.NonPublic | BindingFlags.Instance);
            var textBox2 = type.GetField("TextBox2", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(textBox1);
            Assert.NotNull(textBox2);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), textBox1.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), textBox2.FieldType);
        }

        [Fact]
        public void AdminLogin2_LabelFields_ShouldBeLabelType()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var nameLabel = type.GetField("name", BindingFlags.NonPublic | BindingFlags.Instance);
            var passwordLabel = type.GetField("password", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(nameLabel);
            Assert.NotNull(passwordLabel);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), nameLabel.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), passwordLabel.FieldType);
        }

        [Fact]
        public void AdminLogin2_Button1Field_ShouldBeButtonType()
        {
            // Arrange
            var type = typeof(AdminLogin2);

            // Act
            var button = type.GetField("Button1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(button);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), button.FieldType);
        }
    }
}
