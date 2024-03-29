namespace BFN.Data.Models;

public class ErrorLog : BaseModel
{
    public string ErrorMessage { get; set; }
    public DateTime ErrorDateTime { get; set; }
}