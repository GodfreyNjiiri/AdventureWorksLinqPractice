namespace AdventureWorksLinqPractice.Scenarios
{
    public static class ProductCategoryBarChartScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("************ Products Per Category Chart ************");
            Console.WriteLine("*****************************************************");
            using var context = new AdventureWorksContext();

            // LINQ query to group products by category
            var query = from product in context.Products
                        join subcategory in context.ProductSubcategories on product.ProductSubcategoryId equals subcategory.ProductSubcategoryId
                        join category in context.ProductCategories on subcategory.ProductCategoryId equals category.ProductCategoryId
                        group product by category.Name into g
                        select new
                        {
                            CategoryName = g.Key,
                            ProductCount = g.Count()
                        };

            // Sort categories by the number of products (descending order)
            var results = query.OrderByDescending(q => q.ProductCount).ToList();

            // ASCII bar chart representation
            var maxCount = results.Max(r => r.ProductCount); // Find the highest count for scaling
            Console.WriteLine("Product Categories and Count (ASCII Chart):");
            foreach (var result in results)
            {
                var bar = new string('#', (int)(result.ProductCount * 50 / maxCount)); // Scale to 50 characters max
                Console.WriteLine($"{result.CategoryName.PadRight(20)} | {bar} ({result.ProductCount})");
            }

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
