using BFL.App.Services;
using BFL.Data.Models;
using Microsoft.AspNetCore.Components;

namespace BFL.App.Components.Pages;

public partial class Exercises : ComponentBase
{
    [Inject]
    private DataService dataService { get; set; }

    [Parameter]
    public int CategoryId { get; set; }
    private Category category;

    private IEnumerable<Exercise> exercises;

    protected override async Task OnInitializedAsync()
    {
        category = await dataService.db.Table<Category>().FirstOrDefaultAsync(c => c.Id == CategoryId);
        exercises = await dataService.db.Table<Exercise>().Where(e => e.CategoryId == CategoryId).ToListAsync();
    }
}
