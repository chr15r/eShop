using eShop.CoreBusiness.Models;
using eShop.UseCases.PluginInterfaces.StateStore;
using eShop.UseCases.PluginInterfaces.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.UseCases.ShoppingCartScreen
{
    public class DeleteProductUseCase : IDeleteProductUseCase
    {
        private readonly IShoppingCart shoppingCart;
        private readonly IShoppingCartStateStore shoppingCartStareStore;

        public DeleteProductUseCase(IShoppingCart _shoppingCart, IShoppingCartStateStore _shoppingCartStareStore)
        {
            shoppingCart = _shoppingCart;
            shoppingCartStareStore = _shoppingCartStareStore;
        }

        public async Task<Order> Execute(int productId)
        {
            var order = await this.shoppingCart.DeleteProductAsync(productId);
            this.shoppingCartStareStore.UpdateLineItemsCount();
            return order;

        }
    }
}
