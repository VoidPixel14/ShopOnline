using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts;

using BaseContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


    /// <summary>
    /// WishList service interface extending the base collection service
    /// Provides WishList-specific functionality while inheriting common collection operations
    /// </summary>
    public interface IWishListService : IExtendedCollectionService<WishListItemDto, int>
    {
        event Action<int> OnWishListChanged;
    
        Task<IEnumerable<WishListItemDto>> GetWishListItems();
    
        Task<WishListItemDto> AddToWishList(int productId);
    
        Task<bool> RemoveFromWishList(int productId);
    
        Task<bool> IsInWishList(int productId);
    
        Task<int> GetWishListItemsCount();
    
        Task ClearWishList();
    
        Task<bool> MoveToCart(int productId);

        Task<IEnumerable<WishListItemDto>> GetWishListItemsSorted();

        Task<decimal> GetWishListTotalValue();
    }

   
