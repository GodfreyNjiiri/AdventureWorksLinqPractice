using AdventureWorksLinqPractice.Scenarios;

PersonEmailScenario.Run();

CustomerOrderQuantityScenario.Run();

var concurrentDashboardQueries = new ConcurrentDashboardQueries();
await concurrentDashboardQueries.RunAsync();

EmployeeLeaveSummaryScenario.Run();
