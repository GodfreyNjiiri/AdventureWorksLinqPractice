using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksLinqPractice.Scenarios
{
    public static class UnusedModelsScenario
    {
        public static async Task RunAsync()
        {
            using var context = new AdventureWorksContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Fetch AwbuildVersion
            var awbuildVersions = await context.AwbuildVersions.ToListAsync();
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("AW Build Versions:");
            Console.WriteLine("-----------------------------------------------------");
            foreach (var version in awbuildVersions)
            {
                Console.WriteLine($"System Information ID: {version.SystemInformationId}, Database Version: {version.DatabaseVersion}, Version Date: {version.VersionDate}");
            }

            stopwatch.Stop();
            Console.WriteLine($"Fetching AW Build Versions took: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();

            stopwatch.Start();
            // Fetch Database Logs
            var databaseLogs = await context.DatabaseLogs.Take(20).ToListAsync();
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Database Logs:");
            Console.WriteLine("-----------------------------------------------------");
            foreach (var log in databaseLogs)
            {
                Console.WriteLine($"Log ID: {log.DatabaseLogId}, Event: {log.Event}, Date: {log.PostTime}");
            }

            stopwatch.Stop();
            Console.WriteLine($"Fetching Database Logs took: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();

            stopwatch.Start();
            // Fetch Error Logs
            var errorLogs = await context.ErrorLogs.Take(20).ToListAsync();
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Error Logs:");
            Console.WriteLine("-----------------------------------------------------");
            foreach (var error in errorLogs)
            {
                Console.WriteLine($"Error ID: {error.ErrorLogId}, Message: {error.ErrorMessage}, Date: {error.ErrorTime}");
            }

            stopwatch.Stop();
            Console.WriteLine($"Fetching Error Logs took: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
