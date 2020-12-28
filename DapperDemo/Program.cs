using Dapper;
using eShop.CoreBusiness.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DapperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Data Source=LAPTOP-9CBKDG1T\\SQLEXPRESS;Initial Catalog=eShop;Integrated Security=True";
            var da = new DataAccess(connectionString);

            var products1 = da.Query<Product, dynamic>("SELECT * FROM Product", new { });

            var products2 = da.QuerySingle<Product, dynamic>("SELECT * FROM Product WHERE Name = @Name", new { Name = "Maybelline Fit Me Blush" });

            da.ExecuteCommand<Product>(@"INSERT INTO Product (ProductId, Brand, Name, Price)  
                                            VALUES (@ProductId, @Brand, @Name, @Price)",
                                 new Product { ProductId = 1000000055, Brand = "New Brand", Name = "New Name", Price = 10 });


            da.ExecuteCommand<dynamic>(@"UPDATE Product SET ImageLink=@Url WHERE Name=@Name",
                                 new
                                 {
                                     Url = "https://d3t32hsnjxo7q6.cloudfront.net/i/029889b345c76a70e8bb978b73ed1a87_ra,w158,h184_pa,w158,h184.png",
                                     Name = "New Name"
                                 });

            //using (IDbConnection connection = new SqlConnection(connectionString))
            //{

            //    //// CREATE
            //    //var sql = @"INSERT INTO Product (ProductId, Brand, Name, Price)  
            //    //            VALUES (@ProductId, @Brand, @Name, @Price)";

            //    //var product = new Product { ProductId = 100000002, Brand = "New Brand", Name = "New Name", Price = 10 };
            //    //connection.Execute(sql, product);


            //    // UPDATE
            //    //var sqlUpdate = @"UPDATE Product 
            //    //                    SET ImageLink=@Url 
            //    //                    WHERE Name=@Name";

            //    //connection.Execute(sqlUpdate, new
            //    //{
            //    //    Url = "https://d3t32hsnjxo7q6.cloudfront.net/i/029889b345c76a70e8bb978b73ed1a87_ra,w158,h184_pa,w158,h184.png",
            //    //    Name = "New Name"
            //    //});


            //    // DELETE
            //    var sqlDelete = "DELETE FROM Product WHERE Name = @Name";
            //    connection.Execute(sqlDelete, new { Name = "New Name" });


            //    // READ
            //        var result = connection.Query<Product>("SELECT * FROM Product");
            //    foreach (var record in result)
            //    {
            //        Console.WriteLine($"{record.Name}: {record.Price}");
            //    }


            //}
        }
    }
}
