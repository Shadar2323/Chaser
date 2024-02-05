using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
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
        private int totalTimeInSeconds = 120;
        private ValueAnimator progressAnimator;
        private QuizHandler quizHandler;
        private AnswerButton[] answerButtons;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.quizLayout);
            quizHandler = new QuizHandler();
            tvCountdown = FindViewById<TextView>(Resource.Id.timerTextView);
            secondsRemaining = 120;
            countDownTimer = new Timer();
            countDownTimer.Interval = 1000; // 1 second interval
            countDownTimer.Elapsed += OnTimedEvent;
            countDownTimer.Start();

            timeProgressBar = FindViewById<ProgressBar>(Resource.Id.timeProgressBar);
            // Assuming you have a GridLayout in your XML layout with the id answerGridLayout
            //var answersLayout = FindViewById<GridLayout>(Resource.Id.quizAnswers);
            //מייצר את המערך של הכפתורים
            answerButtons = new AnswerButton[4];
            AnswerButton button1 = FindViewById<AnswerButton>(Resource.Id.button1);
            button1.Text = "Shalom";
            // Add buttons to GridLayout
            //AddAnswerButtons(answersLayout);
        }
        private void AddAnswerButtons(GridLayout gridLayout)
        {
            // Calculate button size based on screen width
            int buttonSizeDp = 40;
            int buttonSizePx = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, buttonSizeDp, Resources.DisplayMetrics);

            QAndA qAndA = quizHandler.GetRandomQuestion();
            // Create and add 4 AnswerButtons to the GridLayout
            for (int i = 0; i < 4; i++)
            {
                Answer answer = qAndA.answers[i];
                var answerButton = new AnswerButton(this, answer.isTrue);
                answerButton.Text = answer.answerText;

                // Set layout parameters for buttons
                GridLayout.LayoutParams buttonLayoutParams = new GridLayout.LayoutParams
                {
                    Width = buttonSizePx,
                    Height = buttonSizePx,
                    RowSpec = GridLayout.InvokeSpec(GridLayout.Undefined, 1f),
                    ColumnSpec = GridLayout.InvokeSpec(GridLayout.Undefined, 1f)
                };

                // Set background color for the button
                var colorFilter = new PorterDuffColorFilter(GetButtonColor(i), PorterDuff.Mode.Src);
                answerButton.Background.SetColorFilter(colorFilter);

                // Add the button to GridLayout
                gridLayout.AddView(answerButton, buttonLayoutParams);

                // Set rounded corners for buttons
                answerButton.SetBackgroundResource(Resource.Drawable.quizButtons);

                // Add spacer between buttons
                if (i < 3)
                {
                    var spacer = new View(this);
                    spacer.LayoutParameters = new GridLayout.LayoutParams
                    {
                        Width = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 1, Resources.DisplayMetrics),
                        Height = ViewGroup.LayoutParams.WrapContent,
                        RowSpec = GridLayout.InvokeSpec(GridLayout.Undefined, 1f),
                        ColumnSpec = GridLayout.InvokeSpec(GridLayout.Undefined, 0f)
                    };
                    gridLayout.AddView(spacer);
                }

                // Connect button click event
                //answerButton.ButtonClick += OnAnswerButtonClick;
                answerButtons[i] = answerButton;
            }
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