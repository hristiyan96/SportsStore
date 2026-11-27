namespace SportsStore.Services.Reviews;

public class ProductReviewItem
{
    public string Author { get; init; } = string.Empty;

    public int Rating { get; init; }

    public string Comment { get; init; } = string.Empty;

    public DateTime CreatedOn { get; init; }
}
