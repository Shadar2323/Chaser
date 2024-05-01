using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chaser
{
    [Activity(Label = "PlayerStatsActivity")]
    public class PlayerStatsActivity : Activity
    {
        private SessionManager sessionManager;
        protected DatabaseHelper databaseHelper;
        Player player;
        string userName;

        ImageButton returnHome;
        private ImageView profileImage;
        private TextView userNameText;
        private TextView currentRecordNumber;
        private string playerProfileImagePath;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.player_stats);

            // Lock screen orientation to portrait
            RequestedOrientation = ScreenOrientation.Portrait;

            profileImage = FindViewById<ImageView>(Resource.Id.profile_image);
            userNameText = FindViewById<TextView>(Resource.Id.user_name);
            currentRecordNumber = FindViewById<TextView>(Resource.Id.current_record_number);

            sessionManager = new SessionManager(GetSharedPreferences("LoginPrefs", FileCreationMode.Private));
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "trivia.db");
            databaseHelper = new DatabaseHelper(dbPath);

            userName = sessionManager.GetSavedUsername();
            player = databaseHelper.GetCurrentPlayer(userName); // Retrieve the current player from your database or session
            playerProfileImagePath = player.ProfileImage; // Get the profile image URI of the player

            returnHome = FindViewById<ImageButton>(Resource.Id.returnHome);
            returnHome.Click += (sender, e) =>
            {
                Intent homeScreenIntent = new Intent(this, typeof(HomeScreenActivity));
                StartActivity(homeScreenIntent);
                Finish(); // Finish MainActivity so that pressing back doesn't return here
            };
            SetElements();

        }
        private void SetElements()
        {
            // Check if player is not null before loading profile image
            if (player != null)
            {
                LoadProfileImage(playerProfileImagePath);
            }
            userNameText.Text = player.UserName;
            currentRecordNumber.Text = player.Record.ToString();
        }
        private void LoadProfileImage(string playerProfileImagePath)
        {
            if (!string.IsNullOrEmpty(playerProfileImagePath))
            {
                Android.Graphics.Bitmap profileBitmap = BitmapFactory.DecodeFile(playerProfileImagePath);
                profileImage.SetImageBitmap(profileBitmap);
                profileBitmap.Dispose();
            }
            else
            {
                profileImage.SetImageResource(Resource.Drawable.player);
                profileImage.SetBackgroundColor(Color.Transparent);
            }
        }
    }
}