using ShopOnline.Models.Base;

namespace ShopOnline.Web.Services.Contracts.BaseContracts;

public interface IQuantifiableItem : IProductCollectionItem
{
    int Qty { get; set; }
    decimal TotalPrice => Price * Qty;
}