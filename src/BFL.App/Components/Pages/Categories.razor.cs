using BFL.App.Services;
using BFL.Data.Models;
using Microsoft.AspNetCore.Components;

namespace BFL.App.Components.Pages;

public partial class Categories : ComponentBase
{
    [Inject]
    private DataService _dataService { get; set; }

    private IEnumerable<Category> categories;

    protected override async Task OnInitializedAsync()
    {
        categories = await _dataService.db.Table<Category>().ToListAsync();
    }
}
