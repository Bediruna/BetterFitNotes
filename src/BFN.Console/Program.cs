using BFN.Data.Migrations;
using FluentMigrator.Runner;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

var connection = new SqliteConnection("Data Source=WorkoutData.sqlite");

var serviceProvider = CreateServices(connection);
using var scope = serviceProvider.CreateScope();
var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
if (runner.HasMigrationsToApplyUp())
{
    runner.MigrateUp();
}
else
{
    Console.WriteLine("No migrations found.");
}

IServiceProvider CreateServices(SqliteConnection connection)
{
    return new ServiceCollection()
        .AddLogging(lb => lb.AddFluentMigratorConsole())
        .AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddSQLite()
            .WithGlobalConnectionString(connection.ConnectionString)
            .ScanIn(typeof(CreateInitialTablesMigration).Assembly).For.Migrations())
        .BuildServiceProvider();
}
