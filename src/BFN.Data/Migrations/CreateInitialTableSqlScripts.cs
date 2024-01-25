namespace BFN.Data.Migrations;

public static class CreateInitialTableSqlScripts
{
    public static string CreateCategoryTableSql =>
        @"CREATE TABLE IF NOT EXISTS Category (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL
        );";

    public static string CreateExerciseTableSql =>
        @"CREATE TABLE IF NOT EXISTS Exercise (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Notes TEXT,
            CategoryId INTEGER NOT NULL,
            FOREIGN KEY (CategoryId) REFERENCES Category(Id)
        );";

    public static string CreateTrainingLogTableSql =>
        @"CREATE TABLE IF NOT EXISTS TrainingLog (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            ExerciseId INTEGER NOT NULL,
            Date DATE NOT NULL,
            MetricWeight INTEGER NOT NULL,
            Reps INTEGER NOT NULL,
            Distance INTEGER NOT NULL DEFAULT 0,
            DurationSeconds INTEGER NOT NULL DEFAULT 0,
            FOREIGN KEY (ExerciseId) REFERENCES Exercise(Id)
        );";
}
