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
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().BeAfter(datetimeBefore).And.BeBefore(datetimeAfter);
        category.IsActive.Should().BeTrue();
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
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().BeAfter(datetimeBefore).And.BeBefore(datetimeAfter);
        category.IsActive.Should().Be(isActive);
    }
    
    [Theory(DisplayName = nameof(ThrowExceptionWhenNameIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ThrowExceptionWhenNameIsNull(string? name)
    {
        Action action = () => new DomainEntity.Category(name!, "Description 1", true);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }
    
    [Theory(DisplayName = nameof(ThrowExceptionWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ThrowExceptionWhenDescriptionIsNull(string? description)
    {
        Action action = () => new DomainEntity.Category("Category 1", description!, true);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be empty or null");
    }
    
    [Theory(DisplayName = nameof(ThrowExceptionWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData(" q ")]
    public void ThrowExceptionWhenNameIsLessThan3Characters(string invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName, "Description", true);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at least 3 characters");
    }
    
    [Fact(DisplayName = nameof(ThrowExceptionWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void ThrowExceptionWhenNameIsGreaterThan255Characters()
    {
        var veryLongName = new string('a', 256);
        Action action = () => new DomainEntity.Category(veryLongName, "Description", true);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at most 255 characters");
    }
    [Fact(DisplayName = nameof(ThrowExceptionWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void ThrowExceptionWhenDescriptionIsGreaterThan10_000Characters()
    {
        var veryLongDescription = new string('a', 10_001);
        Action action = () => new DomainEntity.Category("Name ok", veryLongDescription, true);
        action.Should()
           .Throw<EntityValidationException>()
           .WithMessage("Description should have at most 10.000 characters");
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
        category.IsActive.Should().BeTrue();
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
        category.IsActive.Should().BeFalse();
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
        
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
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
        
        category.Name.Should().Be(validData.Name);
    }
    
    [Theory(DisplayName = nameof(ThrowExceptionOnUpdateWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData(" q ")]
    public void ThrowExceptionOnUpdateWhenNameIsLessThan3Characters(string invalidName)
    {
        var category = new DomainEntity.Category("Name ok", "Description", true);
        var action = () => category.Update(invalidName, "Description");
        
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at least 3 characters");
    }
    
    [Fact(DisplayName = nameof(ThrowExceptionOnUpdateWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void ThrowExceptionOnUpdateWhenNameIsGreaterThan255Characters()
    {
        var category = new DomainEntity.Category("Name ok", "Description", true);
        var veryLongName = new string('a', 256);
        var action = () => category.Update(veryLongName);
        
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should have at most 255 characters");
    }
    
    [Fact(DisplayName = nameof(ThrowExceptionOnUpdateWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void ThrowExceptionOnUpdateWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = new DomainEntity.Category("Name ok", "Description", true);
        var veryLongDescription = new string('a', 10_001);
        var action = () => category.Update("Name ok", veryLongDescription);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should have at most 10.000 characters");
    }
}