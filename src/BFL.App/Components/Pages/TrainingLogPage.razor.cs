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
    private TrainingLog selectedLog = new();
    private List<TrainingLog> exerciseLogs = [];
    private List<TrainingLog> logsForSelectedDate = [];

    private string errorMessage;
    private string primaryButtonText = "Save";
    private string secondaryButtonText = "Clear";
    private bool isEditMode = false;
    private Dictionary<int, double> personalRecords = [];


    private string selectedTab = "track";

    protected override async Task OnInitializedAsync()
    {
        exercise = await dataService.db.GetAsync<Exercise>(ExerciseId);
        exerciseLogs = await dataService.GetLogsForExercise(ExerciseId);

        logsForSelectedDate = await dataService.GetLogsForExerciseAndSelectedDate(ExerciseId);

        personalRecords = await dataService.GetPersonalRecordsForExercise(ExerciseId);
    }

    private void SelectLog(TrainingLog log)
    {
        selectedLog = log;
        isEditMode = true;
        primaryButtonText = "Update";
        secondaryButtonText = "Delete";
    }

    private async Task HandleValidSubmit()
    {
        if (selectedLog.Reps <= 0)
        {
            selectedLog.Reps = 1;
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
            await dataService.db.UpdateAsync(selectedLog);
            isEditMode = false;
            primaryButtonText = "Save";
            secondaryButtonText = "Clear";
        }
        else
        {
            selectedLog.ExerciseId = ExerciseId;
            selectedLog.ExerciseName = exercise.Name;
            selectedLog.LogDate = dataService.SelectedDate;
            await dataService.db.InsertAsync(selectedLog);
        }

        selectedLog = new TrainingLog
        {
            MetricWeight = selectedLog.MetricWeight,
            Reps = selectedLog.Reps,
        };

        /*
         * here we are hitting the db again, when we may be able to use the in memory list of logs and PRs
         * Adjust based on performance
         */
        logsForSelectedDate = await dataService.GetLogsForExerciseAndSelectedDate(ExerciseId);
        personalRecords = await dataService.GetPersonalRecordsForExercise(ExerciseId);
    }

    private async Task ClearOrDelete()
    {
        if (isEditMode)
        {
            //logs = await dataService.DeleteExerciseAndReturnUpdatedLogs(selectedLog);
            await dataService.db.DeleteAsync(selectedLog);
            logsForSelectedDate = await dataService.GetLogsForExerciseAndSelectedDate(ExerciseId);
            isEditMode = false;
            primaryButtonText = "Save";
            secondaryButtonText = "Clear";
        }

        selectedLog = new TrainingLog();
        StateHasChanged();
    }

    private void IncrementWeight()
    {
        int weightIncrement = 5;//might want to add this to appsettings
        selectedLog.MetricWeight += weightIncrement;
    }

    private void DecrementWeight()
    {
        int weightIncrement = 5;//might want to add this to appsettings
        if (selectedLog.MetricWeight > weightIncrement)
        {
            selectedLog.MetricWeight -= weightIncrement;
        }
    }

    private void IncrementReps()
    {
        selectedLog.Reps++;
    }

    private void DecrementReps()
    {
        if (selectedLog.Reps > 1)
        {
            selectedLog.Reps--;
        }
    }
}