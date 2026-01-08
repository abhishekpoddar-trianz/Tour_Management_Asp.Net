using Xunit;
using TourManagement.Domain.DTOs;

namespace TourManagement.Domain.Tests.DTOs;

public class UserDtoTests
{
    [Fact]
    public void UserDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new UserDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Equal(string.Empty, dto.LastName);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Street);
        Assert.Equal(string.Empty, dto.City);
        Assert.Equal(string.Empty, dto.State);
        Assert.Null(dto.ModifiedDate);
    }

    [Fact]
    public void UserDto_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var dto = new UserDto();
        var dateOfBirth = new DateTime(1990, 1, 1);
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow;

        // Act
        dto.Email = "user@example.com";
        dto.FirstName = "John";
        dto.LastName = "Doe";
        dto.Gender = "Male";
        dto.DateOfBirth = dateOfBirth;
        dto.Street = "123 Main St";
        dto.City = "New York";
        dto.State = "NY";
        dto.CreatedDate = createdDate;
        dto.ModifiedDate = modifiedDate;
        dto.IsActive = true;

        // Assert
        Assert.Equal("user@example.com", dto.Email);
        Assert.Equal("John", dto.FirstName);
        Assert.Equal("Doe", dto.LastName);
        Assert.Equal("Male", dto.Gender);
        Assert.Equal(dateOfBirth, dto.DateOfBirth);
        Assert.Equal("123 Main St", dto.Street);
        Assert.Equal("New York", dto.City);
        Assert.Equal("NY", dto.State);
        Assert.Equal(createdDate, dto.CreatedDate);
        Assert.Equal(modifiedDate, dto.ModifiedDate);
        Assert.True(dto.IsActive);
    }
}

public class UserCreateDtoTests
{
    [Fact]
    public void UserCreateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new UserCreateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Equal(string.Empty, dto.LastName);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Password);
        Assert.Equal(string.Empty, dto.Street);
        Assert.Equal(string.Empty, dto.City);
        Assert.Equal(string.Empty, dto.State);
    }

    [Fact]
    public void UserCreateDto_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var dto = new UserCreateDto();
        var dateOfBirth = new DateTime(1990, 1, 1);

        // Act
        dto.Email = "newuser@example.com";
        dto.FirstName = "Jane";
        dto.LastName = "Smith";
        dto.Gender = "Female";
        dto.Password = "SecurePassword123";
        dto.DateOfBirth = dateOfBirth;
        dto.Street = "456 Oak Ave";
        dto.City = "Los Angeles";
        dto.State = "CA";

        // Assert
        Assert.Equal("newuser@example.com", dto.Email);
        Assert.Equal("Jane", dto.FirstName);
        Assert.Equal("Smith", dto.LastName);
        Assert.Equal("Female", dto.Gender);
        Assert.Equal("SecurePassword123", dto.Password);
        Assert.Equal(dateOfBirth, dto.DateOfBirth);
        Assert.Equal("456 Oak Ave", dto.Street);
        Assert.Equal("Los Angeles", dto.City);
        Assert.Equal("CA", dto.State);
    }

    [Fact]
    public void UserCreateDto_WithEmptyPassword_ShouldAllowEmpty()
    {
        // Arrange & Act
        var dto = new UserCreateDto { Password = "" };

        // Assert
        Assert.Equal("", dto.Password);
    }
}

public class UserUpdateDtoTests
{
    [Fact]
    public void UserUpdateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new UserUpdateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Equal(string.Empty, dto.LastName);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Street);
        Assert.Equal(string.Empty, dto.City);
        Assert.Equal(string.Empty, dto.State);
    }

    [Fact]
    public void UserUpdateDto_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var dto = new UserUpdateDto();
        var dateOfBirth = new DateTime(1985, 5, 15);

        // Act
        dto.FirstName = "Updated";
        dto.LastName = "Name";
        dto.Gender = "Other";
        dto.DateOfBirth = dateOfBirth;
        dto.Street = "789 Pine Rd";
        dto.City = "Chicago";
        dto.State = "IL";

        // Assert
        Assert.Equal("Updated", dto.FirstName);
        Assert.Equal("Name", dto.LastName);
        Assert.Equal("Other", dto.Gender);
        Assert.Equal(dateOfBirth, dto.DateOfBirth);
        Assert.Equal("789 Pine Rd", dto.Street);
        Assert.Equal("Chicago", dto.City);
        Assert.Equal("IL", dto.State);
    }

    [Fact]
    public void UserUpdateDto_WithAllEmptyStrings_ShouldAllowEmpty()
    {
        // Arrange & Act
        var dto = new UserUpdateDto
        {
            FirstName = "",
            LastName = "",
            Gender = "",
            Street = "",
            City = "",
            State = ""
        };

        // Assert
        Assert.Equal("", dto.FirstName);
        Assert.Equal("", dto.LastName);
        Assert.Equal("", dto.Gender);
        Assert.Equal("", dto.Street);
        Assert.Equal("", dto.City);
        Assert.Equal("", dto.State);
    }
}

public class UserLoginDtoTests
{
    [Fact]
    public void UserLoginDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new UserLoginDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.Password);
    }

    [Fact]
    public void UserLoginDto_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var dto = new UserLoginDto();

        // Act
        dto.Email = "login@example.com";
        dto.Password = "Password123";

        // Assert
        Assert.Equal("login@example.com", dto.Email);
        Assert.Equal("Password123", dto.Password);
    }

    [Fact]
    public void UserLoginDto_WithEmptyCredentials_ShouldAllowEmpty()
    {
        // Arrange & Act
        var dto = new UserLoginDto
        {
            Email = "",
            Password = ""
        };

        // Assert
        Assert.Equal("", dto.Email);
        Assert.Equal("", dto.Password);
    }

    [Fact]
    public void UserLoginDto_WithValidCredentials_ShouldHoldValues()
    {
        // Arrange & Act
        var dto = new UserLoginDto
        {
            Email = "test@test.com",
            Password = "Test@123"
        };

        // Assert
        Assert.Equal("test@test.com", dto.Email);
        Assert.Equal("Test@123", dto.Password);
    }
}
