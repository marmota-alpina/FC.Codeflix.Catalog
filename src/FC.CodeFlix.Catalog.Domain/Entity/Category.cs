using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Domain.SeedWork;

namespace FC.CodeFlix.Catalog.Domain.Entity;

public class Category : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    public Category(string name, string? description) : base()
    {
        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description?.Trim();
        IsActive = true;
        CreatedAt = DateTime.Now;
        Validate();
    }

    public Category(string name, string? description, bool isActive) : base()
    {
        Id = Guid.NewGuid();
        Name = name?.Trim();
        Description = description?.Trim();
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        Validate();
    }

    private void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new EntityValidationException($"Name should not be empty or null");
        switch (name.Length)
        {
            case < 3:
                throw new EntityValidationException($"Name should have at least 3 characters");
            case > 255:
                throw new EntityValidationException($"Name should have at most 255 characters");
        }
    }

    private void ValidateDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            throw new EntityValidationException($"Description should not be empty or null");
        if (description.Length > 10_000)
            throw new EntityValidationException($"Description should have at most 10.000 characters");
    }
    private void Validate()
    {
        ValidateName(Name);
        ValidateDescription(Description);
    }

    public void Activate()
    {
        this.IsActive = true;
        this.Validate();
    }

    public void Deactivate()
    {
        this.IsActive = false;
        this.Validate();
    }

    public void Update(string name, string description)
    {
        name = name.Trim();
        description = description.Trim();
        
        ValidateName(name);
        ValidateDescription(description);
        
        Name = name;
        Description = description;
    }
    public void Update(string name)
    {
        name = name.Trim();
        ValidateName(name);
        Name = name;
    }
}