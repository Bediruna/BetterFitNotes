using BFL.App.Services;
using BFL.Data.Models;
using Microsoft.AspNetCore.Components;

namespace BFL.App.Components.Pages;
public partial class TrainingLogPage : ComponentBase
{

    [Inject]
    private DataService dataService { get; set; }

    [Parameter]
    public int ExerciseId { get; set; }

    private Exercise exercise;
    private TrainingLog trainingLog = new();
    private List<TrainingLog> logs = [];

    private string errorMessage;
    private string primaryButtonText = "Save";
    private string secondaryButtonText = "Clear";
    private bool isEditMode = false;
    private Dictionary<int, double> personalRecords = [];

    protected override async Task OnInitializedAsync()
    {
        exercise = await dataService.db.GetAsync<Exercise>(ExerciseId);
        logs = await dataService.GetLogsForExercise(ExerciseId);

        personalRecords = await dataService.GetPersonalRecordsForExercise(ExerciseId);
    }

    private void SelectLog(TrainingLog log)
    {
        trainingLog = log;
        isEditMode = true;
        primaryButtonText = "Update";
        secondaryButtonText = "Delete";
    }

    private async Task HandleValidSubmit()
    {
        if (trainingLog.Reps <= 0)
        {
            trainingLog.Reps = 1;
            errorMessage = "Reps must be greater than 0.";
            StateHasChanged();
            return;
        }
        else
        {
            errorMessage = "";
        }

        if (isEditMode)
        {
            await dataService.db.UpdateAsync(trainingLog);
            isEditMode = false;
            primaryButtonText = "Save";
            secondaryButtonText = "Clear";
        }
        else
        {
            trainingLog.ExerciseId = ExerciseId;
            trainingLog.ExerciseName = exercise.Name;
            trainingLog.LogDate = dataService.SelectedDate;
            await dataService.db.InsertAsync(trainingLog);
        }

        trainingLog = new TrainingLog
        {
            MetricWeight = trainingLog.MetricWeight,
            Reps = trainingLog.Reps,
        };

        /*
         * here we are hitting the db again, when we may be able to use the in memory list of logs and PRs
         * Adjust based on performance
         */
        logs = await dataService.GetLogsForExercise(ExerciseId);
        personalRecords = await dataService.GetPersonalRecordsForExercise(ExerciseId);
    }

    private async Task ClearOrDelete()
    {
        if (isEditMode)
        {
            logs = await dataService.DeleteExerciseAndReturnUpdatedLogs(trainingLog);
            //logs = await dataService.GetLogsForExercise(ExerciseId);
            isEditMode = false;
            primaryButtonText = "Save";
            secondaryButtonText = "Clear";
        }

        trainingLog = new TrainingLog();
        StateHasChanged();
    }

    private void IncrementWeight()
    {
        int weightIncrement = 5;//might want to add this to appsettings
        trainingLog.MetricWeight += weightIncrement;
    }

    private void DecrementWeight()
    {
        int weightIncrement = 5;//might want to add this to appsettings
        if (trainingLog.MetricWeight > weightIncrement)
        {
            trainingLog.MetricWeight -= weightIncrement;
        }
    }

    private void IncrementReps()
    {
        trainingLog.Reps++;
    }

    private void DecrementReps()
    {
        if (trainingLog.Reps > 1)
        {
            trainingLog.Reps--;
        }
    }
}