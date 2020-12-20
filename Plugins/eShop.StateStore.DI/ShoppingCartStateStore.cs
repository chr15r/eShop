﻿using eShop.UseCases.PluginInterfaces.StateStore;
using eShop.UseCases.PluginInterfaces.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.StateStore.DI
{
    public class ShoppingCartStateStore : StateStoreBase, IShoppingCartStateStore
    {
        private readonly IShoppingCart shoppingCart;

        public ShoppingCartStateStore(IShoppingCart _shoppingCart)
        {
            shoppingCart = _shoppingCart;
        }

        public async Task<int> GetItemsCount()
        {
            var order = await shoppingCart.GetOrderAsync();
            if (order != null && order.LineItems != null && order.LineItems.Count > 0)
                return order.LineItems.Count;

            return 0;
        }

        public void UpdateLineItemsCount()
        {
            base.BroadcastStateChange();
        }
    }
}
