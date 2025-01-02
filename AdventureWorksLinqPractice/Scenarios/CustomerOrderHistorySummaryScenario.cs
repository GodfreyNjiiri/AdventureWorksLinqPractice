// CustomerOrderHistorySummaryScenario.cs

using System;
using System.Diagnostics;
using System.Linq;

namespace AdventureWorksLinqPractice.Scenarios
{
    public static class CustomerOrderHistorySummaryScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("************** Customer Order History Summary ********");
            Console.WriteLine("*****************************************************");

            try
            {
                using var context = new AdventureWorksContext();
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // LINQ query to summarize customer order history
                var query = from customer in context.Customers
                            join person in context.People on customer.PersonId equals person.BusinessEntityId
                            join salesOrder in context.SalesOrderHeaders on customer.CustomerId equals salesOrder.CustomerId
                            where salesOrder.Status == 5 // Assuming status 5 indicates completed orders
                            group salesOrder by new
                            {
                                customer.CustomerId,
                                person.FirstName,
                                person.LastName
                            } into g
                            select new
                            {
                                CustomerName = g.Key.FirstName + " " + g.Key.LastName,
                                
                                TotalOrders = g.Count(),
                                TotalAmountSpent = g.Sum(order => order.TotalDue),
                                AverageOrderValue = g.Average(order => order.TotalDue),
                                LastOrderDate = g.Max(order => order.OrderDate)
                            };

                // Output the results
                foreach (var item in query)
                {
                    Console.WriteLine($"Customer: {item.CustomerName}");
                    
                    Console.WriteLine($"Total Orders Placed: {item.TotalOrders}");
                    Console.WriteLine($"Total Amount Spent: {item.TotalAmountSpent:C}");
                    Console.WriteLine($"Average Order Value: {item.AverageOrderValue:C}");
                    Console.WriteLine($"Date of Last Order: {item.LastOrderDate:yyyy-MM-dd}");
                    Console.WriteLine("------------------------------------------");
                }

                stopwatch.Stop();
                Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");

                Console.WriteLine("*****************************************************");
                Console.WriteLine("*****************************************************");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
