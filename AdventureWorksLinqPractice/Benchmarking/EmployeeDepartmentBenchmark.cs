using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Linq;

namespace AdventureWorksLinqPractice.Benchmarking
{
    public class EmployeeDepartmentBenchmark
    {
        private AdventureWorksContext _context;

        [GlobalSetup]
        public void Setup()
        {
            _context = new AdventureWorksContext();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [Benchmark]
        public object QuerySyntaxLINQ()
        {
            var result = from edh in _context.EmployeeDepartmentHistories
                         join d in _context.Departments on edh.DepartmentId equals d.DepartmentId
                         group edh by d.Name into g
                         orderby g.Key
                         select new
                         {
                             Department = g.Key,
                             EmployeeCount = g.Count()
                         };

            return result.ToList();
        }

        [Benchmark]
        public object MethodSyntaxLINQ()
        {
            var result = _context.EmployeeDepartmentHistories
                .Join(_context.Departments,
                    edh => edh.DepartmentId,
                    d => d.DepartmentId,
                    (edh, d) => new { EmployeeDepartmentHistory = edh, Department = d })
                .GroupBy(x => x.Department.Name)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Department = g.Key,
                    EmployeeCount = g.Count()
                });

            return result.ToList();
        }

        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("************* Running Sales Benchmarks *************");
            Console.WriteLine("*****************************************************");

            BenchmarkRunner.Run<EmployeeDepartmentBenchmark>();

            Console.WriteLine("*****************************************************");
            Console.WriteLine("************* Benchmarks Completed *****************");
            Console.WriteLine("*****************************************************");
        }
    }
}
