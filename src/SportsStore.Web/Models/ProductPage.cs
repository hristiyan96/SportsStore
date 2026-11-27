using System.ComponentModel.DataAnnotations;
using SportsStore.Services.Catalog;
using SportsStore.Services.Reviews;

namespace SportsStore.Web.Models;

public class ProductPage
{
    public ProductDetails Product { get; set; } = new();

    public IReadOnlyList<ProductReviewItem> Reviews { get; set; } = Array.Empty<ProductReviewItem>();

    [Range(1, 5, ErrorMessage = "Choose a rating from 1 to 5.")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "Write a short review.")]
    [StringLength(1000, ErrorMessage = "A review can be at most 1000 characters.")]
    public string Comment { get; set; } = string.Empty;

    public bool CanReview { get; set; }

    public bool AlreadyReviewed { get; set; }
}
