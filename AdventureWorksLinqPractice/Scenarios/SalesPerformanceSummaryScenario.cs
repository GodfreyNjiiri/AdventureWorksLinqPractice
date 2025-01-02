using System;
using System.Diagnostics;
using System.Linq;

namespace AdventureWorksLinqPractice.Scenarios
{
    public static class SalesPerformanceSummaryScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("************** Sales Performance Summary ************");
            Console.WriteLine("*****************************************************");

            using var context = new AdventureWorksContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // LINQ query to summarize sales performance
            var query = from salesPerson in context.SalesPeople
                        join person in context.People on salesPerson.BusinessEntityId equals person.BusinessEntityId
                        join salesOrder in context.SalesOrderHeaders on salesPerson.BusinessEntityId equals salesOrder.SalesPersonId
                        join territory in context.SalesTerritories on salesPerson.TerritoryId equals territory.TerritoryId
                        where salesOrder.Status == 5 // Filter only completed orders
                        group new { salesPerson, person, salesOrder, territory } by new
                        {
                            salesPerson.BusinessEntityId,
                            person.FirstName,
                            person.LastName,
                            TerritoryName = territory.Name
                        } into g
                        select new
                        {
                            SalesPersonName = g.Key.FirstName + " " + g.Key.LastName,
                            g.Key.TerritoryName,
                            TotalSales = g.Sum(x => x.salesOrder.SubTotal), // Summing the subtotal of orders
                            OrderCount = g.Count(),
                            HighestSale = g.Max(x => x.salesOrder.SubTotal), // Largest single order handled
                            LastOrderDate = g.Max(x => x.salesOrder.OrderDate) // Most recent completed order
                        };

            // Output the results
            foreach (var item in query)
            {
                Console.WriteLine($"Salesperson: {item.SalesPersonName}");
                Console.WriteLine($"Territory: {item.TerritoryName}");
                Console.WriteLine($"Total Sales: {item.TotalSales:C}");
                Console.WriteLine($"Number of Orders: {item.OrderCount}");
                Console.WriteLine($"Highest Sale: {item.HighestSale:C}");
                Console.WriteLine($"Last Order Date: {item.LastOrderDate:yyyy-MM-dd}");
                Console.WriteLine("------------------------------------------");
            }

            stopwatch.Stop();
            Console.WriteLine($"Execution Time: {stopwatch.Elapsed} ms");

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
