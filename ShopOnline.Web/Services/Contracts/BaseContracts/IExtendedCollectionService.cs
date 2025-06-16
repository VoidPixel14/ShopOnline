namespace ShopOnline.Web.Services.Contracts.BaseContracts;

/// <summary>
/// Extended base interface for services that support item manipulation and cross-service operations
/// </summary>
/// <typeparam name="TItem">The type of items in the collection</typeparam>
/// <typeparam name="TKey">The type of the key used to identify items</typeparam>
public interface IExtendedCollectionService<TItem, TKey> : IBaseCollectionService<TItem, TKey> where TItem : class
{
    /// <summary>
    /// Updates the quantity or properties of an existing item
    /// </summary>
    /// <param name="key">The identifier for the item</param>
    /// <param name="updateAction">Action to perform the update</param>
    /// <returns>The updated item, or null if item not found</returns>
    Task<TItem?> UpdateItem(TKey key, Func<TItem, TItem> updateAction);

    /// <summary>
    /// Gets a specific item by its key
    /// </summary>
    /// <param name="key">The identifier for the item</param>
    /// <returns>The item if found, null otherwise</returns>
    Task<TItem?> GetItem(TKey key);

    /// <summary>
    /// Transfers an item to another collection service
    /// </summary>
    /// <param name="key">The identifier for the item to transfer</param>
    /// <param name="targetService">The target collection service</param>
    /// <returns>True if transfer was successful, false otherwise</returns>
    Task<bool> TransferItemTo<TTargetItem>(TKey key, IBaseCollectionService<TTargetItem, TKey> targetService) where TTargetItem : class;
}