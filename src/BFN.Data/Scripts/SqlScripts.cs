namespace BFN.Data.Scripts;

public static class SqlScripts
{
    public static string GetLogsWithExerciseNameForDay =>
        @"SELECT t.*, e.Name AS ExerciseName
            FROM TrainingLog t
            INNER JOIN Exercise e ON t.ExerciseId = e.Id
            WHERE t.Date >= ? AND t.Date < ?
            ORDER BY t.Date DESC";
}
