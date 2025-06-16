using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Api.Utils;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WishlistController : ControllerBase
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IProductRepository _productRepository;

    public WishlistController(IWishlistRepository wishlistRepository, IProductRepository productRepository)
    {
        _wishlistRepository = wishlistRepository;
        _productRepository = productRepository;
    }

    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<IEnumerable<WishListItemDto>>> GetWishlistItems(int userId)
    {
        try
        {
            var defaultWishlist = await _wishlistRepository.GetDefaultWishlistByUserId(userId);
            if (defaultWishlist == null)
            {
                return Ok(new List<WishListItemDto>());
            }

            var items = await _wishlistRepository.GetWishlistItems(defaultWishlist.Id);
            return Ok(items.ConvertToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("user/{userId:int}/items/{productId:int}")]
    public async Task<ActionResult<WishListItemDto>> AddToWishlist(int userId, int productId)
    {
        try
        {
            var productExists = await _productRepository.GetItem(productId);
            if (productExists.Id == 0)
            {
                return NotFound($"Product with ID: {productId} not found");
            }
            
            var defaultWishlist = await _wishlistRepository.GetDefaultWishlistByUserId(userId);
            if (defaultWishlist == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create default wishlist");
            }
            
            var existingItem = await _wishlistRepository.GetWishlistItemByProductId(defaultWishlist.Id, productId);
            if (existingItem != null)
            {
                return Ok(existingItem.ConvertToDto()); 
            }
            
            var wishlistItem = new Entities.WishListItem
            {
                ProductId = productId,
                Qty = 1,
                Priority = 1,
                DateAdded = DateTime.Now
            };

            var addedItem = await _wishlistRepository.AddToWishlist(defaultWishlist.Id, wishlistItem);
            return CreatedAtAction(nameof(GetWishlistItems), new { userId = userId }, addedItem.ConvertToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("user/{userId:int}/items/{productId:int}")]
    public async Task<ActionResult<bool>> RemoveFromWishlist(int userId, int productId)
    {
        try
        {
            var defaultWishlist = await _wishlistRepository.GetDefaultWishlistByUserId(userId);
            if (defaultWishlist == null)
            {
                return Ok(false);
            }

            var result = await _wishlistRepository.RemoveFromWishlistByProductId(defaultWishlist.Id, productId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("user/{userId:int}/items/{productId:int}/exists")]
    public async Task<ActionResult<bool>> IsInWishlist(int userId, int productId)
    {
        try
        {
            var defaultWishlist = await _wishlistRepository.GetDefaultWishlistByUserId(userId);
            if (defaultWishlist == null)
            {
                return Ok(false);
            }

            var exists = await _wishlistRepository.ProductExistsInWishlist(defaultWishlist.Id, productId);
            return Ok(exists);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("user/{userId:int}/count")]
    public async Task<ActionResult<int>> GetWishlistItemsCount(int userId)
    {
        try
        {
            var defaultWishlist = await _wishlistRepository.GetDefaultWishlistByUserId(userId);
            if (defaultWishlist == null)
            {
                return Ok(0);
            }

            var count = await _wishlistRepository.GetWishlistItemsCount(defaultWishlist.Id);
            return Ok(count);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("user/{userId:int}/clear")]
    public async Task<ActionResult<bool>> ClearWishlist(int userId)
    {
        try
        {
            var defaultWishlist = await _wishlistRepository.GetDefaultWishlistByUserId(userId);
            if (defaultWishlist == null)
            {
                return Ok(true); 
            }

            var result = await _wishlistRepository.ClearWishlist(defaultWishlist.Id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("user/{userId:int}/items/{productId:int}")]
    public async Task<ActionResult<WishListItemDto>> GetWishlistItemByProduct(int userId, int productId)
    {
        try
        {
            var defaultWishlist = await _wishlistRepository.GetDefaultWishlistByUserId(userId);
            if (defaultWishlist == null)
            {
                return NotFound("Wishlist not found");
            }

            var item = await _wishlistRepository.GetWishlistItemByProductId(defaultWishlist.Id, productId);
            if (item == null)
            {
                return NotFound("Product not found in wishlist");
            }

            return Ok(item.ConvertToDto());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}