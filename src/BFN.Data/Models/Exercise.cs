class Exercise : BaseModel
{
    public required string Name { get; set; }
    public required string Notes { get; set; }
    public required Category ExerciseCategory { get; set; }
}