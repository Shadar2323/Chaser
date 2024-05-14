using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.DrawerLayout.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaser
{
    [Activity(Label = "HomeScreenActivity")]
    public class HomeScreenActivity : Activity
    {
        ImageButton chaserButton;
        ImageButton fastQuiz;
        ImageButton settings;
        ImageButton profile;
        private SessionManager _sessionManager;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Lock screen orientation to portrait
            RequestedOrientation = ScreenOrientation.Portrait;

            fastQuiz = FindViewById<ImageButton>(Resource.Id.fastQuizButton);
            fastQuiz.Click += OpenQuizClick;
            chaserButton = FindViewById<ImageButton>(Resource.Id.chaserButton);
            chaserButton.Click += OpenGameSettingsClick;
            settings = FindViewById<ImageButton>(Resource.Id.settingsOpener);
            settings.Click += OpenSettingsClick;
            profile = FindViewById<ImageButton>(Resource.Id.playerStatsButton);
            profile.Click += OpenStatsClick;
            _sessionManager = new SessionManager(GetSharedPreferences("LoginPrefs", FileCreationMode.Private));
        }
        private void OpenQuizClick(object sender, EventArgs e)
        {
            Intent fastQuizIntent = new Intent(this, typeof(FastQuizActivity));
            StartActivity(fastQuizIntent);
            Finish();
        }
        private void OpenGameSettingsClick(object sender, EventArgs e)
        {
            //start the SettingsActivity
            Intent chaserSettingsIntent = new Intent(this, typeof(ChaserSettingsActivity));
            StartActivity(chaserSettingsIntent);
            Finish();

        }
        private void OpenStatsClick(object sender, EventArgs e)
        {
            Intent settingsIntent = new Intent(this, typeof(PlayerStatsActivity));
            StartActivity(settingsIntent);
            Finish();
        }
        private void OpenSettingsClick(object sender, EventArgs e)
        {
            Intent settingsIntent = new Intent(this, typeof(ProfileSettingsActivity));
            StartActivity(settingsIntent);
            Finish();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}