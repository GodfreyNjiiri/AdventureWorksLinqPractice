namespace AdventureWorksLinqPractice.Scenarios
{
    public static class SupplierActivityScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("************ Supplier Activity Analysis ************");
            Console.WriteLine("*****************************************************");

            using var context = new AdventureWorksContext();

            // LINQ query to count products per vendor
            var query = from productVendor in context.ProductVendors
                        join product in context.Products on productVendor.ProductId equals product.ProductId
                        join vendor in context.Vendors on productVendor.BusinessEntityId equals vendor.BusinessEntityId
                        group product by vendor.Name into g
                        select new
                        {
                            VendorName = g.Key,
                            ProductCount = g.Count()
                        };

            // Order vendors by the number of products supplied (descending)
            var results = query.OrderByDescending(q => q.ProductCount).Take(10).ToList(); // Top 10 vendors

            // ASCII Bar Chart
            var maxCount = results.Max(r => r.ProductCount); // Scale to the max product count
            Console.WriteLine("Top 10 Suppliers by Product Count (ASCII Chart):");
            foreach (var result in results)
            {
                var bar = new string('#', (int)(result.ProductCount * 50 / maxCount)); // Scale to 50 characters
                Console.WriteLine($"{result.VendorName.PadRight(25)} | {bar} ({result.ProductCount})");
            }

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
