namespace SportsStore.Core.Entities;

public class ProductReview : BaseEntity
{
    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;
}
