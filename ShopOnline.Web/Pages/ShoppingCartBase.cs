﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Globalization;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase:ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        
        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string? ErrorMessage { get; set; }
        protected string? TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);

            RemoveCartItem(id);
            CartChanged();
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = id,
                        Qty = qty
                    };
                    var returnUpdateItemDto = await this.ShoppingCartService.UpdateQty(updateItemDto);

                    UpdateItemTotalPrice(returnUpdateItemDto);

                    CartChanged();

                    await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, false);

                } 
                else
                {
                    var item = this.ShoppingCartItems.FirstOrDefault(i => i.Id == id);
                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task UpdateQty_Input(int id)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, true);
        }


        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }
        }


        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQty();
        }

        private void SetTotalPrice()
        {
            var nairaCulture = new CultureInfo("en-NG");
            nairaCulture.NumberFormat.CurrencySymbol = "₦";
            TotalPrice = this.ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C", nairaCulture);
        }

        private void SetTotalQty()
        {
            TotalQuantity = this.ShoppingCartItems.Sum(p => p.Qty);
        }

        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
        }

        private void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);

            ShoppingCartItems.Remove(cartItemDto);
        }

        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }

    }
}
