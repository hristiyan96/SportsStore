using Microsoft.AspNetCore.Identity;

namespace SportsStore.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime RegisteredOn { get; set; } = DateTime.UtcNow;

    public string FullName => $"{FirstName} {LastName}".Trim();

    public ICollection<CustomerOrder> Orders { get; set; } = new List<CustomerOrder>();

    public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
}
