using BFN.Data.Models;
using SQLite;

namespace BFN.App.Services
{
    public static class CategoryService
    {
        private static SQLiteAsyncConnection db;

        private static async Task InitializeDatabase()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BFNData.db");
            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<Category>();
        }

        public static async Task AddCategory(string name)
        {
            await InitializeDatabase();
            var category = new Category { Name = name };
            await db.InsertAsync(category);
        }

        public static async Task RemoveCategory(int id)
        {
            await InitializeDatabase();
            await db.DeleteAsync<Category>(id);
        }

        public static async Task RemoveAllCategories()
        {
            await InitializeDatabase();
            await db.DeleteAllAsync<Category>();
        }

        public static async Task<IEnumerable<Category>> GetAllCategories()
        {
            await InitializeDatabase();
            return await db.Table<Category>().ToListAsync();
        }

        public static async Task AddDefaultCategories()
        {
            await InitializeDatabase();
            var defaultCategories = new List<Category>
            {
                new() { Name = "Abs" },
                new() { Name = "Back" },
                new() { Name = "Biceps" },
                new() { Name = "Cardio" },
                new() { Name = "Chest" },
                new() { Name = "Legs" },
                new() { Name = "Shoulders" },
                new() { Name = "Triceps" }
            };
            foreach (var category in defaultCategories)
            {
                await db.InsertAsync(category);
            }
        }
    }
}
