using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util.Logging;
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
        private int totalTimeInSeconds = 120;
        private ValueAnimator progressAnimator;
        QuizHandler quizHandler;
        private GridLayout answerGridLayout;
        private AnswerButton[] answerButtons;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.fastQuizLayout);
            quizHandler = new QuizHandler();
            tvCountdown = FindViewById<TextView>(Resource.Id.timerTextView);
            secondsRemaining = 120;
            countDownTimer = new Timer();
            countDownTimer.Interval = 1000; // 1 second interval
            countDownTimer.Elapsed += OnTimedEvent;
            countDownTimer.Start();

            timeProgressBar = FindViewById<ProgressBar>(Resource.Id.timeProgressBar);
            // Assuming you have a GridLayout in your XML layout with the id answerGridLayout
            answerGridLayout = FindViewById<GridLayout>(Resource.Id.answerGridLayout);

            // Add buttons to GridLayout
            AddButtons();
        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                secondsRemaining--;

                if (secondsRemaining >= 0)
                {
                    UpdateCountdown(secondsRemaining);
                    int progress = (int)(((float)secondsRemaining));
                    AnimateProgressBar(progress);
                }
                else
                {
                    // Handle countdown completion
                    countDownTimer.Stop();
                }
            });
        }
        private void AnimateProgressBar(int progress)
        {
            // Use ObjectAnimator for smooth progress animation
            ObjectAnimator animation = ObjectAnimator.OfInt(timeProgressBar, "progress", timeProgressBar.Progress, progress);
            animation.SetDuration(1000); // Animation duration in milliseconds
            animation.Start();
        }

        private void UpdateCountdown(int seconds)
        {
            // Format the time as "mm:ss"
            string formattedTime = $"{(seconds / 60):D2}:{(seconds % 60):D2}";
            // Update the TextView with the remaining time
            tvCountdown.Text = formattedTime.ToString();
        }
        private void AddButtons()
        {
            // Create AnswerButtons
            answerButtons = new AnswerButton[]
            {
        new AnswerButton(this, true),
        new AnswerButton(this, false),
        new AnswerButton(this, false),
        new AnswerButton(this, false)
            };

            // Set common layout parameters for buttons
        ViewGroup.MarginLayoutParams buttonLayoutParams = new ViewGroup.MarginLayoutParams(
        ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent
        );


            // Set margins for buttons
            int margin = 8; // Change this value as needed
            buttonLayoutParams.SetMargins(margin, margin, margin, margin);

            // Set background colors and add buttons to GridLayout
            for (int i = 0; i < answerButtons.Length; i++)
            {
                var button = answerButtons[i];

                if (button != null)
                {
                    // Set background color for the button
                    var colorFilter = new PorterDuffColorFilter(GetButtonColor(i), PorterDuff.Mode.Src);
                    button.Background.SetColorFilter(colorFilter);

                    // Add the button to GridLayout
                    answerGridLayout.AddView(button, buttonLayoutParams);

                    // Set rounded corners for buttons
                    button.SetBackgroundResource(Resource.Drawable.quizButtons);
                }
            }
        }
        // Helper method to get different colors for each button
        private Android.Graphics.Color GetButtonColor(int index)
        {
            // Example: Assign different colors based on the button index
            switch (index)
            {
                case 0: return Android.Graphics.Color.ParseColor("#FF5252");
                case 1: return Android.Graphics.Color.ParseColor("#69F0AE");
                case 2: return Android.Graphics.Color.ParseColor("#64B5F6");
                case 3: return Android.Graphics.Color.ParseColor("#FFD600");
                default: return Android.Graphics.Color.Gray;
            }
        }
    }
}