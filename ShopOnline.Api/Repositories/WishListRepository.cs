using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Repositories;

public class WishListRepository : IWishlistRepository
{
    private readonly ShopOnlineDbContext _context;

    public WishListRepository(ShopOnlineDbContext context)
    {
        _context = context;
    }

    public async Task<Wishlist?> GetDefaultWishlistByUserId(int userId)
    {
        var defaultWishlist = await _context.WishLists
            .Include(w => w.WishlistItems)
                .ThenInclude(wi => wi.Product)
               // .ThenInclude(p => p.CategoryId)
            .FirstOrDefaultAsync(w => w.UserId == userId && w.IsDefault);

        if (defaultWishlist != null) return defaultWishlist;
        {
            defaultWishlist = new Wishlist
            {
                UserId = userId,
                Name = "My Wishlist",
                Description = "Default wishlist",
                IsDefault = true,
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now
            };

            _context.WishLists.Add(defaultWishlist);
            await _context.SaveChangesAsync();

            // Reload with includes
            defaultWishlist = await _context.WishLists
                .Include(w => w.WishlistItems)
                .ThenInclude(wi => wi.Product)
                //.ThenInclude(p => p.CategoryId)
                .FirstOrDefaultAsync(w => w.Id == defaultWishlist.Id);
        }

        return defaultWishlist;
    }

    public async Task<IEnumerable<WishListItem>> GetWishlistItems(int wishlistId)
    {
        return await _context.WishListItems
            .Where(wi => wi.WishlistId == wishlistId)
            .Include(wi => wi.Product)
              //  .ThenInclude(p => p.CategoryId)
            .OrderByDescending(wi => wi.DateAdded)
            .ToListAsync();
    }

    public async Task<WishListItem?> GetWishlistItemByProductId(int wishlistId, int productId)
    {
        return await _context.WishListItems
            .Include(wi => wi.Product)
              //  .ThenInclude(p => p.CategoryId)
            .FirstOrDefaultAsync(wi => wi.WishlistId == wishlistId && wi.ProductId == productId);
    }

    public async Task<WishListItem> AddToWishlist(int wishlistId, WishListItem wishlistItem)
    {
        var existingItem = await GetWishlistItemByProductId(wishlistId, wishlistItem.ProductId);
        if (existingItem != null)
        {
            existingItem.Qty += wishlistItem.Qty;
            existingItem.DateAdded = DateTime.Now;
            if (!string.IsNullOrEmpty(wishlistItem.Notes))
            {
                existingItem.Notes = wishlistItem.Notes;
            }
            existingItem.Priority = wishlistItem.Priority;

            await _context.SaveChangesAsync();
            return existingItem;
        }

        wishlistItem.WishlistId = wishlistId;
        wishlistItem.DateAdded = DateTime.Now;

        _context.WishListItems.Add(wishlistItem);
        
        var wishlist = await _context.WishLists.FindAsync(wishlistId);
        if (wishlist != null)
        {
            wishlist.LastModified = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        
        return await _context.WishListItems
            .Include(wi => wi.Product)
               // .ThenInclude(p => p.CategoryId)
            .FirstAsync(wi => wi.Id == wishlistItem.Id);
    }

    public async Task<bool> RemoveFromWishlistByProductId(int wishlistId, int productId)
    {
        var wishlistItem = await GetWishlistItemByProductId(wishlistId, productId);
        if (wishlistItem == null) return false;

        _context.WishListItems.Remove(wishlistItem);
        
        var wishlist = await _context.WishLists.FindAsync(wishlistId);
        if (wishlist != null)
        {
            wishlist.LastModified = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ProductExistsInWishlist(int wishlistId, int productId)
    {
        return await _context.WishListItems
            .AnyAsync(wi => wi.WishlistId == wishlistId && wi.ProductId == productId);
    }

    public async Task<int> GetWishlistItemsCount(int wishlistId)
    {
        return await _context.WishListItems
            .Where(wi => wi.WishlistId == wishlistId)
            .SumAsync(wi => wi.Qty);
    }

    public async Task<bool> ClearWishlist(int wishlistId)
    {
        var wishlistItems = await _context.WishListItems
            .Where(wi => wi.WishlistId == wishlistId)
            .ToListAsync();

        if (!wishlistItems.Any()) return true;

        _context.WishListItems.RemoveRange(wishlistItems);
        
        var wishlist = await _context.WishLists.FindAsync(wishlistId);
        if (wishlist != null)
        {
            wishlist.LastModified = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
