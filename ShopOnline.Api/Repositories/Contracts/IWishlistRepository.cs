using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Repositories.Contracts;

public interface IWishlistRepository
{
    Task<Wishlist?> GetDefaultWishlistByUserId(int userId);
    
    Task<IEnumerable<WishListItem>> GetWishlistItems(int wishlistId);
    Task<WishListItem?> GetWishlistItemByProductId(int wishlistId, int productId);
    Task<WishListItem> AddToWishlist(int wishlistId, WishListItem wishlistItem);
    Task<bool> RemoveFromWishlistByProductId(int wishlistId, int productId);
    Task<bool> ProductExistsInWishlist(int wishlistId, int productId);
    Task<int> GetWishlistItemsCount(int wishlistId);
    Task<bool> ClearWishlist(int wishlistId);
}