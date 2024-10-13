namespace AdventureWorksLinqPractice.Scenarios
{
    public static class HighestDiscountOrderPerCustomerScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("********* Highest Discount Order Per Customer *********");
            Console.WriteLine("*****************************************************");
            using var context = new AdventureWorksContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Query to find the highest discount order per customer
            var query = from soh in context.SalesOrderHeaders
                        join sod in context.SalesOrderDetails on soh.SalesOrderId equals sod.SalesOrderId
                        group new { soh, sod } by soh.CustomerId into g
                        select new
                        {
                            CustomerID = g.Key,
                            SalesOrderID = g.OrderByDescending(x => x.sod.UnitPriceDiscount).FirstOrDefault().soh.SalesOrderId,
                            MaxDiscount = g.Max(x => x.sod.UnitPriceDiscount),
                            TotalDue = g.OrderByDescending(x => x.sod.UnitPriceDiscount).FirstOrDefault().soh.TotalDue
                        };

            foreach (var item in query)
            {
                Console.WriteLine($"Customer ID: {item.CustomerID}, Sales Order ID: {item.SalesOrderID}, Max Discount: {item.MaxDiscount:P}, Total Due: {item.TotalDue:C}");
            }

            stopwatch.Stop();
            Console.WriteLine($"HighestDiscountOrderPerCustomerScenario Execution Time: {stopwatch.Elapsed} ms");
            Console.WriteLine("*****************************************************");
        }
    }
}
