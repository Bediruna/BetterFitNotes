﻿using BFN.Data.Models;
using SQLite;

namespace BFN.App.Services
{
    public static class ExerciseService
    {
        private static SQLiteAsyncConnection db;
        private static bool isInitialized = false;

        static ExerciseService()
        {
            InitializeDatabase();
        }

        private static void InitializeDatabase()
        {
            if (!isInitialized)
            {
                var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BFNData.db");
                db = new SQLiteAsyncConnection(databasePath);
                db.CreateTableAsync<Exercise>();
                isInitialized = true;
            }
        }

        public static async Task AddDefaultRecords()
        {
            var defaultCategories = new List<Category>
            {
                new() { Id = 1, Name = "Abs" },
                new() { Id = 2, Name = "Back" },
                new() { Id = 3, Name = "Biceps" },
                new() { Id = 4, Name = "Cardio" },
                new() { Id = 5, Name = "Chest" },
                new() { Id = 6, Name = "Legs" },
                new() { Id = 7, Name = "Shoulders" },
                new() { Id = 8, Name = "Triceps" }
            };

            var existingCategories = await db.Table<Category>().ToListAsync();
            var existingCategoryNames = new HashSet<string>(existingCategories.Select(c => c.Name));

            var categoriesToAdd = defaultCategories.Where(c => !existingCategoryNames.Contains(c.Name)).ToList();

            await db.RunInTransactionAsync(trans =>
            {
                foreach (var category in categoriesToAdd)
                {
                    trans.Insert(category);
                }
            });

            var defaultExercises = new List<Exercise>
            {
                new() { CategoryId = 1, Name = "Ab Coaster"},
                new() { CategoryId = 1, Name = "Ab-Wheel Rollout"},
                new() { CategoryId = 8, Name = "Arm Extension Machine"},
                new() { CategoryId = 7, Name = "Arnold Dumbbell Press"},
                new() { CategoryId = 5, Name = "Assisted Dip"},
                new() { CategoryId = 2, Name = "Assisted Pull Up"},
                new() { CategoryId = 2, Name = "Back Extension"},
                new() { CategoryId = 8, Name = "Bar Push Down"},
                new() { CategoryId = 6, Name = "Barbell Calf Raise"},
                new() { CategoryId = 3, Name = "Barbell Curl"},
                new() { CategoryId = 6, Name = "Barbell Front Squat"},
                new() { CategoryId = 6, Name = "Barbell Glute Bridge"},
                new() { CategoryId = 2, Name = "Barbell Row"},
                new() { CategoryId = 2, Name = "Barbell Shrug"},
                new() { CategoryId = 6, Name = "Barbell Squat"},
                new() { CategoryId = 7, Name = "Behind The Neck Barbell Press"},
                new() { CategoryId = 6, Name = "Box Jump"},
                new() { CategoryId = 5, Name = "Cable Crossover"},
                new() { CategoryId = 1, Name = "Cable Crunch"},
                new() { CategoryId = 3, Name = "Cable Curl"},
                new() { CategoryId = 7, Name = "Cable Face Pull"},
                new() { CategoryId = 8, Name = "Cable Overhead Triceps Extension"},
                new() { CategoryId = 1, Name = "Captain's Chair Leg Raise"},
                new() { CategoryId = 2, Name = "Chin Up"},
                new() { CategoryId = 5, Name = "Clap Pushups"},
                new() { CategoryId = 8, Name = "Close Grip Barbell Bench Press"},
                new() { CategoryId = 2, Name = "Close Grip Trap Barbell Pullup"},
                new() { CategoryId = 2, Name = "Close Grip Trap Cable Pullup"},
                new() { CategoryId = 1, Name = "Crunch"},
                new() { CategoryId = 1, Name = "Crunch Machine"},
                new() { CategoryId = 4, Name = "Cycling"},
                new() { CategoryId = 2, Name = "Deadlift"},
                new() { CategoryId = 5, Name = "Decline Barbell Bench Press"},
                new() { CategoryId = 1, Name = "Decline Crunch"},
                new() { CategoryId = 5, Name = "Decline Hammer Strength Chest Press"},
                new() { CategoryId = 7, Name = "Deltoid Raise Machine"},
                new() { CategoryId = 5, Name = "Dip"},
                new() { CategoryId = 6, Name = "Donkey Calf Raise"},
                new() { CategoryId = 1, Name = "Dragon Flag"},
                new() { CategoryId = 3, Name = "Dumbbell Concentration Curl"},
                new() { CategoryId = 3, Name = "Dumbbell Curl"},
                new() { CategoryId = 3, Name = "Dumbbell Hammer Curl"},
                new() { CategoryId = 8, Name = "Dumbbell Overhead Triceps Extension"},
                new() { CategoryId = 3, Name = "Dumbbell Preacher Curl"},
                new() { CategoryId = 2, Name = "Dumbbell Row"},
                new() { CategoryId = 4, Name = "Elliptical Trainer"},
                new() { CategoryId = 3, Name = "EZ-Bar Curl"},
                new() { CategoryId = 3, Name = "EZ-Bar Preacher Curl"},
                new() { CategoryId = 8, Name = "EZ-Bar Skullcrusher"},
                new() { CategoryId = 5, Name = "Flat Barbell Bench Press"},
                new() { CategoryId = 5, Name = "Flat Dumbbell Bench Press"},
                new() { CategoryId = 5, Name = "Flat Dumbbell Fly"},
                new() { CategoryId = 7, Name = "Front Dumbbell Raise"},
                new() { CategoryId = 2, Name = "Front Pull Down"},
                new() { CategoryId = 6, Name = "Glute-Ham Raise"},
                new() { CategoryId = 6, Name = "Goblet Squat"},
                new() { CategoryId = 2, Name = "Good Morning"},
                new() { CategoryId = 6, Name = "Hack Squat"},
                new() { CategoryId = 1, Name = "Hammer Strength Abdominal Crunch"},
                new() { CategoryId = 2, Name = "Hammer Strength Iso-Lateral Wide Pulldown"},
                new() { CategoryId = 2, Name = "Hammer Strength Row"},
                new() { CategoryId = 7, Name = "Hammer Strength Shoulder Press"},
                new() { CategoryId = 2, Name = "Hammer Strength Supinated Pulldown"},
                new() { CategoryId = 1, Name = "Hanging Knee Raise"},
                new() { CategoryId = 1, Name = "Hanging Leg Raise"},
                new() { CategoryId = 2, Name = "High Row"},
                new() { CategoryId = 6, Name = "Hoist Leg Press"},
                new() { CategoryId = 1, Name = "Hoist Roc Abs Crunch"},
                new() { CategoryId = 1, Name = "Hoist Roc Abs Rotated Crunch"},
                new() { CategoryId = 7, Name = "Hoist Shoulder Press Machine"},
                new() { CategoryId = 5, Name = "Incline Barbell Bench Press"},
                new() { CategoryId = 5, Name = "Incline Dumbbell Bench Press"},
                new() { CategoryId = 5, Name = "Incline Dumbbell Fly"},
                new() { CategoryId = 5, Name = "Incline Hammer Strength Chest Press"},
                new() { CategoryId = 6, Name = "Inner Thigh Machine"},
                new() { CategoryId = 2, Name = "Lat Pulldown"},
                new() { CategoryId = 2, Name = "Lat Pulldown Machine"},
                new() { CategoryId = 7, Name = "Lateral Cable Raise"},
                new() { CategoryId = 7, Name = "Lateral Dumbbell Raise"},
                new() { CategoryId = 7, Name = "Lateral Machine Raise"},
                new() { CategoryId = 7, Name = "Leaning Cable Lateral Raise"},
                new() { CategoryId = 6, Name = "Leg Extension Machine"},
                new() { CategoryId = 6, Name = "Leg Press"},
                new() { CategoryId = 7, Name = "Log Press"},
                new() { CategoryId = 6, Name = "Lunge"},
                new() { CategoryId = 6, Name = "Lying Leg Curl Machine"},
                new() { CategoryId = 8, Name = "Lying Triceps Extension"},
                new() { CategoryId = 2, Name = "Machine Shrug"},
                new() { CategoryId = 5, Name = "Multi-Press Flat"},
                new() { CategoryId = 5, Name = "Multi-press Incline"},
                new() { CategoryId = 2, Name = "Neutral Chin Up"},
                new() { CategoryId = 7, Name = "One-Arm Standing Dumbbell Press"},
                new() { CategoryId = 6, Name = "Outer Thigh Machine"},
                new() { CategoryId = 7, Name = "Overhead Press"},
                new() { CategoryId = 8, Name = "Parallel Bar Triceps Dip"},
                new() { CategoryId = 2, Name = "Pendlay Row"},
                new() { CategoryId = 1, Name = "Plank"},
                new() { CategoryId = 5, Name = "Plate Loaded Chest Press"},
                new() { CategoryId = 1, Name = "Plate Loaded Crunch"},
                new() { CategoryId = 5, Name = "Plate Loaded Decline Press"},
                new() { CategoryId = 2, Name = "Plate Loaded High Row"},
                new() { CategoryId = 5, Name = "Plate Loaded Incline Press"},
                new() { CategoryId = 2, Name = "Plate Loaded Lat Pulldown"},
                new() { CategoryId = 2, Name = "Plate Loaded Low Row"},
                new() { CategoryId = 3, Name = "Plate Loaded Preacher Curl"},
                new() { CategoryId = 8, Name = "Plate Loaded Seated Dip"},
                new() { CategoryId = 7, Name = "Plate Loaded Shoulder Press"},
                new() { CategoryId = 6, Name = "Plate Loaded Squat"},
                new() { CategoryId = 3, Name = "Pronated Wide Cable Curl"},
                new() { CategoryId = 2, Name = "Pull Up"},
                new() { CategoryId = 7, Name = "Push Press"},
                new() { CategoryId = 2, Name = "Rack Pull"},
                new() { CategoryId = 7, Name = "Rear Delt Dumbbell Raise"},
                new() { CategoryId = 7, Name = "Rear Delt Machine Fly"},
                new() { CategoryId = 3, Name = "Reverse Barbell Curl"},
                new() { CategoryId = 8, Name = "Ring Dip"},
                new() { CategoryId = 6, Name = "Romanian Deadlift"},
                new() { CategoryId = 8, Name = "Rope Push Down"},
                new() { CategoryId = 1, Name = "Rotary Torso"},
                new() { CategoryId = 4, Name = "Rowing Machine"},
                new() { CategoryId = 4, Name = "Running (Outdoor)"},
                new() { CategoryId = 4, Name = "Running (Treadmill)"},
                new() { CategoryId = 7, Name = "Seated Barbell Press"},
                new() { CategoryId = 2, Name = "Seated Cable Row"},
                new() { CategoryId = 6, Name = "Seated Calf Raise Machine"},
                new() { CategoryId = 5, Name = "Seated Chest Press"},
                new() { CategoryId = 8, Name = "Seated Dip Machine"},
                new() { CategoryId = 3, Name = "Seated Dumbbell Curl"},
                new() { CategoryId = 7, Name = "Seated Dumbbell Lateral Raise"},
                new() { CategoryId = 7, Name = "Seated Dumbbell Press"},
                new() { CategoryId = 3, Name = "Seated Incline Cable Curl"},
                new() { CategoryId = 3, Name = "Seated Incline Dumbbell Curl"},
                new() { CategoryId = 6, Name = "Seated Leg Curl Machine"},
                new() { CategoryId = 6, Name = "Seated Leg Press"},
                new() { CategoryId = 3, Name = "Seated Machine Curl"},
                new() { CategoryId = 5, Name = "Seated Machine Fly"},
                new() { CategoryId = 2, Name = "Seated Row Machine"},
                new() { CategoryId = 7, Name = "Shoulder Press Machine"},
                new() { CategoryId = 1, Name = "Side Plank"},
                new() { CategoryId = 6, Name = "Sled Push And Pull"},
                new() { CategoryId = 8, Name = "Smith Machine Close Grip Bench Press"},
                new() { CategoryId = 5, Name = "Smith Machine Flat Bench Press"},
                new() { CategoryId = 5, Name = "Smith Machine Incline Bench Press"},
                new() { CategoryId = 7, Name = "Smith Machine Overhead Press"},
                new() { CategoryId = 7, Name = "Standing Barbell Shoulder Press"},
                new() { CategoryId = 6, Name = "Standing Calf Raise Machine"},
                new() { CategoryId = 4, Name = "Stationary Bike"},
                new() { CategoryId = 6, Name = "Stiff-Legged Deadlift"},
                new() { CategoryId = 2, Name = "Straight-Arm Cable Pushdown"},
                new() { CategoryId = 6, Name = "Sumo Deadlift"},
                new() { CategoryId = 4, Name = "Swimming"},
                new() { CategoryId = 2, Name = "T-Bar Row"},
                new() { CategoryId = 8, Name = "Tricep Extension Machine"},
                new() { CategoryId = 8, Name = "Tricep Press"},
                new() { CategoryId = 8, Name = "V-Bar Push Down"},
                new() { CategoryId = 4, Name = "Walking"},
                new() { CategoryId = 3, Name = "Wide Cable Curl"},
                new() { CategoryId = 4, Name = "Yoga"}
            };
            foreach (var Exercise in defaultExercises)
            {
                await db.InsertAsync(Exercise);
            }
        }
    }
}
