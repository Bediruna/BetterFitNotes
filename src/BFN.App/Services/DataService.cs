using BFN.Data.Migrations;
using BFN.Data.Models;
using SQLite;

namespace BFN.App.Services
{
    public static class DataService
    {
        public static SQLiteAsyncConnection db;
        private static bool isInitialized = false;

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
                    var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BFNData.db");
                    db = new SQLiteAsyncConnection(databasePath);
                    await db.CreateTableAsync<Category>();
                    await db.CreateTableAsync<Exercise>();
                    await db.CreateTableAsync<TrainingLog>();

                    var categoriesCount = await db.Table<Category>().CountAsync();
                    var exercisesCount = await db.Table<Exercise>().CountAsync();
                    if (categoriesCount == 0 || exercisesCount == 0)
                    {
                        await AddDefaultRecords();
                    }

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

        public static async Task AddDefaultRecords()
        {
            try
            {
                var existingCategories = await db.Table<Category>().ToListAsync();
                var existingCategoryNames = new HashSet<string>(existingCategories.Select(c => c.Name));
                var categoriesToAdd = DefaultRecords.Categories.Where(c => !existingCategoryNames.Contains(c.Name)).ToList();

                await db.RunInTransactionAsync(trans =>
                {
                    foreach (var category in categoriesToAdd)
                    {
                        trans.Insert(category);
                    }
                });

                var existingExercises = await db.Table<Exercise>().ToListAsync();
                var existingExerciseNames = new HashSet<string>(existingExercises.Select(c => c.Name));
                var exercisesToAdd = DefaultRecords.Exercises.Where(c => !existingExerciseNames.Contains(c.Name)).ToList();

                await db.RunInTransactionAsync(trans =>
                {
                    foreach (var exercise in exercisesToAdd)
                    {
                        trans.Insert(exercise);
                    }
                });
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
