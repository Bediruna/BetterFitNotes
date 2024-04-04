using BFL.Data.Models;
using BFL.App.Services;
using Microsoft.AspNetCore.Components;
namespace BFL.App.Components.Pages;

public partial class Settings : ComponentBase
{

    [Inject]
    private DataService dataService { get; set; }

    private AppSettings appSettings = new AppSettings();
    private string errorMessage; // Variable to hold the error message

    protected override async Task OnInitializedAsync()
    {
        appSettings = await dataService.db.Table<AppSettings>().FirstOrDefaultAsync();
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            await dataService.db.UpdateAsync(appSettings);
            errorMessage = ""; // Clear the error message upon successful submission
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message; // Set the error message if an exception occurs
        }
    }
}
