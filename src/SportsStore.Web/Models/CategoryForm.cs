using System.ComponentModel.DataAnnotations;
using SportsStore.Services.Admin;

namespace SportsStore.Web.Models;

public class CategoryForm
{
    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Shown in the shop")]
    public bool IsActive { get; set; } = true;

    public CategoryInput ToInput()
    {
        return new CategoryInput
        {
            Name = Name,
            Description = Description,
            IsActive = IsActive
        };
    }

    public static CategoryForm From(CategoryInput input)
    {
        return new CategoryForm
        {
            Name = input.Name,
            Description = input.Description,
            IsActive = input.IsActive
        };
    }
}
