using System.Net.Http.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using ShopOnline.Web.Services.Contracts.BaseContracts;

namespace ShopOnline.Web.Services;

public class WishListService : IWishListService
{
    private readonly HttpClient _httpClient;
    private readonly IShoppingCartService _shopCartService;
    private readonly ICacheService _cacheService;
    private readonly ILogger<WishListService> _logger;
    private const int DefaultUserId = HardCoded.UserId;

    private readonly string _wishListKey = "WishListItems";
    public event Action<int>? OnCollectionChanged = delegate { };
    public WishListService(IShoppingCartService shopCartService, HttpClient httpClient, ICacheService cacheService, ILogger<WishListService> logger)
    {
        _shopCartService = shopCartService;
        _httpClient = httpClient;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<IEnumerable<WishListItemDto>> GetItems()
    {
        return await GetWishListItemsInternal();
    }

    public async Task<int> GetItemsCount()
    {
        return await GetWishListItemsCountInternal();
    }

    public async Task<WishListItemDto> AddItem(int productId)
    {
        return await AddToWishListInternal(productId);
    }

    public async Task<bool> RemoveItem(int productId)
    {
        return await RemoveFromWishListInternal(productId);
    }

    public async Task<bool> ContainsItem(int productId)
    {
        return await IsInWishListInternal(productId);
    }

    public async Task ClearCollection()
    {
        await ClearWishListInternal();
    }



    public async Task<WishListItemDto?> UpdateItem(int productId, Func<WishListItemDto, WishListItemDto> updateAction)
    {
        try
        {
            var wishListItems = (await GetWishListItemsInternal()).ToList();
            var existingItem = wishListItems.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
            {
                var updatedItem = updateAction(existingItem);
                var index = wishListItems.IndexOf(existingItem);
                wishListItems[index] = updatedItem;

                _cacheService.Add(_wishListKey, wishListItems, TimeSpan.FromMinutes(2));
                OnCollectionChanged?.Invoke(wishListItems.Count);

                return updatedItem;
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<WishListItemDto?> GetItem(int productId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Wishlist/user/{DefaultUserId}/items/{productId}");

            if (response.IsSuccessStatusCode)
            {
                var item = await response.Content.ReadFromJsonAsync<WishListItemDto>();
                return item;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }


            var wishListItems = await GetWishListItemsInternal();
            return wishListItems.FirstOrDefault(x => x.ProductId == productId);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> TransferItemTo<TTargetItem>(int productId,
        IBaseCollectionService<TTargetItem, int> targetService) where TTargetItem : class
    {
        try
        {
            var item = await GetItem(productId);
            if (item != null)
            {
                await targetService.AddItem(productId);

                return await RemoveItem(productId);
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public event Action<int>? OnWishListChanged;


    public event Action<int> OnWishlistChanged
    {
        add => OnCollectionChanged += value;
        remove => OnCollectionChanged -= value;
    }

    public async Task<IEnumerable<WishListItemDto>> GetWishListItems()
    {
        return await GetItems();
    }

    public async Task<WishListItemDto> AddToWishList(int productId)
    {
        return await AddItem(productId);
    }

    public async Task<bool> RemoveFromWishList(int productId)
    {
        return await RemoveItem(productId);
    }

    public async Task<bool> IsInWishList(int productId)
    {
        return await ContainsItem(productId);
    }

    public async Task<int> GetWishListItemsCount()
    {
        return await GetItemsCount();
    }

    public async Task ClearWishList()
    {
        await ClearCollection();
    }

    public async Task<bool> MoveToCart(int productId)
    {
        try
        {
            var wishListItems = (await GetWishListItemsInternal()).ToList();
            var wishListItem = wishListItems.FirstOrDefault(x => x.ProductId == productId);

            if (wishListItem != null)
            {
                await _shopCartService.AddItem(new CartItemToAddDto { CartId = HardCoded.CartId, ProductId = productId, Qty = 1 });

                await RemoveFromWishList(productId);
                return true;
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<WishListItemDto>> GetWishListItemsSorted()
    {
        try
        {
            var items = await GetWishListItemsInternal();
            return items.OrderByDescending(x => x.DateAdded);
        }
        catch (Exception)
        {
            return new List<WishListItemDto>();
        }
    }

    public async Task<decimal> GetWishListTotalValue()
    {
        try
        {
            var items = await GetWishListItemsInternal();
            return items.Sum(x => x.Price);
        }
        catch (Exception)
        {
            return 0;
        }
    }


    #region Private Implementation Methods

    private async Task<IEnumerable<WishListItemDto>> GetWishListItemsInternal()
    {
        try
        {

            var response = await _httpClient.GetAsync($"api/Wishlist/user/{DefaultUserId}");
            if (response.IsSuccessStatusCode)
            {
                var items = await response.Content.ReadFromJsonAsync<List<WishListItemDto>>();
                return items ?? new List<WishListItemDto>();
            }
        }
        catch (Exception)
        {
            return await Task.FromResult<IEnumerable<WishListItemDto>>(new List<WishListItemDto>());
        }
        return await Task.FromResult<IEnumerable<WishListItemDto>>(new List<WishListItemDto>());
    }

    private async Task<WishListItemDto> AddToWishListInternal(int productId)
    {
        try
        {
            var items = await GetWishListItemsInternal();
            var wishListItems = items.ToList();
            
            var existingItem = wishListItems.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                return existingItem; 
            }

            var response = await _httpClient.PostAsync($"api/Wishlist/user/{DefaultUserId}/items/{productId}", null);


            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to add item to WishList");
            }

            var wishlistItem = await response.Content.ReadFromJsonAsync<WishListItemDto>();
            if (wishlistItem != null)
            {
                _ = Task.Run(async () =>
                {
                    var count = await GetWishListItemsCountInternal();
                    OnCollectionChanged?.Invoke(count);
                });

                _logger.LogInformation("Successfully added ProductId: {ProductId} to wishlist", productId);
                return wishlistItem;
            }

            return wishlistItem ?? new WishListItemDto();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error adding to WishList: {ex.Message}");
        }
    }

    private async Task<bool> RemoveFromWishListInternal(int productId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Wishlist/user/{DefaultUserId}/items/{productId}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                if (!result) return result;
                _ = Task.Run(async () =>
                {
                    var count = await GetWishListItemsCountInternal();
                    OnCollectionChanged?.Invoke(count);
                });

                _logger.LogInformation("Successfully removed ProductId: {ProductId} from wishlist", productId);

                return result;

            }
        }
        catch (Exception)
        {
            return false;
        }

        return false;
    }

    private async Task<bool> IsInWishListInternal(int productId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Wishlist/user/{DefaultUserId}/items/{productId}/exists");
            if (response.IsSuccessStatusCode)
            {
                var exists = await response.Content.ReadFromJsonAsync<bool>();
                return exists;
            }

            _logger.LogWarning("Failed to check if item exists in wishlist. Status: {StatusCode}", response.StatusCode);
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private async Task<int> GetWishListItemsCountInternal()
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Wishlist/user/{DefaultUserId}/count");
            if (response.IsSuccessStatusCode)
            {
                var count = await response.Content.ReadFromJsonAsync<int>();
                return count;
            }

            _logger.LogWarning("Failed to get wishlist items count. Status: {StatusCode}", response.StatusCode);
            return 0;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    private async  Task ClearWishListInternal()
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Wishlist/user/{DefaultUserId}/clear");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                if (result)
                {
                    OnCollectionChanged?.Invoke(0);
                    _logger.LogInformation("Successfully cleared wishlist");
                }
            }
            else
            {
                _logger.LogWarning("Failed to clear wishlist. Status: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("{error}", ex.Message);
        }
        
    }

    #endregion

}