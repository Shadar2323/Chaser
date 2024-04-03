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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.quizLayout);
            sessionManager = new SessionManager(GetSharedPreferences("LoginPrefs", FileCreationMode.Private));
            userName = sessionManager.GetSavedUsername();
            quizHandler = new QuizHandler(userName);
            timeProgressBar = FindViewById<ProgressBar>(Resource.Id.timeProgressBar);
            tvCountdown = FindViewById<TextView>(Resource.Id.timerTextView);
            

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
        }
        public void UpdateScreen()
        {
            qAndA = quizHandler.GetRandomQuestion();
            questionText.Text =questionNum+". "+ qAndA.question;
            answeredCorrectlyText.Text = "Answered Correctly: "+answeredCorrectly;

            for (int i = 0; i < 4; i++)
            {
                Answer answer = qAndA.answers[i];
                answerButtons[i].Text = answer.answerText;
                answerButtons[i].IsTrue = answer.isTrue;
            }
        }
        private void ShowCustomDialog()
        {
            // Inflate the custom layout
            View dialogView = LayoutInflater.Inflate(Resource.Layout.dialog_return, null);

            // Create the AlertDialog
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetView(dialogView);
            builder.SetCancelable(false);

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
                // Handle Return to Game button click
                // You can perform any action related to returning to the game here
                Intent chaserGameIntent = new Intent(this, typeof(ChaserGameActivity));
                StartActivity(chaserGameIntent);
            };

            // Show the dialog
            builder.Show();
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

                // Decrease the countdown timer
                secondsRemaining--;

                if (secondsRemaining >= 0)
                {
                    // Update the countdown UI on the main thread
                    RunOnUiThread(() => UpdateCountdown(secondsRemaining));
                }
            }
            RunOnUiThread(EndOfGame);
            // הטיימר נגמר - כמה שאלות ענית נכון, להציג את השיא ובחירה עוד משחק או חזרה למסך בית
        }

        private void AnimateProgressBar()
        {
            ValueAnimator animation = ValueAnimator.OfInt(timeProgressBar.Progress, 0);
            animation.SetDuration(secondsRemaining*1000); // Animation duration in milliseconds (2 minutes)
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
        private void EndOfGame()
        {
            // Inflate the custom layout for the dialog
            var dialogView = LayoutInflater.Inflate(Resource.Layout.quiz_ending_dialog, null);

            // Find TextViews by their IDs
            var titleTextView = dialogView.FindViewById<TextView>(Resource.Id.title_text_view);
            var scoreTextView = dialogView.FindViewById<TextView>(Resource.Id.score_text_view);
            var newRecordTextView = dialogView.FindViewById<TextView>(Resource.Id.new_record_text_view);

            // Update the text properties of TextViews
            titleTextView.Text = "Great Job!";
            scoreTextView.Text = "4/5";
            newRecordTextView.Text = "New Record!";
            if (quizHandler.IsRecordHigher(answeredCorrectly))
            {
                newRecordTextView.Text = "New Record! \n " + answeredCorrectly;
                newRecordTextView.Visibility = ViewStates.Visible; // Make new record text visible
            }
            else
            {
                newRecordTextView.Text = "current record: "+ quizHandler.currentRecord;
                newRecordTextView.Visibility = ViewStates.Visible; // Make new record text visible
            }

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