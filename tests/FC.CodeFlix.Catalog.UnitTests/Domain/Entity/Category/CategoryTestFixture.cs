using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTestFixture
{
    public DomainEntity.Category GetValidCategory(bool isActive = true) => 
        new("Category name", "Description", isActive);
}
[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
{
}