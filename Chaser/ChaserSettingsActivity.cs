﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaser
{
    [Activity(Label = "ChaserSettingsActivity")]
    public class ChaserSettingsActivity : Activity
    {
        Spinner diffSpinner;
        Spinner timeSpinner;
        Button startGame;
        Settings settings = Settings.Instance;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.settings);
            // Create your application here
            startGame = FindViewById<Button>(Resource.Id.startGameButton);
            startGame.SetBackgroundResource(Resource.Drawable.roundedButton);
            startGame.Click += StartGame_Click;

            diffSpinner = FindViewById<Spinner>(Resource.Id.difficultySpinner);
            timeSpinner = FindViewById<Spinner>(Resource.Id.timeSpinner);
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            string selectedDiff = diffSpinner.SelectedItem.ToString();
            string selectedTime = timeSpinner.SelectedItem.ToString();

            settings.Diff = checkDifficulty(selectedDiff);
            settings.Duration = Convert.ToInt32(selectedTime);

            Intent startGame = new Intent(this, typeof(ChaserGameActivity));
            StartActivity(startGame);
        }
        public string checkDifficulty(string diffValue)
        {
            string diff = "hard";
            if (diffValue=="קל")
            {
                return "easy";
            }
            if (diffValue == "בינוני")
            {
                return "medium";
            }
            return diff;
        }
    }
}