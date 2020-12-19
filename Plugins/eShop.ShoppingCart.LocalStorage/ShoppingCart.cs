using eShop.CoreBusiness.Models;
using eShop.UseCases.PluginInterfaces.DataStore;
using eShop.UseCases.PluginInterfaces.UI;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.ShoppingCart.LocalStorage
{
    public class ShoppingCart : IShoppingCart
    {
        private readonly IJSRuntime jsRuntime;
        public const string cstrShoppingCart = "eShop.ShoppingCart";
        private readonly IProductRepository productRepository;

        // Inject Javascript runtime for localstorage
        public ShoppingCart(IJSRuntime jsRuntime, IProductRepository _productRepository)
        {
            this.jsRuntime = jsRuntime;
            productRepository = _productRepository;
        }        

        public async Task<Order> AddProductAsync(Product product)
        {
            var order = await GetOrder();
            order.AddProduct(product.ProductId, 1, product.Price);
            await SetOrder(order);
            return order;
        }

        public async Task<Order> DeleteProductAsync(int productId)
        {
            var order = await GetOrder();
            order.RemoveProduct(productId);
            await SetOrder(order);
            return order;

        }

        public Task EmptyAsync()
        {
            return this.SetOrder(null);
        }

        public async Task<Order> GetOrderAsync()
        {
            return await GetOrder();
        }

        public Task<Order> PlaceOrderAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            await this.SetOrder(order);
            return order;
        }

        public async Task<Order> UpdateQuantityAsync(int productId, int quantity)
        {
            var order = await GetOrder();
            if (quantity < 0)
                return order;

            else if (quantity == 0)
                return await DeleteProductAsync(productId);

            var lineItem = order.LineItems.SingleOrDefault(x => x.Productid == productId);
            if (lineItem != null) lineItem.Quantity = quantity;
            await SetOrder(order);
            return order;

        }

        /// <summary>
        /// Uses javascript Interop to get order from localstorage
        /// </summary>
        /// <returns></returns>
        private async Task<Order> GetOrder()
        {
            Order order = null;
            var strOrder = await jsRuntime.InvokeAsync<string>("localStorage.getItem", cstrShoppingCart);
            if (!string.IsNullOrWhiteSpace(strOrder) && strOrder.ToLower() != "null")
                order = JsonConvert.DeserializeObject<Order>(strOrder);
            else
            {
                order = new Order();
                await SetOrder(order);
            }

            foreach (var item in order.LineItems)
            {
                item.Product = productRepository.GetProduct(item.Productid);
            }

            return order;
            
        }

        /// <summary>
        /// Uses javascript Interop to set order in localstorage
        /// </summary>
        /// <returns></returns>
        private async Task SetOrder(Order order)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", cstrShoppingCart, JsonConvert.SerializeObject(order));
        }
    }
}
