using ShopOnline.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Models.Dtos;
public class WishListItemDto : IProductCollectionItem
{
    public int Id { get; set; }
    public int WishlistId { get; set; }
    [Required]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public string ProductImageUrl { get; set; } = string.Empty;
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Qty { get; set; } = 1;
    [MaxLength(500)]
    public string? Notes { get; set; }
    [Range(1, 3, ErrorMessage = "Priority must be between 1 (Low) and 3 (High)")]
    public int Priority { get; set; } = 1;
    public decimal TotalPrice { get; set; }
}