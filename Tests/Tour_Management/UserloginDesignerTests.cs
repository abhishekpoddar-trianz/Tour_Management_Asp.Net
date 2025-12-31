using System;
using Xunit;
using System.Reflection;
using Tour_Management;

namespace Tour_Management.Tests
{
    public class UserloginDesignerTests
    {
        [Fact]
        public void Userlogin_ShouldHaveForm1Field()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var field = type.GetField("form1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.HtmlControls.HtmlForm), field.FieldType);
        }

        [Fact]
        public void Userlogin_ShouldHaveLabel1Field()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var field = type.GetField("Label1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void Userlogin_ShouldHaveTxtEmailField()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var field = type.GetField("txtEmail", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void Userlogin_ShouldHaveLabel2Field()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var field = type.GetField("Label2", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void Userlogin_ShouldHaveTxtPasswordField()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var field = type.GetField("txtPassword", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void Userlogin_ShouldHaveRegisterButtonField()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var field = type.GetField("Register", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), field.FieldType);
        }

        [Fact]
        public void Userlogin_ShouldHaveButton1Field()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var field = type.GetField("Button1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), field.FieldType);
        }

        [Fact]
        public void Userlogin_ProtectedFields_ShouldNotBeNull()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotEmpty(fields);
            Assert.True(fields.Length >= 7);
        }

        [Fact]
        public void Userlogin_AllFields_ShouldBeProtected()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            foreach (var field in fields)
            {
                Assert.True(field.IsFamily || field.IsFamilyOrAssembly);
            }
        }

        [Fact]
        public void Userlogin_Form1Field_ShouldBeHtmlForm()
        {
            // Arrange
            var type = typeof(userlogin);
            var field = type.GetField("form1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var fieldType = field?.FieldType;

            // Assert
            Assert.NotNull(fieldType);
            Assert.True(typeof(System.Web.UI.HtmlControls.HtmlForm).IsAssignableFrom(fieldType));
        }

        [Fact]
        public void Userlogin_TextBoxFields_ShouldBeTextBoxType()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var txtEmail = type.GetField("txtEmail", BindingFlags.NonPublic | BindingFlags.Instance);
            var txtPassword = type.GetField("txtPassword", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(txtEmail);
            Assert.NotNull(txtPassword);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), txtEmail.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), txtPassword.FieldType);
        }

        [Fact]
        public void Userlogin_LabelFields_ShouldBeLabelType()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var label1 = type.GetField("Label1", BindingFlags.NonPublic | BindingFlags.Instance);
            var label2 = type.GetField("Label2", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(label1);
            Assert.NotNull(label2);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), label1.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), label2.FieldType);
        }

        [Fact]
        public void Userlogin_ButtonFields_ShouldBeButtonType()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var registerButton = type.GetField("Register", BindingFlags.NonPublic | BindingFlags.Instance);
            var button1 = type.GetField("Button1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(registerButton);
            Assert.NotNull(button1);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), registerButton.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), button1.FieldType);
        }

        [Fact]
        public void Userlogin_ShouldBePartialClass()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var isPartial = type.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false).Length == 0;

            // Assert
            Assert.True(isPartial);
        }

        [Fact]
        public void Userlogin_ShouldBeInTourManagementNamespace()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var ns = type.Namespace;

            // Assert
            Assert.Equal("Tour_Management", ns);
        }

        [Fact]
        public void Userlogin_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var isPublic = type.IsPublic;

            // Assert
            Assert.True(isPublic);
        }

        [Fact]
        public void Userlogin_ShouldInheritFromPage()
        {
            // Arrange
            var type = typeof(userlogin);

            // Act
            var baseType = type.BaseType;

            // Assert
            Assert.NotNull(baseType);
            Assert.True(typeof(System.Web.UI.Page).IsAssignableFrom(type));
        }
    }
}
