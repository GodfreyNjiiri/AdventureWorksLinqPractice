using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AdventureWorksLinqPractice.Scenarios
{
    public class ConcurrentDashboardQueriesV2
    {
        public static async Task RunAsync()
        {
            // Create the context locally in a using block to ensure disposal
            using var context = new AdventureWorksContext();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start all queries concurrently
            Task<List<Person>> personsTask = context.People.Take(20).ToListAsync();
            Task<List<SalesOrderHeader>> salesOrdersTask = context.SalesOrderHeaders
                                                                  .OrderBy(o => o.OrderDate)
                                                                  .Take(20)
                                                                  .ToListAsync();
            Task<List<Product>> productsTask = context.Products
                                                       .OrderBy(p => p.Name)
                                                       .Take(20)
                                                       .ToListAsync();

            // Await all tasks concurrently
            await Task.WhenAll(personsTask, salesOrdersTask, productsTask);

            stopwatch.Stop();
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine($"Concurrent queries completed in: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("-----------------------------------------------------");

            // Display Persons
            var persons = personsTask.Result;
            Console.WriteLine("Persons:");
            Console.WriteLine("{0,-15} {1,-15}", "First Name", "Last Name");
            foreach (var person in persons)
            {
                Console.WriteLine("{0,-15} {1,-15}", person.FirstName, person.LastName);
            }

            Console.WriteLine("-----------------------------------------------------");

            // Display Sales Orders
            var salesOrders = salesOrdersTask.Result;
            Console.WriteLine("Sales Orders:");
            Console.WriteLine("{0,-15} {1,-25}", "Sales Order ID", "Order Date");
            foreach (var order in salesOrders)
            {
                Console.WriteLine("{0,-15} {1,-25}", order.SalesOrderId, order.OrderDate);
            }

            Console.WriteLine("-----------------------------------------------------");

            // Display Products
            var products = productsTask.Result;
            Console.WriteLine("Products:");
            Console.WriteLine("{0,-15} {1,-25}", "Product ID", "Name");
            foreach (var product in products)
            {
                Console.WriteLine("{0,-15} {1,-25}", product.ProductId, product.Name);
            }
        }
    }
}
