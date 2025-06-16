namespace ShopOnline.Web.Services.Contracts.BaseContracts;

/// <summary>
/// Provides common functionality for managing collections of items
/// </summary>
/// <typeparam name="TItem">The type of items in the collection</typeparam>
/// <typeparam name="TKey">The type of the key used to identify items (usually int for ProductId)</typeparam>
public interface IBaseCollectionService<TItem, TKey> where TItem : class
{
    /// <summary>
    /// Event fired when the collection changes (items added, removed, or collection cleared)
    /// </summary>
    event Action<int> OnCollectionChanged;

    /// <summary>
    /// Gets all items in the collection
    /// </summary>
    /// <returns>Collection of items</returns>
    Task<IEnumerable<TItem>> GetItems();

    /// <summary>
    /// Gets the total count of items in the collection
    /// </summary>
    /// <returns>Number of items</returns>
    Task<int> GetItemsCount();

    /// <summary>
    /// Adds an item to the collection
    /// </summary>
    /// <param name="key">The identifier for the item (e.g., ProductId)</param>
    /// <returns>The added item or existing item if already present</returns>
    Task<TItem> AddItem(TKey key);

    /// <summary>
    /// Removes an item from the collection
    /// </summary>
    /// <param name="key">The identifier for the item to remove</param>
    /// <returns>True if item was removed, false if item was not found</returns>
    Task<bool> RemoveItem(TKey key);

    /// <summary>
    /// Checks if an item exists in the collection
    /// </summary>
    /// <param name="key">The identifier for the item</param>
    /// <returns>True if item exists, false otherwise</returns>
    Task<bool> ContainsItem(TKey key);

    /// <summary>
    /// Clears all items from the collection
    /// </summary>
    Task ClearCollection();
}
