namespace SportsStore.Core.Entities;

public class Brand : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
