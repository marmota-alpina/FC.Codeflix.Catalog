using FC.CodeFlix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
namespace FC.CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        // Arrange
        var validData = new
        {
            Name = "Category 1",
            Description = "Description 1"
        };
        
        // Act
        var datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var datetimeAfter = DateTime.Now;
        
        // Assert
        category.Should().NotBeNull();
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.True(category.IsActive);
    }
    
    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        // Arrange
        var validData = new
        {
            Name = "Category 1",
            Description = "Description 1"
        };
        
        // Act
        var datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var datetimeAfter = DateTime.Now;
        
        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }
    
    [Theory(DisplayName = nameof(ThrowExceptionWhenNameIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ThrowExceptionWhenNameIsNull(string? name)
    {
        // Arrange
        Action action = () => new DomainEntity.Category(name!, "Description 1", true);
        // Act
        var exception = Assert.Throws<EntityValidationException>(action);
        // Assert
        Assert.Equal("Name should not be empty or null", exception.Message);
    }
    
    [Theory(DisplayName = nameof(ThrowExceptionWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ThrowExceptionWhenDescriptionIsNull(string? description)
    {
        // Arrange
        Action action = () => new DomainEntity.Category("Category 1", description!, true);
        // Act
        var exception = Assert.Throws<EntityValidationException>(action);
        // Assert
        Assert.Equal("Description should not be empty or null", exception.Message);
    }
    
    [Theory(DisplayName = nameof(ThrowExceptionWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData(" q ")]
    public void ThrowExceptionWhenNameIsLessThan3Characters(string invalidName)
    {
        // Arrange
        Action action = () => new DomainEntity.Category(invalidName, "Description", true);
        // Act
        var exception = Assert.Throws<EntityValidationException>(action);
        // Assert
        Assert.Equal("Name should have at least 3 characters", exception.Message);
    }
    
    [Fact(DisplayName = nameof(ThrowExceptionWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void ThrowExceptionWhenNameIsGreaterThan255Characters()
    {
        // Arrange
        var veryLongName = new string('a', 256);
        Action action = () => new DomainEntity.Category(veryLongName, "Description", true);
        // Act
        var exception = Assert.Throws<EntityValidationException>(action);
        // Assert
        Assert.Equal("Name should have at most 255 characters", exception.Message);
    }
    [Fact(DisplayName = nameof(ThrowExceptionWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void ThrowExceptionWhenDescriptionIsGreaterThan10_000Characters()
    {
        // Arrange
        var veryLongDescription = new string('a', 10_001);
        Action action = () => new DomainEntity.Category("Name ok", veryLongDescription, true);
        // Act
        var exception = Assert.Throws<EntityValidationException>(action);
        // Assert
        Assert.Equal("Description should have at most 10.000 characters", exception.Message);
    }
    
    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        // Arrange
        var category = new DomainEntity.Category("Name ok", "Description ok", false);
        // Act
        category.Activate();
        // Assert
        Assert.True(category.IsActive);
    }
    
    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        // Arrange
        var category = new DomainEntity.Category("Name ok", "Description ok", true);
        // Act
        category.Deactivate();
        // Assert
        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = new DomainEntity.Category("Name ok", "Description ok", true);
        var validData = new
        {
            Name = "Category 1",
            Description = "Description 1"
        };
        
        category.Update(validData.Name, validData.Description);
        
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
    }
    
    [Fact(DisplayName = nameof(UpdateName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateName()
    {
        var category = new DomainEntity.Category("Name ok", "Description ok", true);
        var validData = new
        {
            Name = "Category 1"
        };
        
        category.Update(validData.Name);
        
        Assert.Equal(validData.Name, category.Name);
    }
    
    [Theory(DisplayName = nameof(ThrowExceptionOnUpdateWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData(" q ")]
    public void ThrowExceptionOnUpdateWhenNameIsLessThan3Characters(string invalidName)
    {
        // Arrange
        var category = new DomainEntity.Category("Name ok", "Description", true);
        Action action = () => category.Update(invalidName, "Description");
        // Act
        var exception = Assert.Throws<EntityValidationException>(action);
        // Assert
        Assert.Equal("Name should have at least 3 characters", exception.Message);
    }
    
    [Fact(DisplayName = nameof(ThrowExceptionOnUpdateWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void ThrowExceptionOnUpdateWhenNameIsGreaterThan255Characters()
    {
        // Arrange
        var category = new DomainEntity.Category("Name ok", "Description", true);
        var veryLongName = new string('a', 256);
        Action action = () => category.Update(veryLongName);
        // Act
        var exception = Assert.Throws<EntityValidationException>(action);
        // Assert
        Assert.Equal("Name should have at most 255 characters", exception.Message);
    }
    
    [Fact(DisplayName = nameof(ThrowExceptionOnUpdateWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void ThrowExceptionOnUpdateWhenDescriptionIsGreaterThan10_000Characters()
    {
        // Arrange
        var category = new DomainEntity.Category("Name ok", "Description", true);
        var veryLongDescription = new string('a', 10_001);
        Action action = () => category.Update("Name ok", veryLongDescription);
        // Act
        var exception = Assert.Throws<EntityValidationException>(action);
        // Assert
        Assert.Equal("Description should have at most 10.000 characters", exception.Message);
    }
    
    [Fact(DisplayName = nameof(IdShouldNotBeNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void IdShouldNotBeNull()
    {
        // Arrange
        var category = new DomainEntity.Category("Name ok", "Description ok", true);
        // Assert
       Assert.NotEqual(default(Guid), category.Id);
    }
}