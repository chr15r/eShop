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
            var conenctionString = "Data Source=LAPTOP-9CBKDG1T\\SQLEXPRESS;Initial Catalog=eShop;Integrated Security=True";
            using (IDbConnection connection = new SqlConnection(conenctionString))
            {
                var result = connection.Query<Product>("SELECT * FROM Product");
                foreach (var record in result)
                {
                    Console.WriteLine($"{record.Name}: {record.Price}");
                }
            }
        }
    }
}
