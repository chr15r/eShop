using eShop.CoreBusiness.Models;
using eShop.DataStore.SQL.Dapper.Helpers;
using eShop.UseCases.PluginInterfaces.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eShop.DataStore.SQL.Dapper
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDataAccess dataAccess;

        public OrderRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public int CreateOrder(Order order)
        {
            // Create Order
            var sqlOrder = @"INSERT INTO [dbo].[Order]
                        ([DatePlaced], [DateProcessing], [DateProcessed], 
                         [CustomerName], [CustomerAddress], [CustomerCity], [CustomerStateProvince], [CustomerCountry], 
                         [AdminUser], [UniqueId])  
                        OUTPUT INSERTED.OrderId 
                        VALUES 
                        (@DatePlaced, @DateProcessing, @DateProcessed, @CustomerName, @CustomerAddress, @CustomerCity, 
                        @CustomerStateProvince, @CustomerCountry, @AdminUser, @UniqueId)";

            var orderId = dataAccess.QuerySingle<int, Order>(sqlOrder, order);

            var sqlLineItem = @"INSERT INTO [dbo].[OrderLineItem] 
                               ([ProductId], [OrderId], [Quantity], [Price]) 
                               VALUES 
                               (@ProductId, @OrderId, @Quantity, @Price)";

            // Create line items
            order.LineItems.ForEach(x => x.OrderId = orderId);
            dataAccess.ExecuteCommand(sqlLineItem, order.LineItems);

            return orderId;

        }

        public IEnumerable<OrderLineItem> GetLineItemsByOrderId(int orderId)
        {
            var sql = "SELECT * FROM OrderLineItem WHERE OrderId = @OrderId";
            var lineItems = dataAccess.Query<OrderLineItem, dynamic>(sql, new { OrderId = orderId });

            var sqlProduct = "SELECT * FROM Product WHERE ProductId = @ProductId";
            lineItems.ForEach(x => x.Product = dataAccess.QuerySingle<Product, dynamic>(sqlProduct, new { ProductId = x.Productid }));
            return lineItems;
        }

        public Order GetOrder(int Id)
        {
            var sql = "SELECT * FROM [Order] WHERE OrderId = @OrderId";
            var order = dataAccess.QuerySingle<Order, dynamic>(sql, new { OrderId = Id });
            order.LineItems = GetLineItemsByOrderId(order.OrderId.Value).ToList();
            return order;
        }

        public Order GetOrderByUniqueId(string uniqueId)
        {
            var sql = "SELECT * FROM [Order] WHERE UniqueId = @UniqueId";
            var order = dataAccess.QuerySingle<Order, dynamic>(sql, new { UniqueId = uniqueId });
            order.LineItems = GetLineItemsByOrderId(order.OrderId.Value).ToList();
            return order;
        }

        public IEnumerable<Order> GetOrders()
        {
            return dataAccess.Query<Order, dynamic>("SELECT * FROM [Order]", new { });
        }

        public IEnumerable<Order> GetOutstandingOrders()
        {
            var sql = "SELECT * FROM [Order] WHERE DateProcessed is null";
            return dataAccess.Query<Order, dynamic>(sql, new { });
        }

        public IEnumerable<Order> GetProcessedOrders()
        {
            var sql = "SELECT * FROM [Order] WHERE DateProcessed is not null";
            return dataAccess.Query<Order, dynamic>(sql, new { });
        }

        public void UpdateOrder(Order order)
        {

            // Update order
            var sql = @"UPDATE [Order] 
                      SET [DatePlaced] = @DatePlaced, [DateProcessing] = @DateProcessing, [DateProcessed] = @DateProcessed, 
                      [CustomerName] = @CustomerName, [CustomerAddress] = @CustomerAddress, [CustomerCity] = @CustomerCity, 
                      [CustomerstateProvince] = @CustomerStateProvince, [CustomerCountry] = @CustomerCountry, 
                      [AdminUser] = @AdminUser, [UniqueId] = @UniqueId WHERE OrderId = @OrderId";

            dataAccess.ExecuteCommand<Order>(sql, order);

            // Update LineItems
            var sqlLineItems = @"UPDATE [OrderLineItem] SET [ProductId] = @ProductId, [OrderId] = @OrderId, [Quantity] = @Quantity, 
                                [Price] = @Price WHERE LineItemId = @LineItemId";

            dataAccess.ExecuteCommand<List<OrderLineItem>>(sqlLineItems, order.LineItems);

        }
    }
}
