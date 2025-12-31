using System;
using Xunit;
using System.Reflection;
using Tour_Management;

namespace Tour_Management.Tests
{
    public class SignUpFormDesignerTests
    {
        [Fact]
        public void SignUpForm_ShouldHaveEmailField()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("email", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHaveFnameField()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("fname", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHaveLnameField()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("lname", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHaveGenderField()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("gender", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.DropDownList), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHavePassword1Field()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("password1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHavePassword2Field()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("password2", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHaveDobField()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("dob", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHaveStreetField()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("street", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHaveCityField()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("city", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHaveStateField()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("state", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHaveRegisterButtonField()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("Register", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHaveResetButtonField()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("Reset", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHaveLabel1Field()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var field = type.GetField("Label1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.Label), field.FieldType);
        }

        [Fact]
        public void SignUpForm_ShouldHaveMultipleLabelFields()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var label2 = type.GetField("Label2", BindingFlags.NonPublic | BindingFlags.Instance);
            var label3 = type.GetField("Label3", BindingFlags.NonPublic | BindingFlags.Instance);
            var label4 = type.GetField("Label4", BindingFlags.NonPublic | BindingFlags.Instance);
            var label5 = type.GetField("Label5", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(label2);
            Assert.NotNull(label3);
            Assert.NotNull(label4);
            Assert.NotNull(label5);
        }

        [Fact]
        public void SignUpForm_ProtectedFields_ShouldNotBeNull()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotEmpty(fields);
            Assert.True(fields.Length >= 20);
        }

        [Fact]
        public void SignUpForm_AllFields_ShouldBeProtected()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            foreach (var field in fields)
            {
                Assert.True(field.IsFamily || field.IsFamilyOrAssembly);
            }
        }

        [Fact]
        public void SignUpForm_TextBoxFields_ShouldBeTextBoxType()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var email = type.GetField("email", BindingFlags.NonPublic | BindingFlags.Instance);
            var fname = type.GetField("fname", BindingFlags.NonPublic | BindingFlags.Instance);
            var lname = type.GetField("lname", BindingFlags.NonPublic | BindingFlags.Instance);
            var password1 = type.GetField("password1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(email);
            Assert.NotNull(fname);
            Assert.NotNull(lname);
            Assert.NotNull(password1);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), email.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), fname.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), lname.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.TextBox), password1.FieldType);
        }

        [Fact]
        public void SignUpForm_GenderField_ShouldBeDropDownListType()
        {
            // Arrange
            var type = typeof(SignUpForm);
            var field = type.GetField("gender", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var fieldType = field?.FieldType;

            // Assert
            Assert.NotNull(fieldType);
            Assert.True(typeof(System.Web.UI.WebControls.DropDownList).IsAssignableFrom(fieldType));
        }

        [Fact]
        public void SignUpForm_ButtonFields_ShouldBeButtonType()
        {
            // Arrange
            var type = typeof(SignUpForm);

            // Act
            var registerButton = type.GetField("Register", BindingFlags.NonPublic | BindingFlags.Instance);
            var resetButton = type.GetField("Reset", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(registerButton);
            Assert.NotNull(resetButton);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), registerButton.FieldType);
            Assert.Equal(typeof(System.Web.UI.WebControls.Button), resetButton.FieldType);
        }
    }
}
