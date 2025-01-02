using System;
using System.Diagnostics;
using System.Linq;

namespace AdventureWorksLinqPractice.Scenarios
{
    public static class CustomerOrderHistorySummaryScenarioPagination
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

                // User input for pagination and sorting
                Console.Write("Enter page number: ");
                int pageNumber = int.TryParse(Console.ReadLine(), out var pn) ? pn : 1;

                Console.Write("Enter page size: ");
                int pageSize = int.TryParse(Console.ReadLine(), out var ps) ? ps : 10;

                Console.Write("Sort by (TotalAmountSpent, TotalOrders, LastOrderDate): ");
                string sortBy = Console.ReadLine()?.Trim() ?? "TotalAmountSpent";

                Console.Write("Sort direction (asc/desc): ");
                string sortDirection = Console.ReadLine()?.Trim().ToLower() ?? "desc";

                // LINQ query to summarize customer order history
                var query = from customer in context.Customers
                            join person in context.People on customer.PersonId equals person.BusinessEntityId
                            join salesOrder in context.SalesOrderHeaders on customer.CustomerId equals salesOrder.CustomerId
                            where salesOrder.Status == 5 // Completed orders
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

                // Apply sorting
                query = sortBy switch
                {
                    "TotalAmountSpent" => sortDirection == "asc" ? query.OrderBy(q => q.TotalAmountSpent) : query.OrderByDescending(q => q.TotalAmountSpent),
                    "TotalOrders" => sortDirection == "asc" ? query.OrderBy(q => q.TotalOrders) : query.OrderByDescending(q => q.TotalOrders),
                    "LastOrderDate" => sortDirection == "asc" ? query.OrderBy(q => q.LastOrderDate) : query.OrderByDescending(q => q.LastOrderDate),
                    _ => query.OrderByDescending(q => q.TotalAmountSpent)
                };

                // Apply pagination
                var pagedResult = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Output the results
                foreach (var item in pagedResult)
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
                Console.WriteLine($"Page {pageNumber} of {Math.Ceiling((double)query.Count() / pageSize)}");
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
