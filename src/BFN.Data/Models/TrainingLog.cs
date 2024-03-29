namespace BFN.Data.Models;

public class TrainingLog : BaseModel
{
    public int ExerciseId { get; set; }
    public double MetricWeight { get; set; }
    public int Reps { get; set; }
    public int Distance { get; set; }
    public int DurationSeconds { get; set; }
    public int OrderInDay { get; set; }
    public DateOnly LogDate { get; set; }
    public bool IsPersonalRecord { get; set; }
    public TrainingLog()
    {
        Reps = 1;
    }
}
