using BFN.Data.Models;
using SQLite;

namespace BFN.App.Services;

public static class CategoryService
{
    static SQLiteAsyncConnection db;

    static async Task Init()
    {
        // Get an absolute path to the database file
        var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BFNData.db");

        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<Category>();
    }

    public static async Task AddCategory(string name)
    {
        await Init();

        var category = new Category { Name = name };

        var id = await db.InsertAsync(category);
    }

    public static async Task RemoveCategory(int id)
    {
        await Init();

        await db.DeleteAsync<Category>(id);
    }
    public static async Task<IEnumerable<Category>> GetAllCategories()
    {
        await Init();

        return await db.Table<Category>().ToListAsync();
    }
}
