using Microsoft.EntityFrameworkCore;
using SportsStore.Core.Entities;
using SportsStore.Data;

namespace SportsStore.Services.Reviews;

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _db;

    public ReviewService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductReviewItem>> ListAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await _db.ProductReviews
            .AsNoTracking()
            .Where(review => review.ProductId == productId)
            .OrderByDescending(review => review.CreatedOn)
            .Select(review => new ProductReviewItem
            {
                Author = (review.User.FirstName + " " + review.User.LastName).Trim(),
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedOn = review.CreatedOn
            })
            .ToListAsync(cancellationToken);
    }

    public Task<bool> HasReviewedAsync(int productId, string userId, CancellationToken cancellationToken = default)
    {
        return _db.ProductReviews.AnyAsync(review => review.ProductId == productId && review.UserId == userId, cancellationToken);
    }

    public async Task<string?> AddAsync(int productId, string userId, int rating, string comment, CancellationToken cancellationToken = default)
    {
        var text = comment.Trim();
        if (rating is < 1 or > 5 || text.Length is < 1 or > 1000)
        {
            return "Choose a rating from 1 to 5 and write a review of at most 1000 characters.";
        }

        var productIsActive = await _db.Products.AnyAsync(product => product.Id == productId && product.IsActive, cancellationToken);
        if (!productIsActive)
        {
            return "That product is not for sale.";
        }

        var alreadyReviewed = await HasReviewedAsync(productId, userId, cancellationToken);
        if (alreadyReviewed)
        {
            return "You have already reviewed this product.";
        }

        _db.ProductReviews.Add(new ProductReview
        {
            ProductId = productId,
            UserId = userId,
            Rating = rating,
            Comment = text,
            CreatedOn = DateTime.UtcNow
        });
        await _db.SaveChangesAsync(cancellationToken);
        return null;
    }
}
