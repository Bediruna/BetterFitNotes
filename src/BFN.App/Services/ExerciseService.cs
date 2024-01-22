using BFN.Data.Models;
using SQLite;

namespace BFN.App.Services
{
    public static class ExerciseService
    {
        private static SQLiteAsyncConnection db;

        private static async Task InitializeDatabase()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BFNData.db");
            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<Exercise>();
        }

        public static async Task AddExercise(string name)
        {
            await InitializeDatabase();
            var Exercise = new Exercise { Name = name };
            await db.InsertAsync(Exercise);
        }

        public static async Task RemoveExercise(int id)
        {
            await InitializeDatabase();
            await db.DeleteAsync<Exercise>(id);
        }

        public static async Task RemoveAllExercises()
        {
            await InitializeDatabase();
            await db.DeleteAllAsync<Exercise>();
        }

        public static async Task<IEnumerable<Exercise>> GetAllExercises()
        {
            await InitializeDatabase();
            return await db.Table<Exercise>().ToListAsync();
        }

        public static async Task AddDefaultExercises()
        {
            await InitializeDatabase();
            var defaultExercises = new List<Exercise>
            {
                new() { Name = "Ab Coaster" , ExerciseId = 1},
                new() { Name = "Ab-Wheel Rollout" , ExerciseId = 1},
                new() { Name = "Arm Extension Machine" , ExerciseId = 8},
                new() { Name = "Arnold Dumbbell Press" , ExerciseId = 7},
                new() { Name = "Assisted Dip" , ExerciseId = 5},
                new() { Name = "Assisted Pull Up" , ExerciseId = 2},
                new() { Name = "Back Extension" , ExerciseId = 2},
                new() { Name = "Bar Push Down" , ExerciseId = 8},
                new() { Name = "Barbell Calf Raise" , ExerciseId = 6},
                new() { Name = "Barbell Curl" , ExerciseId = 3},
                new() { Name = "Barbell Front Squat" , ExerciseId = 6},
                new() { Name = "Barbell Glute Bridge" , ExerciseId = 6},
                new() { Name = "Barbell Row" , ExerciseId = 2},
                new() { Name = "Barbell Shrug" , ExerciseId = 2},
                new() { Name = "Barbell Squat" , ExerciseId = 6},
                new() { Name = "Behind The Neck Barbell Press" , ExerciseId = 7},
                new() { Name = "Box Jump" , ExerciseId = 6},
                new() { Name = "Cable Crossover" , ExerciseId = 5},
                new() { Name = "Cable Crunch" , ExerciseId = 1},
                new() { Name = "Cable Curl" , ExerciseId = 3},
                new() { Name = "Cable Face Pull" , ExerciseId = 7},
                new() { Name = "Cable Overhead Triceps Extension" , ExerciseId = 8},
                new() { Name = "Captain's Chair Leg Raise" , ExerciseId = 1},
                new() { Name = "Chin Up" , ExerciseId = 2},
                new() { Name = "Clap Pushups" , ExerciseId = 5},
                new() { Name = "Close Grip Barbell Bench Press" , ExerciseId = 8},
                new() { Name = "Close Grip Trap Barbell Pullup" , ExerciseId = 2},
                new() { Name = "Close Grip Trap Cable Pullup" , ExerciseId = 2},
                new() { Name = "Crunch" , ExerciseId = 1},
                new() { Name = "Crunch Machine" , ExerciseId = 1},
                new() { Name = "Cycling" , ExerciseId = 4},
                new() { Name = "Deadlift" , ExerciseId = 2},
                new() { Name = "Decline Barbell Bench Press" , ExerciseId = 5},
                new() { Name = "Decline Crunch" , ExerciseId = 1},
                new() { Name = "Decline Hammer Strength Chest Press" , ExerciseId = 5},
                new() { Name = "Deltoid Raise Machine" , ExerciseId = 7},
                new() { Name = "Dip" , ExerciseId = 5},
                new() { Name = "Donkey Calf Raise" , ExerciseId = 6},
                new() { Name = "Dragon Flag" , ExerciseId = 1},
                new() { Name = "Dumbbell Concentration Curl" , ExerciseId = 3},
                new() { Name = "Dumbbell Curl" , ExerciseId = 3},
                new() { Name = "Dumbbell Hammer Curl" , ExerciseId = 3},
                new() { Name = "Dumbbell Overhead Triceps Extension" , ExerciseId = 8},
                new() { Name = "Dumbbell Preacher Curl" , ExerciseId = 3},
                new() { Name = "Dumbbell Row" , ExerciseId = 2},
                new() { Name = "Elliptical Trainer" , ExerciseId = 4},
                new() { Name = "EZ-Bar Curl" , ExerciseId = 3},
                new() { Name = "EZ-Bar Preacher Curl" , ExerciseId = 3},
                new() { Name = "EZ-Bar Skullcrusher" , ExerciseId = 8},
                new() { Name = "Flat Barbell Bench Press" , ExerciseId = 5},
                new() { Name = "Flat Dumbbell Bench Press" , ExerciseId = 5},
                new() { Name = "Flat Dumbbell Fly" , ExerciseId = 5},
                new() { Name = "Front Dumbbell Raise" , ExerciseId = 7},
                new() { Name = "Front Pull Down" , ExerciseId = 2},
                new() { Name = "Glute-Ham Raise" , ExerciseId = 6},
                new() { Name = "Goblet Squat" , ExerciseId = 6},
                new() { Name = "Good Morning" , ExerciseId = 2},
                new() { Name = "Hack Squat" , ExerciseId = 6},
                new() { Name = "Hammer Strength Abdominal Crunch" , ExerciseId = 1},
                new() { Name = "Hammer Strength Iso-Lateral Wide Pulldown" , ExerciseId = 2},
                new() { Name = "Hammer Strength Row" , ExerciseId = 2},
                new() { Name = "Hammer Strength Shoulder Press" , ExerciseId = 7},
                new() { Name = "Hammer Strength Supinated Pulldown" , ExerciseId = 2},
                new() { Name = "Hanging Knee Raise" , ExerciseId = 1},
                new() { Name = "Hanging Leg Raise" , ExerciseId = 1},
                new() { Name = "High Row" , ExerciseId = 2},
                new() { Name = "Hoist Leg Press" , ExerciseId = 6},
                new() { Name = "Hoist Roc Abs Crunch" , ExerciseId = 1},
                new() { Name = "Hoist Roc Abs Rotated Crunch" , ExerciseId = 1},
                new() { Name = "Hoist Shoulder Press Machine" , ExerciseId = 7},
                new() { Name = "Incline Barbell Bench Press" , ExerciseId = 5},
                new() { Name = "Incline Dumbbell Bench Press" , ExerciseId = 5},
                new() { Name = "Incline Dumbbell Fly" , ExerciseId = 5},
                new() { Name = "Incline Hammer Strength Chest Press" , ExerciseId = 5},
                new() { Name = "Inner Thigh Machine" , ExerciseId = 6},
                new() { Name = "Lat Pulldown" , ExerciseId = 2},
                new() { Name = "Lat Pulldown Machine" , ExerciseId = 2},
                new() { Name = "Lateral Cable Raise" , ExerciseId = 7},
                new() { Name = "Lateral Dumbbell Raise" , ExerciseId = 7},
                new() { Name = "Lateral Machine Raise" , ExerciseId = 7},
                new() { Name = "Leaning Cable Lateral Raise" , ExerciseId = 7},
                new() { Name = "Leg Extension Machine" , ExerciseId = 6},
                new() { Name = "Leg Press" , ExerciseId = 6},
                new() { Name = "Log Press" , ExerciseId = 7},
                new() { Name = "Lunge" , ExerciseId = 6},
                new() { Name = "Lying Leg Curl Machine" , ExerciseId = 6},
                new() { Name = "Lying Triceps Extension" , ExerciseId = 8},
                new() { Name = "Machine Shrug" , ExerciseId = 2},
                new() { Name = "Multi-Press Flat" , ExerciseId = 5},
                new() { Name = "Multi-press Incline" , ExerciseId = 5},
                new() { Name = "Neutral Chin Up" , ExerciseId = 2},
                new() { Name = "One-Arm Standing Dumbbell Press" , ExerciseId = 7},
                new() { Name = "Outer Thigh Machine" , ExerciseId = 6},
                new() { Name = "Overhead Press" , ExerciseId = 7},
                new() { Name = "Parallel Bar Triceps Dip" , ExerciseId = 8},
                new() { Name = "Pendlay Row" , ExerciseId = 2},
                new() { Name = "Plank" , ExerciseId = 1},
                new() { Name = "Plate Loaded Chest Press" , ExerciseId = 5},
                new() { Name = "Plate Loaded Crunch" , ExerciseId = 1},
                new() { Name = "Plate Loaded Decline Press" , ExerciseId = 5},
                new() { Name = "Plate Loaded High Row" , ExerciseId = 2},
                new() { Name = "Plate Loaded Incline Press" , ExerciseId = 5},
                new() { Name = "Plate Loaded Lat Pulldown" , ExerciseId = 2},
                new() { Name = "Plate Loaded Low Row" , ExerciseId = 2},
                new() { Name = "Plate Loaded Preacher Curl" , ExerciseId = 3},
                new() { Name = "Plate Loaded Seated Dip" , ExerciseId = 8},
                new() { Name = "Plate Loaded Shoulder Press" , ExerciseId = 7},
                new() { Name = "Plate Loaded Squat" , ExerciseId = 6},
                new() { Name = "Pronated Wide Cable Curl" , ExerciseId = 3},
                new() { Name = "Pull Up" , ExerciseId = 2},
                new() { Name = "Push Press" , ExerciseId = 7},
                new() { Name = "Rack Pull" , ExerciseId = 2},
                new() { Name = "Rear Delt Dumbbell Raise" , ExerciseId = 7},
                new() { Name = "Rear Delt Machine Fly" , ExerciseId = 7},
                new() { Name = "Reverse Barbell Curl" , ExerciseId = 3},
                new() { Name = "Ring Dip" , ExerciseId = 8},
                new() { Name = "Romanian Deadlift" , ExerciseId = 6},
                new() { Name = "Rope Push Down" , ExerciseId = 8},
                new() { Name = "Rotary Torso" , ExerciseId = 1},
                new() { Name = "Rowing Machine" , ExerciseId = 4},
                new() { Name = "Running (Outdoor)" , ExerciseId = 4},
                new() { Name = "Running (Treadmill)" , ExerciseId = 4},
                new() { Name = "Seated Barbell Press" , ExerciseId = 7},
                new() { Name = "Seated Cable Row" , ExerciseId = 2},
                new() { Name = "Seated Calf Raise Machine" , ExerciseId = 6},
                new() { Name = "Seated Chest Press" , ExerciseId = 5},
                new() { Name = "Seated Dip Machine" , ExerciseId = 8},
                new() { Name = "Seated Dumbbell Curl" , ExerciseId = 3},
                new() { Name = "Seated Dumbbell Lateral Raise" , ExerciseId = 7},
                new() { Name = "Seated Dumbbell Press" , ExerciseId = 7},
                new() { Name = "Seated Incline Cable Curl" , ExerciseId = 3},
                new() { Name = "Seated Incline Dumbbell Curl" , ExerciseId = 3},
                new() { Name = "Seated Leg Curl Machine" , ExerciseId = 6},
                new() { Name = "Seated Leg Press" , ExerciseId = 6},
                new() { Name = "Seated Machine Curl" , ExerciseId = 3},
                new() { Name = "Seated Machine Fly" , ExerciseId = 5},
                new() { Name = "Seated Row Machine" , ExerciseId = 2},
                new() { Name = "Shoulder Press Machine" , ExerciseId = 7},
                new() { Name = "Side Plank" , ExerciseId = 1},
                new() { Name = "Sled Push And Pull" , ExerciseId = 6},
                new() { Name = "Smith Machine Close Grip Bench Press" , ExerciseId = 8},
                new() { Name = "Smith Machine Flat Bench Press" , ExerciseId = 5},
                new() { Name = "Smith Machine Incline Bench Press" , ExerciseId = 5},
                new() { Name = "Smith Machine Overhead Press" , ExerciseId = 7},
                new() { Name = "Standing Barbell Shoulder Press" , ExerciseId = 7},
                new() { Name = "Standing Calf Raise Machine" , ExerciseId = 6},
                new() { Name = "Stationary Bike" , ExerciseId = 4},
                new() { Name = "Stiff-Legged Deadlift" , ExerciseId = 6},
                new() { Name = "Straight-Arm Cable Pushdown" , ExerciseId = 2},
                new() { Name = "Sumo Deadlift" , ExerciseId = 6},
                new() { Name = "Swimming" , ExerciseId = 4},
                new() { Name = "T-Bar Row" , ExerciseId = 2},
                new() { Name = "Tricep Extension Machine" , ExerciseId = 8},
                new() { Name = "Tricep Press" , ExerciseId = 8},
                new() { Name = "V-Bar Push Down" , ExerciseId = 8},
                new() { Name = "Walking" , ExerciseId = 4},
                new() { Name = "Wide Cable Curl" , ExerciseId = 3},
                new() { Name = "Yoga" , ExerciseId = 4}
            };
            foreach (var Exercise in defaultExercises)
            {
                await db.InsertAsync(Exercise);
            }
        }
    }
}
