using BFN.Data.Migrations;
using BFN.Data.Models;
using BFN.Data.Scripts;
using SQLite;
using Dapper;
using System.Data.SQLite;

namespace BFN.App.Services
{
    public static class DataService
    {
        public static SQLiteAsyncConnection db;
        private static bool isInitialized = false;
        private static string databasePath;

        static DataService()
        {
            try
            {
                //Task.Run(InitializeDatabase).Wait();
                InitializeDatabase();
            }
            catch (Exception ex)
            {
                // Handle or log the exception as required
                //throw new InvalidOperationException("Failed to initialize database", ex);
                Console.WriteLine(ex.ToString());
            }
        }

        private static async Task InitializeDatabase()
        {
            if (!isInitialized)
            {
                try
                {
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BFNData.db");
                    db = new SQLiteAsyncConnection(databasePath);

                    await AddInitialTables();
                    await AddDefaultRecordsIfNeeded();

                    isInitialized = true;
                }
                catch (Exception ex)
                {
                    // Handle or log the exception as required
                    //throw new InvalidOperationException("Database initialization failed", ex);
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private static async Task AddInitialTables()
        {
            var connectionString = $"Data Source={databasePath}";
            using var connection = new System.Data.SQLite.SQLiteConnection(connectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(CreateInitialTableSqlScripts.CreateCategoryTableSql);
            await connection.ExecuteAsync(CreateInitialTableSqlScripts.CreateExerciseTableSql);
            await connection.ExecuteAsync(CreateInitialTableSqlScripts.CreateTrainingLogTableSql);
        }

        public static async Task<List<TrainingLog>> FetchTodaysLogs(int ExerciseId)
        {
            try
            {
                var today = DateTime.Now.Date;
                var tomorrow = today.AddDays(1);

                var connectionString = $"Data Source={databasePath}";

                using var connection = new System.Data.SQLite.SQLiteConnection(connectionString);
                await connection.OpenAsync();

                //var result = await connection.QueryAsync<TrainingLog>(SqlScripts.FetchTodaysLogs, new
                //{
                //    StartDate = today,
                //    EndDate = tomorrow,
                //    ExerciseId = ExerciseId
                //});
                
                var result = await connection.QueryAsync<TrainingLog>(SqlScripts.FetchTodaysLogsTest);

                return result.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<TrainingLog>(); // Or handle the exception as needed
            }
        }


        public class TrainingLogQueryParameters
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int ExerciseId { get; set; }
        }

        public static async Task AddDefaultRecordsIfNeeded()
        {
            try
            {
                var categories = await db.Table<Category>().ToListAsync();
                if (categories.Count == 0)
                {
                    var existingCategoryNames = new HashSet<string>(categories.Select(c => c.Name));
                    var categoriesToAdd = DefaultRecords.Categories.Where(c => !existingCategoryNames.Contains(c.Name)).ToList();

                    await db.RunInTransactionAsync(trans =>
                    {
                        foreach (var category in categoriesToAdd)
                        {
                            trans.Insert(category);
                        }
                    });
                }

                var exercises = await db.Table<Exercise>().ToListAsync();
                if (exercises.Count == 0)
                {
                    var existingExerciseNames = new HashSet<string>(exercises.Select(c => c.Name));
                    var exercisesToAdd = DefaultRecords.Exercises.Where(c => !existingExerciseNames.Contains(c.Name)).ToList();

                    await db.RunInTransactionAsync(trans =>
                    {
                        foreach (var exercise in exercisesToAdd)
                        {
                            trans.Insert(exercise);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as required
                //throw new InvalidOperationException("Failed to add default records", ex);
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
