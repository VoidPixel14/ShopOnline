namespace ShopOnline.Models.Base;

/// <summary>
/// Base interface for items that can be stored in collections
/// Provides common properties that most collection items should have
/// </summary>
public interface ICollectionItem
{
    int Id { get; set; }

    int ProductId { get; set; }

    DateTime DateAdded { get; set; }
}