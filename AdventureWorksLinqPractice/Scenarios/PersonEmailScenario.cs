// PersonEmailScenario.cs

namespace AdventureWorksLinqPractice.Scenarios
{
    public static class PersonEmailScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            using var context = new AdventureWorksContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var query = from p in context.People
                        join e in context.EmailAddresses on p.BusinessEntityId equals e.BusinessEntityId
                        select new { p.FirstName, p.LastName, e.EmailAddress1 };

            foreach (var item in query)
            {
                Console.WriteLine($"{item.FirstName} {item.LastName}: {item.EmailAddress1}");
            }

            stopwatch.Stop();
            Console.WriteLine($"Execution Time: {stopwatch.Elapsed} ms");

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
