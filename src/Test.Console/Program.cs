using SQLite;

var databasePath = @"C:\Users\Bedir\Downloads\FitNotes_Backup.db";
var db = new SQLiteAsyncConnection(databasePath);

var exerciseList = await db.Table<Exercise>().ToListAsync();
var categories = await db.Table<Category>().ToListAsync();

// Create a dictionary for category ID and Name
var categoryDictionary = categories.ToDictionary(c => c._Id, c => c.Name);

var sortedCategoryDictionary = categoryDictionary.OrderBy(pair => pair.Value).ToList();


List<Exercise> sortedExercises = exerciseList.OrderBy(exercise => exercise.Name).ToList();

var newDictionary = new Dictionary<int, string>();
int id = 1;
foreach (var item in sortedCategoryDictionary)
{
    newDictionary.Add(id, item.Value);
    id++;
}

var exercises = new List<TempExercise>();
foreach (var exercise in sortedExercises)
{
    //Console.WriteLine($"Exercise Id: {exercise._Id} Name: {exercise.Name}");

    // Find the category name using the category_id from the dictionary
    if (categoryDictionary.TryGetValue(exercise.category_id, out var categoryName))
    {
        //Console.WriteLine($"Category Id: {exercise.category_id} Name: {categoryName}");

        var newCategoryId = newDictionary.FirstOrDefault(x => x.Value == categoryName).Key;
        //Console.WriteLine($"New Category Id: {newCategoryId} Name: {categoryName}");

        var ex = new TempExercise
        {
            Name = exercise.Name,
            category_id = newCategoryId
        };

        exercises.Add(ex);

        Console.WriteLine("new() { CategoryId = " + newCategoryId + ", Name = \"" + exercise.Name + "\"},");
    }
}

class Exercise
{
    public int _Id { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public int category_id { get; set; }
}

class Category
{
    public int _Id { get; set; }
    public string Name { get; set; }
}

class TempExercise
{
    public string Name { get; set; }
    public int category_id { get; set; }
}
