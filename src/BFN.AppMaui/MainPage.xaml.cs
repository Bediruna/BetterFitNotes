using BFN.Data.Models.DTOs;
using BFN.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BFN.AppMaui
{
    public partial class MainPage : ContentPage
    {
        private DataService _dataService; // Assume this is your service for data fetching
        public ObservableCollection<TrainingLogWithExerciseName> Logs { get; set; } = new ObservableCollection<TrainingLogWithExerciseName>();

        public MainPage(DataService dataService)
        {
            InitializeComponent();

            _dataService = dataService;
            LoadExercisesCommand.Execute(null);
        }

        public ICommand LoadExercisesCommand => new Command(async () => await LoadExercises());
        public ICommand GoToPreviousDayCommand => new Command(async () => await GoToPreviousDay());
        public ICommand SetDateToTodayCommand => new Command(async () => await SetDateToToday());
        public ICommand GoToNextDayCommand => new Command(async () => await GoToNextDay());


        private async Task LoadExercises()
        {
            var logs = await _dataService.GetExercises(); // This should match your actual method to fetch exercises
            Logs.Clear();
            foreach (var log in logs)
            {
                Logs.Add(log);
            }
        }
    }

}
