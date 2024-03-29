using BFN.Data.Migrations;
using BFN.Data.Models;
using BFN.Data.Models.DTOs;
using BFN.Data.Scripts;
using SQLite;

namespace BFN.App.Services
{
    public class DataService
    {
        public SQLiteAsyncConnection db;
        private static bool isInitialized = false;
        public DateOnly SelectedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public DataService()
        {
            try
            {
                Task.Run(InitializeDatabase).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task InitializeDatabase()
        {
            if (!isInitialized)
            {
                try
                {
                    db = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

                    await db.CreateTableAsync<Category>();
                    await db.CreateTableAsync<Exercise>();
                    await db.CreateTableAsync<TrainingLog>();
                    await db.CreateTableAsync<AppSettings>();
                    await db.CreateTableAsync<ErrorLog>();

                    await AddDefaultRecordsIfNeeded();

                    isInitialized = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public async Task<List<TrainingLogWithExerciseName>> GetExercises()
        {
            try
            {
                var startDate = SelectedDate;
                var nextDay = startDate.AddDays(1);

                var results = await db.QueryAsync<TrainingLogWithExerciseName>(SqlScripts.GetLogsWithExerciseNameForDay, startDate, nextDay);

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        public async Task<List<TrainingLog>> DeleteExerciseAndReturnUpdatedLogs(TrainingLog exerciseToDelete)
        {
            var updatedLogsForTheDay = new List<TrainingLog>();
            try
            {
                // Define the date range of interest
                var dateOfExercise = exerciseToDelete.LogDate;

                // Delete the exercise
                await db.DeleteAsync(exerciseToDelete);

                // Retrieve and update the OrderInDay for remaining exercises on the same day
                var exercisesForTheDay = await GetLogsForExercise(exerciseToDelete.Id);

                int updatedOrder = 1; // Start reordering from 1
                foreach (var exercise in exercisesForTheDay)
                {
                    if (exercise.Id != exerciseToDelete.Id) // Skip the deleted exercise (if it's still in the list due to async timing)
                    {
                        exercise.OrderInDay = updatedOrder++;
                        await db.UpdateAsync(exercise);
                    }
                }

                // Retrieve the updated list of logs for that day
                updatedLogsForTheDay = await GetLogsForExercise(exerciseToDelete.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Consider handling the error more gracefully, depending on your application's needs
            }

            return updatedLogsForTheDay;
        }


        public async Task<List<TrainingLog>> GetLogsForExercise(int exerciseId)
        {
            try
            {
                var startDate = SelectedDate;
                var nextDay = startDate.AddDays(1);

                var results = await db.Table<TrainingLog>()
                                      .Where(log => log.ExerciseId == exerciseId &&
                                                    log.LogDate >= startDate &&
                                                    log.LogDate < nextDay)
                                      .OrderBy(log => log.OrderInDay)
                                      .ThenByDescending(log => log.LogDate)
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

                var appSettingsList = await db.Table<AppSettings>().ToListAsync();
                if (appSettingsList.Count == 0)
                {
                    await db.InsertAsync(DefaultRecords.AppSettings);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<Dictionary<int, double>> GetPersonalRecordsForExercise(int exerciseId)
        {
            try
            {
                // Group logs by reps, then select the max weight for each group
                var logs = await db.Table<TrainingLog>()
                                   .Where(log => log.ExerciseId == exerciseId)
                                   .ToListAsync();

                var personalRecords = logs
                                      .GroupBy(log => log.Reps)
                                      .Select(g => new { Reps = g.Key, MaxWeight = g.Max(log => log.MetricWeight) })
                                      .ToDictionary(x => x.Reps, x => x.MaxWeight);

                return personalRecords;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

    }
}
