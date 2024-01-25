using FluentMigrator;

namespace BFN.Data.Migrations;

[Migration(20230703120000)]
public class CreateInitialTablesMigration : Migration
{
    public override void Up()
    {
        // Check if the Category table exists
        if (!Schema.Table("Category").Exists())
        {
            Create.Table("Category")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable();
        }

        // Check if the Exercise table exists
        if (!Schema.Table("Exercise").Exists())
        {
            Create.Table("Exercise")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Notes").AsString().Nullable()
                .WithColumn("CategoryId").AsInt32().NotNullable()
                .ForeignKey("FK_Exercise_Category", "Category", "Id");
        }

        // Check if the TrainingLog table exists
        if (!Schema.Table("TrainingLog").Exists())
        {
            Create.Table("TrainingLog")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("ExerciseId").AsInt32().NotNullable()
                .WithColumn("Date").AsDate().NotNullable()
                .WithColumn("MetricWeight").AsInt32().NotNullable()
                .WithColumn("Reps").AsInt32().NotNullable()
                .WithColumn("Distance").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("DurationSeconds").AsInt32().NotNullable().WithDefaultValue(0)
                .ForeignKey("FK_TrainingLog_Exercise", "Exercise", "Id");
        }
    }

    public override void Down()
    {
        Delete.Table("TrainingLog");
        Delete.Table("Exercise");
        Delete.Table("Category");
    }
}
