using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Api.Entities;


public class Wishlist
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = "My Wishlist";

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime LastModified { get; set; } = DateTime.Now;

    public bool IsPublic { get; set; } = false;

    public bool IsDefault { get; set; } = true;

    public User User { get; set; } = null!;
    public ICollection<WishListItem> WishlistItems { get; set; } = new List<WishListItem>();
}
