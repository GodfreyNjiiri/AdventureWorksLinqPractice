namespace AdventureWorksLinqPractice.Scenarios
{
    public static class CustomerOrderHistoryScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("************* Customer Order History ***************");
            Console.WriteLine("*****************************************************");

            using var context = new AdventureWorksContext();

            // LINQ query to analyze customer order history
            var query = from customer in context.Customers
                        join sales in context.SalesOrderHeaders on customer.CustomerId equals sales.CustomerId
                        group sales by new
                        {
                            customer.CustomerId,
                            customer.Person.FirstName,
                            customer.Person.LastName
                        } into g
                        select new
                        {
                            CustomerName = g.Key.FirstName + " " + g.Key.LastName,
                            TotalOrders = g.Count(),
                            TotalSpent = g.Sum(s => s.TotalDue),
                            AverageOrderValue = g.Average(s => s.TotalDue),
                            MaxOrderValue = g.Max(s => s.TotalDue)
                        };

            // Display the results
            Console.WriteLine($"{"Customer",-25} {"Orders",-10} {"Total Spent",-15} {"Avg Order",-15} {"Max Order",-15}");
            Console.WriteLine(new string('-', 80));

            foreach (var result in query.OrderByDescending(r => r.TotalSpent).Take(10)) // Top 10 by total spending
            {
                Console.WriteLine($"{result.CustomerName,-25} {result.TotalOrders,-10} {result.TotalSpent,15:C} {result.AverageOrderValue,15:C} {result.MaxOrderValue,15:C}");
            }

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
