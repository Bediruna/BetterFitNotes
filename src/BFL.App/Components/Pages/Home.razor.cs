using BFL.App.Services;
using BFL.Data.Models;
using Microsoft.AspNetCore.Components;
namespace BFL.App.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    private DataService _dataService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    private List<TrainingLog> logs = [];

    protected override async Task OnInitializedAsync()
    {
        logs = await _dataService.GetExercises();
    }

    private string DisplayDateText
    {
        get
        {
            var today = DateTime.Today;
            var date = _dataService.SelectedDate;

            if (date == today)
            {
                return "Today";
            }
            else if (date == today.AddDays(-1))
            {
                return "Yesterday";
            }
            else if (date == today.AddDays(1))
            {
                return "Tomorrow";
            }
            else
            {
                return date.ToString("ddd, MMM dd");
            }
        }
    }

    private async Task SetDateToToday()
    {
        _dataService.SelectedDate = DateTime.Now;
        logs = await _dataService.GetExercises();
    }

    private async Task GoToPreviousDay()
    {
        _dataService.SelectedDate = _dataService.SelectedDate.AddDays(-1);
        logs = await _dataService.GetExercises();
    }

    private async Task GoToNextDay()
    {
        _dataService.SelectedDate = _dataService.SelectedDate.AddDays(1);
        logs = await _dataService.GetExercises();
    }

    private void NavigateToExercise(int exerciseId)
    {
        NavigationManager.NavigateTo($"/traininglog/{exerciseId}");
    }
}