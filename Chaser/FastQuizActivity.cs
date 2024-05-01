using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Util.Logging;
using Org.Apache.Commons.Logging;
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
        private int secondsRemaining = 10; // Initial countdown time in seconds
        private TextView tvCountdown;
        private ProgressBar timeProgressBar;
        private QuizHandler quizHandler;
        private AnswerButton[] answerButtons;
        private QAndA qAndA;
        private TextView questionText;
        private TextView answeredCorrectlyText;
        private ImageButton openDialogBtn;
        private SessionManager sessionManager;
        private string userName;
        private int questionNum = 1;
        private int answeredCorrectly = 0;
        private bool isDialogShown = false; // Flag to track whether the dialog is currently shown
        private AlertDialog dialog;
        private ValueAnimator progressBarAnimation; // Declare a class-level variable to store the progress bar animation

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.quizLayout);
            sessionManager = new SessionManager(GetSharedPreferences("LoginPrefs", FileCreationMode.Private));
            userName = sessionManager.GetSavedUsername();
            quizHandler = new QuizHandler(userName);
            timeProgressBar = FindViewById<ProgressBar>(Resource.Id.timeProgressBar);
            tvCountdown = FindViewById<TextView>(Resource.Id.timerTextView);

            // Lock screen orientation to portrait
            RequestedOrientation = ScreenOrientation.Portrait;

            // Create a new thread and start it
            Thread thread = new Thread(new ThreadStart(CountdownThread));
            thread.Start();
            AnimateProgressBar();
            questionText = FindViewById<TextView>(Resource.Id.questionTextView);
            answeredCorrectlyText = FindViewById<TextView>(Resource.Id.answeredCorrectlyTextView);

            openDialogBtn = FindViewById<ImageButton>(Resource.Id.openDialog);
            openDialogBtn.Click += (sender, e) =>
            {
                ShowCustomDialog();
            };

            answerButtons = new AnswerButton[4];
            //מייצר את המערך של הכפתורים
            InitializeAnswerButtons();
            UpdateScreen();
            StartMusic();
        }
        public void UpdateScreen()
        {
            qAndA = quizHandler.GetRandomQuestion();
            questionText.Text = qAndA.question;
            answeredCorrectlyText.Text = "Question "+questionNum;

            for (int i = 0; i < 4; i++)
            {
                Answer answer = qAndA.answers[i];
                answerButtons[i].Text = answer.answerText;
                answerButtons[i].IsTrue = answer.isTrue;
            }
        }
        private void ShowCustomDialog()
        {
            // Pause the countdown thread
            isDialogShown = true;

            // Stop the progress bar animation
            StopProgressBarAnimation();

            // Inflate the custom layout
            View dialogView = LayoutInflater.Inflate(Resource.Layout.dialog_return, null);

            // Create the AlertDialog builder
            var builder = new AlertDialog.Builder(this);
            builder.SetView(dialogView);

            // Create the AlertDialog object
            dialog = builder.Create();

            // Get references to dialog components
            TextView dialogMessage = dialogView.FindViewById<TextView>(Resource.Id.dialogMessage);
            Button btnReturnHome = dialogView.FindViewById<Button>(Resource.Id.btnReturnHome);
            Button btnReturnToGame = dialogView.FindViewById<Button>(Resource.Id.btnReturnToGame);

            // Set the message
            dialogMessage.Text = "Do you want to:";

            // Set click event for Return to Home button
            btnReturnHome.Click += (sender, e) =>
            {
                // Handle Return to Home button click
                // You can navigate to the home screen or perform any action here
                Intent mainActivityIntent = new Intent(this, typeof(HomeScreenActivity));
                StartActivity(mainActivityIntent);
            };

            // Set click event for Return to Game button
            btnReturnToGame.Click += (sender, e) =>
            {
                // Dismiss the dialog
                dialog.Dismiss();

                // Resume the countdown thread
                isDialogShown = false;

                // Restart the progress bar animation
                AnimateProgressBar();
            };

            // Show the dialog
            dialog.Show();
        }
        private void StopProgressBarAnimation()
        {
            // Cancel the progress bar animation
            if (progressBarAnimation != null)
            {
                progressBarAnimation.Cancel();
                progressBarAnimation = null;
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
                answeredCorrectly++;
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

                // Check if the dialog is shown
                if (!isDialogShown)
                {
                    // Decrease the countdown timer
                    secondsRemaining--;

                    if (secondsRemaining >= 0)
                    {
                        // Update the countdown UI on the main thread
                        RunOnUiThread(() => UpdateCountdown(secondsRemaining));
                    }
                }
            }
            RunOnUiThread(EndOfGame);
            // הטיימר נגמר - כמה שאלות ענית נכון, להציג את השיא ובחירה עוד משחק או חזרה למסך בית
        }
        private void AnimateProgressBar()
        {
            // Stop any existing animation
            StopProgressBarAnimation();

            progressBarAnimation = ValueAnimator.OfInt(timeProgressBar.Progress, 0);
            progressBarAnimation.SetDuration(secondsRemaining * 1000);
            progressBarAnimation.SetInterpolator(new Android.Views.Animations.LinearInterpolator());

            progressBarAnimation.Update += (sender, e) =>
            {
                int value = (int)progressBarAnimation.AnimatedValue;
                timeProgressBar.Progress = value;
            };

            progressBarAnimation.AnimationEnd += (sender, e) =>
            {
                // Animation has ended, you can perform any additional actions here
            };

            progressBarAnimation.Start();
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
        private void EndOfGame()
        {
            ShowVictoryDialog();
            StopMusic();
        }
        public void StartMusic()
        {
            StartService(new Intent(this, typeof(MusicService)));
        }
        public void StopMusic()
        {
            StopService(new Intent(this, typeof(MusicService)));
        }
        public void ShowVictoryDialog()
        {
            // Inflate the custom layout for the dialog
            var dialogView = LayoutInflater.Inflate(Resource.Layout.quiz_ending_dialog, null);

            // Find TextViews by their IDs
            var titleTextView = dialogView.FindViewById<TextView>(Resource.Id.title_text_view);
            var scoreTextView = dialogView.FindViewById<TextView>(Resource.Id.score_text_view);
            var newRecordTextView = dialogView.FindViewById<TextView>(Resource.Id.new_record_text_view);
            Button btnReturnHome = dialogView.FindViewById<Button>(Resource.Id.continue_button);
            Button btnReturnToGame = dialogView.FindViewById<Button>(Resource.Id.retry_button);

            // Update the text properties of TextViews
            titleTextView.Text = "Great Job!";
            scoreTextView.Text = answeredCorrectly + "/" + questionNum;
            newRecordTextView.Text = "New Record!";
            if (quizHandler.IsRecordHigher(answeredCorrectly))
            {
                newRecordTextView.Text = "New Record! \n " + answeredCorrectly;
                newRecordTextView.Visibility = ViewStates.Visible; // Make new record text visible
            }
            else
            {
                newRecordTextView.Text = "current record: " + quizHandler.currentRecord;
                newRecordTextView.Visibility = ViewStates.Visible; // Make new record text visible
            }
            // Set click event for Return to Home button
            btnReturnHome.Click += (sender, e) =>
            {
                // Handle Return to Home button click
                // You can navigate to the home screen or perform any action here
                Intent mainActivityIntent = new Intent(this, typeof(HomeScreenActivity));
                StartActivity(mainActivityIntent);
            };

            // Set click event for Return to Game button
            btnReturnToGame.Click += (sender, e) =>
            {
                // Handle Return to Game button click
                // You can perform any action related to returning to the game here
                Intent fastQuizIntent = new Intent(this, typeof(FastQuizActivity));
                StartActivity(fastQuizIntent);
            };

            // Create the AlertDialog builder
            var builder = new AlertDialog.Builder(this);
            builder.SetView(dialogView);

            // Create the AlertDialog object
            var dialog = builder.Create();

            // Show the dialog
            dialog.Show();
        }
    }
}