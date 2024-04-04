namespace BFL.Data.Models;

public class Exercise : BaseModel
{
    public string Name { get; set; }
    public string Notes { get; set; }
    public int CategoryId { get; set; }
}