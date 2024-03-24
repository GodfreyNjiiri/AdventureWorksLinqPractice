using Microsoft.EntityFrameworkCore;

namespace AdventureWorksLinqPractice.Scenarios
{
    public class ConcurrentDashboardQueries
    {
        private readonly AdventureWorksContext _context = new();

        public async Task RunAsync()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var persons = await _context.People.Take(20).ToListAsync();
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Persons:");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("{0,-15} {1,-15}", "First Name", "Last Name");
            foreach (var person in persons)
            {
                Console.WriteLine("{0,-15} {1,-15}", person.FirstName, person.LastName);
            }

            stopwatch.Stop();
            Console.WriteLine($"Fetching Persons took: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();

            stopwatch.Start();
            var salesOrders = await _context.SalesOrderHeaders.OrderBy(o => o.OrderDate).Take(20).ToListAsync();
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Sales Orders:");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("{0,-15} {1,-25}", "Sales Order ID", "Order Date");
            foreach (var order in salesOrders)
            {
                Console.WriteLine("{0,-15} {1,-25}", order.SalesOrderId, order.OrderDate);
            }

            stopwatch.Stop();
            Console.WriteLine($"Fetching Sales Orders took: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();

            stopwatch.Start();
            var products = await _context.Products.OrderBy(p => p.Name).Take(20).ToListAsync();
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Products:");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("{0,-15} {1,-25}", "Product ID", "Name");
            foreach (var product in products)
            {
                Console.WriteLine("{0,-15} {1,-25}", product.ProductId, product.Name);
            }

            stopwatch.Stop();
            Console.WriteLine($"Fetching Products took: {stopwatch.ElapsedMilliseconds} ms");
        }

    }
}
