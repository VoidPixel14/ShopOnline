using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Api.Entities;

public class WishListItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int WishlistId { get; set; }

    [Required]
    public int ProductId { get; set; }

    public DateTime DateAdded { get; set; } = DateTime.Now;

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Qty { get; set; } = 1;

    [MaxLength(500)]
    public string? Notes { get; set; }

    public int Priority { get; set; } = 1; // 1 = Low, 2 = Medium, 3 = High


    public Wishlist Wishlist { get; set; } = null!;
    public Product Product { get; set; } = null!;
    
    [NotMapped]
    public decimal TotalPrice => Product?.Price * Qty ?? 0;
}