﻿using Android.Animation;
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
using System.Threading;
using System.Timers;

namespace Chaser
{
    [Activity(Label = "FastQuizActivity")]
    public class FastQuizActivity : Activity
    {
        private bool isRunning = true;
        private int secondsRemaining = 120; // Initial countdown time in seconds
        private TextView tvCountdown;
        private ProgressBar timeProgressBar;
        private int totalTimeInSeconds = 120;
        private ValueAnimator progressAnimator;
        private QuizHandler quizHandler;
        private AnswerButton[] answerButtons;
        private QAndA qAndA;
        private TextView questionText;
        private int questionNum = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.quizLayout);
            quizHandler = new QuizHandler();
            timeProgressBar = FindViewById<ProgressBar>(Resource.Id.timeProgressBar);
            tvCountdown = FindViewById<TextView>(Resource.Id.timerTextView);

            // Create a new thread and start it
            Thread thread = new Thread(new ThreadStart(CountdownThread));
            thread.Start();
            AnimateProgressBar();
            questionText = FindViewById<TextView>(Resource.Id.questionTextView);

            answerButtons = new AnswerButton[4];
            //מייצר את המערך של הכפתורים
            InitializeAnswerButtons();
            UpdateScreen();
        }
        public void UpdateScreen()
        {
            qAndA = quizHandler.GetRandomQuestion();
            questionText.Text =questionNum+". "+ qAndA.question;

            for (int i = 0; i < 4; i++)
            {
                Answer answer = qAndA.answers[i];
                answerButtons[i].Text = answer.answerText;
                answerButtons[i].IsTrue = answer.isTrue;
            }
        }
        private void InitializeAnswerButtons()
        {
            // Update the array definition at the beginning of your activity
            answerButtons[0] = FindViewById<AnswerButton>(Resource.Id.button1);
            answerButtons[1] = FindViewById<AnswerButton>(Resource.Id.button2);
            answerButtons[2] = FindViewById<AnswerButton>(Resource.Id.button3);
            answerButtons[3] = FindViewById<AnswerButton>(Resource.Id.button4);

            // Loop through each button to customize them
            for (int i = 0; i < answerButtons.Length; i++)
            {
                AnswerButton button = answerButtons[i];

                // Set elegant background color and text
                button.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(GetButtonColor(i));
                button.Text = $"Button {i + 1}";

                // Set up click event handler
                button.Click += AnswerButtonClick;
            }
        }
        // Handle button clicks
        private void AnswerButtonClick(object sender, EventArgs e)
        {
            AnswerButton clickedButton = (AnswerButton)sender;

            // Check if the clicked answer is correct
            if (clickedButton.IsTrue)
            {
                // Display a toast for correct answer
                Toast.MakeText(this, "Correct!", ToastLength.Short).Show();
            }
            else
            {
                // Display a toast for incorrect answer
                Toast.MakeText(this, "Incorrect. Try again!", ToastLength.Short).Show();
            }

            // Increment questionNum
            questionNum++;

            // Update the screen for the next question
            UpdateScreen();
        }

        private void CountdownThread()
        {
            while (secondsRemaining > 0)
            {
                Thread.Sleep(1000); // Sleep for 1 second

                // Decrease the countdown timer
                secondsRemaining--;

                if (secondsRemaining >= 0)
                {
                    // Update the countdown UI on the main thread
                    RunOnUiThread(() => UpdateCountdown(secondsRemaining));
                }
            }

            // Countdown finished, handle completion here
        }

        private void AnimateProgressBar()
        {
            ValueAnimator animation = ValueAnimator.OfInt(timeProgressBar.Progress, 0);
            animation.SetDuration(120000); // Animation duration in milliseconds (2 minutes)
            animation.SetInterpolator(new Android.Views.Animations.LinearInterpolator()); // Use a linear interpolator for a constant speed

            animation.Update += (sender, e) =>
            {
                int value = (int)animation.AnimatedValue;
                Android.OS.Handler handler = new Android.OS.Handler(Looper.MainLooper);
                handler.Post(() => timeProgressBar.Progress = value);
            };

            animation.AnimationEnd += (sender, e) =>
            {
                // Animation has ended, you can perform any additional actions here
            };

            animation.Start();
        }
        private void UpdateCountdown(int seconds)
        {
            // Format the time as "mm:ss"
            string formattedTime = $"{(seconds / 60):D2}:{(seconds % 60):D2}";
            // Update the TextView with the remaining time
            tvCountdown.Text = formattedTime;
        }
        // Helper method to get different colors for each button
        private Color GetButtonColor(int index)
        {
            switch (index)
            {
                case 0: return Android.Graphics.Color.ParseColor("#FF8A80"); // A shade of red
                case 1: return Android.Graphics.Color.ParseColor("#81C784"); // A shade of green
                case 2: return Android.Graphics.Color.ParseColor("#64B5F6"); // A shade of blue-gray
                case 3: return Android.Graphics.Color.ParseColor("#FFD54F"); // A shade of yellow
                default: return Android.Graphics.Color.Gray;
            }
        }
    }
}