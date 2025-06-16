using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Utils;

public static class WishlistDtoConversion
{
    public static WishListItemDto ConvertToDto(this WishListItem wishlistItem)
    {
        if (wishlistItem == null)
            throw new ArgumentNullException(nameof(wishlistItem));

        return new WishListItemDto
        {
            Id = wishlistItem.Id,
            WishlistId = wishlistItem.WishlistId,
            ProductId = wishlistItem.ProductId,
            ProductName = wishlistItem.Product?.Name ?? string.Empty,
            ProductDescription = wishlistItem.Product?.Description ?? string.Empty,
            ProductImageUrl = wishlistItem.Product?.ImageURL ?? string.Empty,
            Price = wishlistItem.Product?.Price ?? 0,
            // CategoryName = wishlistItem.Product?.CategoryId?.Name ?? string.Empty,
            DateAdded = wishlistItem.DateAdded,
            Qty = wishlistItem.Qty,
            Notes = wishlistItem.Notes,
            Priority = wishlistItem.Priority,
            TotalPrice = wishlistItem.TotalPrice
        };
    }

    public static IEnumerable<WishListItemDto> ConvertToDto(this IEnumerable<WishListItem>? wishlistItems)
    {
        return wishlistItems == null ? new List<WishListItemDto>() : wishlistItems.Select(item => item.ConvertToDto());
    }

    public static WishListItem ConvertToEntity(this WishListItemDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        return new WishListItem
        {
            Id = dto.Id,
            WishlistId = dto.WishlistId,
            ProductId = dto.ProductId,
            DateAdded = dto.DateAdded,
            Qty = dto.Qty,
            Notes = dto.Notes,
            Priority = dto.Priority
        };
    }

    public static WishListItem CreateWishlistItem(int productId, int quantity = 1, int priority = 1, string? notes = null)
    {
        return new WishListItem
        {
            ProductId = productId,
            Qty = quantity,
            Priority = priority,
            Notes = notes,
            DateAdded = DateTime.Now
        };
    }
}