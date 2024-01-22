using BFN.Data.Models;
using SQLite;

namespace BFN.App.Services
{
    public static class CategoryService
    {
        public static SQLiteAsyncConnection db;
        private static bool isInitialized = false;

        static CategoryService()
        {
            InitializeDatabase();
        }

        private static void InitializeDatabase()
        {
            if (!isInitialized)
            {
                var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BFNData.db");
                db = new SQLiteAsyncConnection(databasePath);
                db.CreateTableAsync<Category>();
                isInitialized = true;
            }
        }
    }
}
