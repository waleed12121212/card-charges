using System.ComponentModel.DataAnnotations;

namespace BlazingPizza.Shared;

public class RefillCardPurchaseRequest
{
    [Required]
    public int RefillCardId { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "الكمية يجب أن تكون بين 1 و 100")]
    public int Quantity { get; set; } = 1;
} 