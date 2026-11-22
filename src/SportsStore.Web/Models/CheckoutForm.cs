using System.ComponentModel.DataAnnotations;

namespace SportsStore.Web.Models;

public class CheckoutForm
{
    [Required(ErrorMessage = "Enter the name for the delivery.")]
    [StringLength(120)]
    [Display(Name = "Name")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter a phone number.")]
    [StringLength(40, MinimumLength = 5)]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter the delivery address.")]
    [StringLength(240, MinimumLength = 5)]
    [Display(Name = "Address")]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter the city.")]
    [StringLength(80, MinimumLength = 2)]
    public string City { get; set; } = string.Empty;
}
