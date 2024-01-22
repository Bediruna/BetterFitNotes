namespace BFN.Data.Models;

class TrainingLog : BaseModel
{
    public DateTime Date { get; set; }
    public int MetricWeight { get; set; }
    public int Reps { get; set; }
    public int Distance { get; set; }
    public int DurationSeconds { get; set; }
    public Exercise LogExercise { get; set; }
}
