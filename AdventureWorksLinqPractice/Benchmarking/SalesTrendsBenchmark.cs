using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;

namespace AdventureWorksLinqPractice.Benchmarking
{
    public class SalesTrendsBenchmark
    {
        private AdventureWorksContext _context;

        [GlobalSetup]
        public void Setup()
        {
            _context = new AdventureWorksContext();
        }

        [Benchmark]
        public void QuerySyntaxLINQ()
        {
            var query = from sales in _context.SalesOrderHeaders
                        group sales by new
                        {
                            Year = sales.OrderDate.Year,
                            Month = sales.OrderDate.Month
                        } into g
                        orderby g.Key.Year, g.Key.Month
                        select new
                        {
                            g.Key.Year,
                            g.Key.Month,
                            TotalSales = g.Sum(s => s.TotalDue)
                        };

            foreach (var result in query) { }
        }

        [Benchmark]
        public void MethodSyntaxLINQ()
        {
            var query = _context.SalesOrderHeaders
                .GroupBy(sales => new { Year = sales.OrderDate.Year, Month = sales.OrderDate.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalSales = g.Sum(s => s.TotalDue)
                });

            foreach (var result in query) { }
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("************* Running Sales Benchmarks *************");
            Console.WriteLine("*****************************************************");

            BenchmarkRunner.Run<SalesTrendsBenchmark>();

            Console.WriteLine("*****************************************************");
            Console.WriteLine("************* Benchmarks Completed *****************");
            Console.WriteLine("*****************************************************");
        }
    }
}
