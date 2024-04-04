using SQLite;

namespace BFL.Data.Models;

public abstract class BaseModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
}