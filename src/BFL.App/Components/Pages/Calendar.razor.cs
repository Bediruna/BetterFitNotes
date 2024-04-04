using BFL.App.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BFL.App.Components.Pages
{
    public partial class Calendar : ComponentBase
    {
        [Inject]
        private DataService _dataService { get; set; }
        [Inject]
        private IJSRuntime JSRuntime { get; set; }
        private List<DateTime> displayedMonths = new();
        private DateTime initialMonth = DateTime.Today;
        private int monthsToDisplay = 24; // Initial number of months to display
        private ElementReference calendarContainer;

        protected override void OnInitialized()
        {
            try
            {
                LoadMonths(initialMonth, monthsToDisplay);
            }
            catch (Exception ex)
            {
                // Handle initialization exception (e.g., log the error or show a message to the user)
                Console.Error.WriteLine($"Error initializing Calendar: {ex.Message}");
            }
        }

        private void LoadMonths(DateTime startMonth, int count, bool prepend = false)
        {
            try
            {
                if (prepend)
                {
                    for (int i = count; i >= 1; i--)
                    {
                        displayedMonths.Insert(0, startMonth.AddMonths(-i));
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        displayedMonths.Add(startMonth.AddMonths(i));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception for loading months (e.g., log the error)
                Console.Error.WriteLine($"Error loading months: {ex.Message}");
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    await JSRuntime.InvokeVoidAsync("initializeScrollHandling", DotNetObjectReference.Create(this), "calendarContainer");
                }
                catch (Exception ex)
                {
                    // Handle JS invocation exception
                    Console.Error.WriteLine($"Error in JSRuntime invocation: {ex.Message}");
                }
            }
        }

        [JSInvokable]
        public async Task LoadMoreMonths(bool prepend)
        {
            try
            {
                if (prepend)
                {
                    var firstMonth = displayedMonths.First();
                    LoadMonths(firstMonth.AddMonths(0), 1, true);
                }
                else
                {
                    var lastMonth = displayedMonths.Last();
                    LoadMonths(lastMonth.AddMonths(1), 1);
                }

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                // Handle exception for loading more months (e.g., log the error)
                Console.Error.WriteLine($"Error loading more months: {ex.Message}");
            }
        }

        private RenderFragment RenderMonth(int month) => builder =>
        {
            try
            {
                var firstDayOfMonth = new DateTime(DateTime.Now.Year, month, 1);
                int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);
                int offset = (int)firstDayOfMonth.DayOfWeek;
                int row = 0;

                builder.OpenElement(row++, "tr");
                for (int i = 0; i < offset; i++)
                {
                    builder.OpenElement(row++, "td");
                    builder.CloseElement();
                }

                for (int day = 1; day <= daysInMonth; day++)
                {
                    if ((day + offset) % 7 == 1 && day > 1)
                    {
                        builder.CloseElement(); // Close the previous row
                        builder.OpenElement(row++, "tr"); // Start a new row
                    }

                    builder.OpenElement(row++, "td");
                    builder.AddContent(row++, day.ToString());
                    builder.CloseElement();
                }

                int remainingCells = (7 - ((daysInMonth + offset) % 7)) % 7;
                for (int i = 0; i < remainingCells; i++)
                {
                    builder.OpenElement(row++, "td");
                    builder.CloseElement();
                }

                builder.CloseElement(); // Close the last row
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur while rendering the month
                Console.Error.WriteLine($"Error rendering month: {ex.Message}");
                // Consider adding an error display element or some form of notification to the user
            }
        };
    }
}
