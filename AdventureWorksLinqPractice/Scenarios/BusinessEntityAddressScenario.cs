// BusinessEntityAddressScenario.cs

namespace AdventureWorksLinqPractice.Scenarios
{
    public static class BusinessEntityAddressScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            using var context = new AdventureWorksContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // LINQ query to join BusinessEntity and BusinessEntityAddress
            var query = from be in context.BusinessEntities
                        join bea in context.BusinessEntityAddresses on be.BusinessEntityId equals bea.BusinessEntityId
                        join addr in context.Addresses on bea.AddressId equals addr.AddressId
                        join addrType in context.AddressTypes on bea.AddressTypeId equals addrType.AddressTypeId
                        select new
                        {
                            be.BusinessEntityId,
                            Address = addr.AddressLine1 + ", " + addr.City + ", " + addr.StateProvince,
                            AddressType = addrType.Name,
                            ModifiedDate = bea.ModifiedDate
                        };

            // Output the results
            foreach (var item in query)
            {
                Console.WriteLine($"Business Entity ID: {item.BusinessEntityId}");
                Console.WriteLine($"Address: {item.Address}");
                Console.WriteLine($"Address Type: {item.AddressType}");
                Console.WriteLine($"Modified Date: {item.ModifiedDate}");
                Console.WriteLine("------------------------------------------");
            }

            stopwatch.Stop();
            Console.WriteLine($"Execution Time: {stopwatch.Elapsed} ms");

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
