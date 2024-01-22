namespace BFN.Data.Models;

public class Exercise : BaseModel
{
    public string Name { get; set; }
    public string Notes { get; set; }
    public Category ExerciseCategory { get; set; }
}