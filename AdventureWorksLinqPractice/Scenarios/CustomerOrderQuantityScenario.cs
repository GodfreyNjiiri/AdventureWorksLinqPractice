// CustomerOrderQuantityScenario.cs
using System;
using System.Diagnostics;
using System.Linq;
using AdventureWorksLinqPractice.Data;
using AdventureWorksLinqPractice.Models;

namespace AdventureWorksLinqPractice.Scenarios
{
    public static class CustomerOrderQuantityScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            using (var context = new AdventureWorksContext())
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var query = from c in context.Customers
                            join soh in context.SalesOrderHeaders on c.CustomerId equals soh.CustomerId
                            join sod in context.SalesOrderDetails on soh.SalesOrderId equals sod.SalesOrderId
                            group sod by c.CustomerId into g
                            select new { CustomerID = g.Key, TotalQuantity = g.Sum(sod => sod.OrderQty) };

                foreach (var item in query)
                {
                    Console.WriteLine($"Customer ID: {item.CustomerID}, Total Quantity: {item.TotalQuantity}");
                }

                stopwatch.Stop();
                Console.WriteLine($"Execution Time: {stopwatch.Elapsed} ms");

                Console.WriteLine("*****************************************************");
                Console.WriteLine("*****************************************************");
                Console.WriteLine("*****************************************************");
            }
        }
    }
}
