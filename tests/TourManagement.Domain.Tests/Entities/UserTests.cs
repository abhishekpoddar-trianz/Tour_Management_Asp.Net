using Xunit;
using TourManagement.Domain.Entities;

namespace TourManagement.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void User_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.NotNull(user);
        Assert.Equal(string.Empty, user.Email);
        Assert.Equal(string.Empty, user.FirstName);
        Assert.Equal(string.Empty, user.LastName);
        Assert.Equal(string.Empty, user.Gender);
        Assert.Equal(string.Empty, user.Password);
        Assert.Equal(string.Empty, user.Street);
        Assert.Equal(string.Empty, user.City);
        Assert.Equal(string.Empty, user.State);
        Assert.Equal(string.Empty, user.CreatedBy);
        Assert.Null(user.ModifiedBy);
        Assert.Null(user.ModifiedDate);
        Assert.NotNull(user.Bookings);
        Assert.Empty(user.Bookings);
    }

    [Fact]
    public void User_SetEmail_ShouldSetEmailProperty()
    {
        // Arrange
        var user = new User();
        var email = "test@example.com";

        // Act
        user.Email = email;

        // Assert
        Assert.Equal(email, user.Email);
    }

    [Fact]
    public void User_SetFirstName_ShouldSetFirstNameProperty()
    {
        // Arrange
        var user = new User();
        var firstName = "John";

        // Act
        user.FirstName = firstName;

        // Assert
        Assert.Equal(firstName, user.FirstName);
    }

    [Fact]
    public void User_SetLastName_ShouldSetLastNameProperty()
    {
        // Arrange
        var user = new User();
        var lastName = "Doe";

        // Act
        user.LastName = lastName;

        // Assert
        Assert.Equal(lastName, user.LastName);
    }

    [Fact]
    public void User_SetGender_ShouldSetGenderProperty()
    {
        // Arrange
        var user = new User();
        var gender = "Male";

        // Act
        user.Gender = gender;

        // Assert
        Assert.Equal(gender, user.Gender);
    }

    [Fact]
    public void User_SetPassword_ShouldSetPasswordProperty()
    {
        // Arrange
        var user = new User();
        var password = "SecurePassword123";

        // Act
        user.Password = password;

        // Assert
        Assert.Equal(password, user.Password);
    }

    [Fact]
    public void User_SetDateOfBirth_ShouldSetDateOfBirthProperty()
    {
        // Arrange
        var user = new User();
        var dateOfBirth = new DateTime(1990, 1, 1);

        // Act
        user.DateOfBirth = dateOfBirth;

        // Assert
        Assert.Equal(dateOfBirth, user.DateOfBirth);
    }

    [Fact]
    public void User_SetStreet_ShouldSetStreetProperty()
    {
        // Arrange
        var user = new User();
        var street = "123 Main St";

        // Act
        user.Street = street;

        // Assert
        Assert.Equal(street, user.Street);
    }

    [Fact]
    public void User_SetCity_ShouldSetCityProperty()
    {
        // Arrange
        var user = new User();
        var city = "New York";

        // Act
        user.City = city;

        // Assert
        Assert.Equal(city, user.City);
    }

    [Fact]
    public void User_SetState_ShouldSetStateProperty()
    {
        // Arrange
        var user = new User();
        var state = "NY";

        // Act
        user.State = state;

        // Assert
        Assert.Equal(state, user.State);
    }

    [Fact]
    public void User_SetCreatedDate_ShouldSetCreatedDateProperty()
    {
        // Arrange
        var user = new User();
        var createdDate = DateTime.UtcNow;

        // Act
        user.CreatedDate = createdDate;

        // Assert
        Assert.Equal(createdDate, user.CreatedDate);
    }

    [Fact]
    public void User_SetModifiedDate_ShouldSetModifiedDateProperty()
    {
        // Arrange
        var user = new User();
        var modifiedDate = DateTime.UtcNow;

        // Act
        user.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, user.ModifiedDate);
    }

    [Fact]
    public void User_SetIsActive_ShouldSetIsActiveProperty()
    {
        // Arrange
        var user = new User();

        // Act
        user.IsActive = true;

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void User_SetCreatedBy_ShouldSetCreatedByProperty()
    {
        // Arrange
        var user = new User();
        var createdBy = "admin";

        // Act
        user.CreatedBy = createdBy;

        // Assert
        Assert.Equal(createdBy, user.CreatedBy);
    }

    [Fact]
    public void User_SetModifiedBy_ShouldSetModifiedByProperty()
    {
        // Arrange
        var user = new User();
        var modifiedBy = "admin";

        // Act
        user.ModifiedBy = modifiedBy;

        // Assert
        Assert.Equal(modifiedBy, user.ModifiedBy);
    }

    [Fact]
    public void User_AddBooking_ShouldAddBookingToCollection()
    {
        // Arrange
        var user = new User();
        var booking = new Booking { Id = 1, TourName = "Test Tour" };

        // Act
        user.Bookings.Add(booking);

        // Assert
        Assert.Single(user.Bookings);
        Assert.Contains(booking, user.Bookings);
    }

    [Fact]
    public void User_MultipleBookings_ShouldMaintainAllBookings()
    {
        // Arrange
        var user = new User();
        var booking1 = new Booking { Id = 1, TourName = "Tour 1" };
        var booking2 = new Booking { Id = 2, TourName = "Tour 2" };

        // Act
        user.Bookings.Add(booking1);
        user.Bookings.Add(booking2);

        // Assert
        Assert.Equal(2, user.Bookings.Count);
        Assert.Contains(booking1, user.Bookings);
        Assert.Contains(booking2, user.Bookings);
    }
}
