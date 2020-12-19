using eShop.UseCases.PluginInterfaces.DataStore;
using eShop.UseCases.PluginInterfaces.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.UseCases.ViewProductScreen
{
    public class AddProductToCartUseCase : IAddProductToCartUseCase
    {
        private readonly IShoppingCart shoppingCart;
        private readonly IProductRepository productRepository;

        public AddProductToCartUseCase(IProductRepository _productRepository, IShoppingCart _shoppingCart)
        {
            productRepository = _productRepository;
            shoppingCart = _shoppingCart;
        }


        public async void Execute(int productId)
        {
            var product = productRepository.GetProduct(productId);
            await shoppingCart.AddProductAsync(product);
        }
    }
}
