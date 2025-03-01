using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksLinqPractice.Scenarios
{
    public static class BusinessLogicScenario
    {
        public static async Task RunAsync()
        {
            using var context = new AdventureWorksContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Fetch Customers
            var customers = await context.Customers.Take(20).ToListAsync();
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Customers:");
            Console.WriteLine("-----------------------------------------------------");
            foreach (var customer in customers)
            {
                Console.WriteLine($"Customer ID: {customer.CustomerId}, Account Number: {customer.AccountNumber}, Territory ID: {customer.TerritoryId}");
            }

            stopwatch.Stop();
            Console.WriteLine($"Fetching Customers took: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();

            stopwatch.Start();
            // Fetch Vendors
            var vendors = await context.Vendors.Take(20).ToListAsync();
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Vendors:");
            Console.WriteLine("-----------------------------------------------------");
            foreach (var vendor in vendors)
            {
                Console.WriteLine($"Vendor ID: {vendor.BusinessEntityId}, Name: {vendor.Name}, Account Number: {vendor.AccountNumber}");
            }

            stopwatch.Stop();
            Console.WriteLine($"Fetching Vendors took: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();

            stopwatch.Start();
            // Fetch Sales Orders
            var salesOrders = await context.SalesOrderHeaders.OrderBy(o => o.OrderDate).Take(20).ToListAsync();
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Sales Orders:");
            Console.WriteLine("-----------------------------------------------------");
            foreach (var order in salesOrders)
            {
                Console.WriteLine($"Sales Order ID: {order.SalesOrderId}, Order Date: {order.OrderDate}, Total Due: {order.TotalDue}");
            }

            stopwatch.Stop();
            Console.WriteLine($"Fetching Sales Orders took: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();

            stopwatch.Start();
            // Fetch Products
            var products = await context.Products.OrderBy(p => p.Name).Take(20).ToListAsync();
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Products:");
            Console.WriteLine("-----------------------------------------------------");
            foreach (var product in products)
            {
                Console.WriteLine($"Product ID: {product.ProductId}, Name: {product.Name}, List Price: {product.ListPrice}");
            }

            stopwatch.Stop();
            Console.WriteLine($"Fetching Products took: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
