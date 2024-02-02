namespace BFN.Data.Models.DTOs;

public class TrainingLogWithExerciseName
{
    public int ExerciseId { get; set; }
    public string ExerciseName { get; set; } // Name from Exercise class
    public int MetricWeight { get; set; }
    public int Reps { get; set; }
    public int Distance { get; set; }
    public int DurationSeconds { get; set; }
    public DateTime Date { get; set; }
}
