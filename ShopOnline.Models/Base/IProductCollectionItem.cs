namespace ShopOnline.Models.Base;

public interface IProductCollectionItem : ICollectionItem
{
    string ProductName { get; set; }
    string ProductDescription { get; set; }
    string ProductImageUrl { get; set; }
    decimal Price { get; set; }
    string CategoryName { get; set; }
}