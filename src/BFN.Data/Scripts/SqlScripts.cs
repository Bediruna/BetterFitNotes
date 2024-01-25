namespace BFN.Data.Scripts;

public static class SqlScripts
{
    public static string FetchTodaysLogs =>
        @"SELECT * FROM TrainingLog
        WHERE Date >= @startDate AND Date < @endDate AND ExerciseId = @exerciseId";
    public static string FetchTodaysLogsTest =>
        @"SELECT * FROM TrainingLog";
}
