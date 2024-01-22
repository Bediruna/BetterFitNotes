using SQLite;

namespace BFN.Data.Models;

public abstract class BaseModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
}