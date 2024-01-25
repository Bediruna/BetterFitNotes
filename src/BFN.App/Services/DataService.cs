using BFN.Data.Migrations;
using BFN.Data.Models;
using SQLite;

namespace BFN.App.Services
{
    public class DataService
    {
        public SQLiteAsyncConnection db;
        private static bool isInitialized = false;
        private static string databasePath;

        public DataService()
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

        private async Task InitializeDatabase()
        {
            if (!isInitialized)
            {
                try
                {
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BFNData.db");
                    db = new SQLiteAsyncConnection(databasePath);

                    await db.CreateTableAsync<Category>();
                    await db.CreateTableAsync<Exercise>();
                    await db.CreateTableAsync<TrainingLog>();

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

        public async Task<List<TrainingLog>> GetLogs(int exerciseId, DateTime specifiedDate)
        {
            try
            {
                var targetDate = specifiedDate.Date;
                var nextDay = targetDate.AddDays(1);

                var results = await db.Table<TrainingLog>()
                                      .Where(log => log.ExerciseId == exerciseId &&
                                                    log.Date >= targetDate &&
                                                    log.Date < nextDay)
                                      .OrderByDescending(log => log.Date)
                                      .ToListAsync();

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        public async Task AddDefaultRecordsIfNeeded()
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
