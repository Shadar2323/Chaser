using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Chaser
{
    [Activity(Label = "FastQuizActivity")]
    public class FastQuizActivity : Activity
    {
        private Timer countDownTimer;
        private int secondsRemaining;
        private TextView tvCountdown;
        private ProgressBar timeProgressBar;
        private LinearLayout answersLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.fastQuizLayout);

            tvCountdown = FindViewById<TextView>(Resource.Id.timerTextView);
            secondsRemaining = 120;
            countDownTimer = new Timer();
            countDownTimer.Interval = 1000; // 1 second interval
            countDownTimer.Elapsed += OnTimedEvent;
            countDownTimer.Start();
            timeProgressBar = FindViewById<ProgressBar>(Resource.Id.timeProgressBar);

            // Get the LinearLayout container for answer buttons
            answersLayout = FindViewById<LinearLayout>(Resource.Id.answersLayout);

            // Add AnswerButtons dynamically
            AddAnswerButton("Paris");
            AddAnswerButton("Berlin");
            AddAnswerButton("London");
            AddAnswerButton("Madrid");
            // Create your application here
        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                secondsRemaining--;

                if (secondsRemaining >= 0)
                {
                    UpdateCountdown(secondsRemaining);
                    UpdateProgressBar();
                }
                else
                {
                    
                    // Handle countdown completion
                }
            });
        }

        private void UpdateCountdown(int seconds)
        {
            // Format the time as "mm:ss"
            string formattedTime = $"{(seconds / 60):D2}:{(seconds % 60):D2}";
            // Update the TextView with the remaining time
            tvCountdown.Text = formattedTime.ToString();
        }
        private void UpdateProgressBar()
        {
            int progress = (int)((float)(secondsRemaining / 120 * 100));
            timeProgressBar.Progress = progress;
        }
        private void AddAnswerButton(string answerText)
        {
            // Create a new instance of AnswerButton
            AnswerButton answerButton = new AnswerButton(this, isTrue: false)
            {
                LayoutParameters = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent
                ),
                Text = answerText,
                BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#3498db")),
                Alpha = 0.8f,
                TextSize = 18f,
            };

            // Set text color using the SetTextColor method
            answerButton.SetTextColor(Android.Graphics.Color.White);

            // Set padding using the SetPadding method
            answerButton.SetPadding(16, 16, 16, 16);

            // Attach an event handler for the button click
            answerButton.ButtonClick += (sender, e) =>
            {
                // Handle button click event
            };

            // Add the AnswerButton to the LinearLayout
            answersLayout.AddView(answerButton);
        }


    }
}