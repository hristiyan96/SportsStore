using System.ComponentModel.DataAnnotations;
using SportsStore.Services.Admin;

namespace SportsStore.Web.Models;

public class BrandForm
{
    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Country { get; set; } = string.Empty;

    [Display(Name = "Shown in the shop")]
    public bool IsActive { get; set; } = true;

    public BrandInput ToInput()
    {
        return new BrandInput
        {
            Name = Name,
            Country = Country,
            IsActive = IsActive
        };
    }

    public static BrandForm From(BrandInput input)
    {
        return new BrandForm
        {
            Name = input.Name,
            Country = input.Country,
            IsActive = input.IsActive
        };
    }
}
