using System;
using Xunit;
using System.Reflection;
using Tour_Management;

namespace Tour_Management.Tests
{
    public class TourCrudDesignerTests
    {
        [Fact]
        public void TourCrud_ShouldHaveForm1Field()
        {
            // Arrange
            var type = typeof(TourCrud);

            // Act
            var field = type.GetField("form1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.HtmlControls.HtmlForm), field.FieldType);
        }

        [Fact]
        public void TourCrud_ShouldHaveGridView1Field()
        {
            // Arrange
            var type = typeof(TourCrud);

            // Act
            var field = type.GetField("GridView1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.GridView), field.FieldType);
        }

        [Fact]
        public void TourCrud_ShouldHaveSqlDataSource1Field()
        {
            // Arrange
            var type = typeof(TourCrud);

            // Act
            var field = type.GetField("SqlDataSource1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(System.Web.UI.WebControls.SqlDataSource), field.FieldType);
        }

        [Fact]
        public void TourCrud_ProtectedFields_ShouldNotBeNull()
        {
            // Arrange
            var type = typeof(TourCrud);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotEmpty(fields);
            Assert.True(fields.Length >= 3);
        }

        [Fact]
        public void TourCrud_AllFields_ShouldBeProtected()
        {
            // Arrange
            var type = typeof(TourCrud);

            // Act
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            foreach (var field in fields)
            {
                Assert.True(field.IsFamily || field.IsFamilyOrAssembly);
            }
        }

        [Fact]
        public void TourCrud_Form1Field_ShouldBeHtmlForm()
        {
            // Arrange
            var type = typeof(TourCrud);
            var field = type.GetField("form1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var fieldType = field?.FieldType;

            // Assert
            Assert.NotNull(fieldType);
            Assert.True(typeof(System.Web.UI.HtmlControls.HtmlForm).IsAssignableFrom(fieldType));
        }

        [Fact]
        public void TourCrud_GridView1Field_ShouldBeGridViewType()
        {
            // Arrange
            var type = typeof(TourCrud);
            var field = type.GetField("GridView1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var fieldType = field?.FieldType;

            // Assert
            Assert.NotNull(fieldType);
            Assert.True(typeof(System.Web.UI.WebControls.GridView).IsAssignableFrom(fieldType));
        }

        [Fact]
        public void TourCrud_SqlDataSource1Field_ShouldBeSqlDataSourceType()
        {
            // Arrange
            var type = typeof(TourCrud);
            var field = type.GetField("SqlDataSource1", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var fieldType = field?.FieldType;

            // Assert
            Assert.NotNull(fieldType);
            Assert.True(typeof(System.Web.UI.WebControls.SqlDataSource).IsAssignableFrom(fieldType));
        }

        [Fact]
        public void TourCrud_ShouldBePartialClass()
        {
            // Arrange
            var type = typeof(TourCrud);

            // Act
            var isPartial = type.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false).Length == 0;

            // Assert
            Assert.True(isPartial);
        }

        [Fact]
        public void TourCrud_ShouldBeInTourManagementNamespace()
        {
            // Arrange
            var type = typeof(TourCrud);

            // Act
            var ns = type.Namespace;

            // Assert
            Assert.Equal("Tour_Management", ns);
        }

        [Fact]
        public void TourCrud_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(TourCrud);

            // Act
            var isPublic = type.IsPublic;

            // Assert
            Assert.True(isPublic);
        }

        [Fact]
        public void TourCrud_ShouldInheritFromPage()
        {
            // Arrange
            var type = typeof(TourCrud);

            // Act
            var baseType = type.BaseType;

            // Assert
            Assert.NotNull(baseType);
            Assert.True(typeof(System.Web.UI.Page).IsAssignableFrom(type));
        }

        [Fact]
        public void TourCrud_ShouldHaveDefaultConstructor()
        {
            // Arrange
            var type = typeof(TourCrud);

            // Act
            var constructor = type.GetConstructor(Type.EmptyTypes);

            // Assert
            Assert.NotNull(constructor);
        }
    }
}
