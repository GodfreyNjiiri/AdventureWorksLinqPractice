namespace AdventureWorksLinqPractice.Scenarios
{
    public static class ProductCategoryHierarchyScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("******** Product Category Hierarchy ***************");
            Console.WriteLine("*****************************************************");

            using var context = new AdventureWorksContext();

            // LINQ query to explore the hierarchy of categories and subcategories
            var query = from category in context.ProductCategories
                        join subcategory in context.ProductSubcategories on category.ProductCategoryId equals subcategory.ProductCategoryId
                        join product in context.Products on subcategory.ProductSubcategoryId equals product.ProductSubcategoryId into productsGroup
                        group new { category, subcategory, productsGroup } by category.Name into g
                        select new
                        {
                            CategoryName = g.Key,
                            Subcategories = g.Select(sub => new
                            {
                                SubcategoryName = sub.subcategory.Name,
                                ProductCount = sub.productsGroup.Count()
                            })
                        };

            // Display the hierarchy in an indented format
            foreach (var category in query)
            {
                Console.WriteLine(category.CategoryName);
                foreach (var subcategory in category.Subcategories)
                {
                    Console.WriteLine($"  - {subcategory.SubcategoryName}: {subcategory.ProductCount} products");
                }
                Console.WriteLine();
            }

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
