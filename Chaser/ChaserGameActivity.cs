using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Timers;
using Android.Util;
using System.Drawing;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Views.Animations;
using Android.Content.PM;

namespace Chaser
{
    [Activity(Label = "chaserGameActivity")]
    public class ChaserGameActivity : Activity
    {
        private TextView tvCountdown;
        private TextView playerName;
        private Timer countDownTimer;
        private int secondsRemaining;

        private ImageView chaserLogo;
        private ImageView playerLogo;
        private ImageView chestLogo;
        private ImageView playerProfileImage;
        QAndA qAndA;

        ImageButton btnShowDialog;

        GameHandler gameHandler;

        TextView question;

        AnswerButton[] answersButtons;

        private string playerProfileImagePath;
        private SessionManager sessionManager;
        protected DatabaseHelper databaseHelper;
        private string userName;
        Player currentPlayer;
        private AlertDialog dialog; // Declare a field to store the AlertDialog instance
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.chaser);
            gameHandler = new GameHandler();
            sessionManager = new SessionManager(GetSharedPreferences("LoginPrefs", FileCreationMode.Private));
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "trivia.db");
            databaseHelper = new DatabaseHelper(dbPath);

            // Lock screen orientation to landscape
            RequestedOrientation = ScreenOrientation.Landscape;

            qAndA = gameHandler.GetRandomQuestion();
            question = FindViewById<TextView>(Resource.Id.question);
            question.Text = qAndA.question;

            tvCountdown = FindViewById<TextView>(Resource.Id.tvCountdown);
            // Set the initial time (secondsRemaining)
            UpdateCountdown(secondsRemaining);

            //מייצר את המערך
            answersButtons = new AnswerButton[4];

            // Create and start the countdown timer
            secondsRemaining = gameHandler.GetDuration();
            countDownTimer = new Timer
            {
                Interval = 1000 // 1 second interval
            };
            countDownTimer.Elapsed += OnTimedEvent;
            countDownTimer.Start();

            playerName = FindViewById<TextView>(Resource.Id.playerName);
            userName = sessionManager.GetSavedUsername();
            currentPlayer = databaseHelper.GetCurrentPlayer(userName); // Retrieve the current player from your database or session

            SetPlayerDetails();
           


            chestLogo = FindViewById<ImageView>(Resource.Id.chestIcon);
            chaserLogo = FindViewById<ImageView>(Resource.Id.chaserLogo1);
            playerLogo = FindViewById<ImageView>(Resource.Id.playerLogo1);            
            SetPlayerLogo();

            btnShowDialog = FindViewById<ImageButton>(Resource.Id.openDialog);

            // Set click event for the button
            btnShowDialog.Click += (sender, e) =>
            {
                ShowCustomDialog();
            };


            // Create and set up AnswerButtons
            InitializeAnswerButtonsArray();
        }
        public void SetPlayerLogo()
        {
            // Get the current layout parameters
            ViewGroup.MarginLayoutParams layoutParams = (ViewGroup.MarginLayoutParams)playerLogo.LayoutParameters;
            string diff = gameHandler.GetDiff();
            int marginLeft = 0;

            switch (diff)
            {
                case "easy":
                    marginLeft = 150; // Adjust the margin value as needed
                    break;
                case "medium":
                    marginLeft = 100; // Adjust the margin value as needed
                    break;
                case "hard":
                    marginLeft = 50; // Adjust the margin value as needed
                    break;
            }
            // Set the left margin for the player logo
            layoutParams.LeftMargin = marginLeft;

            playerLogo.LayoutParameters = layoutParams;

        }
        private void SetPlayerDetails()
        {
            playerName.Text = userName;
            playerProfileImagePath = currentPlayer.ProfileImage; // Get the profile image URI of the player
            playerProfileImage = FindViewById<ImageView>(Resource.Id.playerPic);
            LoadProfileImage();
        }
        private void LoadProfileImage()
        {
            if (!string.IsNullOrEmpty(playerProfileImagePath))
            {
                Android.Graphics.Bitmap profileBitmap = BitmapFactory.DecodeFile(playerProfileImagePath);
                playerProfileImage.SetImageBitmap(profileBitmap);
                profileBitmap.Dispose();
            }
            else
            {
                playerProfileImage.SetImageResource(Resource.Drawable.player);                
            }
            playerProfileImage.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
        private void ShowCustomDialog()
        {
            // Pause the game
            PauseGame();

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
                Finish();
            };

            // Set click event for Return to Game button
            btnReturnToGame.Click += (sender, e) =>
            {
                // Dismiss the dialog
                dialog.Dismiss();

                // Resume the game
                ResumeGame();
            };

            // Show the dialog
            dialog = builder.Show();
        }

        private void MoveAnimation(ImageView logo)
        {

            logo.Animate()
                    .TranslationXBy(gameHandler.GetMoveAnimation())
                    .SetDuration(500) // Set the duration of the animation in milliseconds
                    .Start();
        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                secondsRemaining--;

                if (secondsRemaining >= 0)
                {
                    UpdateCountdown(secondsRemaining);
                }
                else
                {
                    UpdateScreen();
                    ResetTimer();
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

        private void InitializeAnswerButtonsArray()
        {
            answersButtons = new AnswerButton[]
            {
                FindViewById<AnswerButton>(Resource.Id.button1),
                FindViewById<AnswerButton>(Resource.Id.button2),
                FindViewById<AnswerButton>(Resource.Id.button3),
                FindViewById<AnswerButton>(Resource.Id.button4)
            };

            // Set the buttons to dusty pink
            foreach (var button in answersButtons)
            {
                button.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#D2B4B4"));
                button.Click += OnAnswerButtonClick;
            }
            UpdateScreen();
        }

        private void OnAnswerButtonClick(object sender, EventArgs e)
        {
            countDownTimer.Stop();
            // Handle the button click in the activity
            if (sender is AnswerButton clickedButton)
            {
                if (clickedButton.IsTrue)
                {
                    // The clicked button is marked as true
                    Toast.MakeText(this, "Correct!", ToastLength.Short).Show();
                    MoveAnimation(playerLogo);
                    int future = gameHandler.answeredCorrectly();
                    switch (future)
                    {
                        case 1:
                            MoveAnimation(chaserLogo);
                            EndOfGame(true);
                            break;
                        case 2:
                            EndOfGame(true);
                            break;
                        case 3:
                            MoveAnimation(chaserLogo);
                            UpdateScreen();
                            break;
                        case 4:
                            UpdateScreen();
                            break;
                    }
                }
                else
                {
                    // The clicked button is marked as false
                    Toast.MakeText(this, "Incorrect!", ToastLength.Short).Show();                    
                    switch (gameHandler.answeredInCorrectly())
                    {
                        case 1:
                            MoveAnimation(chaserLogo);
                            //הודעה כי הצ'ייסר ניצח
                            EndOfGame(false);
                            break;
                        case 2:
                            MoveAnimation(chaserLogo);
                            UpdateScreen();
                            //
                            break;
                        case 3:
                            //שני השחקנים טעו
                            UpdateScreen();
                            break;
                    }
                }
            }
        }

        public void UpdateScreen()
        {
            qAndA = gameHandler.GetRandomQuestion();
            question.Text = qAndA.question;
            for (int i = 0; i < 4; i++)
            {
                Answer answer = qAndA.answers[i];
                answersButtons[i].Text = answer.answerText;
                answersButtons[i].IsTrue = answer.isTrue;
            }
            ResetTimer();
        }
        private void ResetTimer()
        {
            // Stop the current timer
            countDownTimer.Stop();

            // Reset the secondsRemaining variable to its initial value
            secondsRemaining = gameHandler.GetDuration();

            // Update the countdown display with the initial value
            UpdateCountdown(secondsRemaining);

            // Start the timer again
            countDownTimer.Start();
        }
        private void ShowVictoryDialog(bool playerWon)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            LayoutInflater inflater = LayoutInflater.From(this);
            View dialogView = inflater.Inflate(Resource.Layout.victoryDialog, null, false);
            TextView titleView = dialogView.FindViewById<TextView>(Resource.Id.dialog_title);
            TextView messageView = dialogView.FindViewById<TextView>(Resource.Id.dialog_message);


            if (playerWon)
            {
                titleView.Text = "Victory";
                messageView.Text = "Congratulations! You have won!";
            }
            else
            {
                titleView.Text = "Chaser Won";
                messageView.Text = "Better luck next time! The chaser has won!";
            }

            builder.SetView(dialogView);

            // Set click handlers for buttons
            builder.SetPositiveButton("Another Game", (sender, args) =>
            {
                // Handle the "Another Game" button click
                // You can implement the logic to start a new game
                Intent chaserGameIntent = new Intent(this, typeof(ChaserSettingsActivity));
                StartActivity(chaserGameIntent);
            });

            builder.SetNegativeButton("Return Home", (sender, args) =>
            {
                // Handle the "Return Home" button click
                // You can implement the logic to return to the home screen
                Intent mainActivityIntent = new Intent(this, typeof(HomeScreenActivity));
                StartActivity(mainActivityIntent);
            });

            AlertDialog dialog = builder.Create();
            dialog.Show();

        }

        private void EndOfGame(bool playerWon)
        {
            countDownTimer.Stop();
            gameHandler.RestartGame();
            //await VictoryAnimnation();
            ShowVictoryDialog(playerWon);
        }
        private void PauseGame()
        {
            // Pause any game-related activities
            countDownTimer.Stop();
            // You can add more pause logic here if needed
        }

        private void ResumeGame()
        {
            // Resume game-related activities
            countDownTimer.Start();
            // You can add more resume logic here if needed
        }
        private async Task VictoryAnimnation()
        {
            Animation animScale = AnimationUtils.LoadAnimation(this, Resource.Animation.victory_anim);
            chestLogo.StartAnimation(animScale);
            await Task.Delay(3000);
        }
    }
}