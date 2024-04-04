namespace BFL.Data.Models;

public class AppSettings : BaseModel
{
    public int WeightIncrement { get; set; }
    public bool UseMetric { get; set; }
    public bool UseGraphicsForExercises {  get; set; }
}