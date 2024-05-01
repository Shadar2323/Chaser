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
            //profile = FindViewById<ImageButton>(Resource.Id.imageButton);
            //profile.Click += ImageButton_Click;
            _sessionManager = new SessionManager(GetSharedPreferences("LoginPrefs", FileCreationMode.Private));

            //DrawerLayout drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //ListView drawerList = FindViewById<ListView>(Resource.Id.drawer_list);
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

        private void OpenSettingsClick(object sender, EventArgs e)
        {
            Intent settingsIntent = new Intent(this, typeof(ProfileSettingsActivity));
            StartActivity(settingsIntent);
            Finish();
        }

        //private void ShowPopupMenu(View v)
        //{
        //    PopupMenu popupMenu = new PopupMenu(this, v);
        //    popupMenu.Inflate(Resource.Menu.menu_drawer);
        //    popupMenu.MenuItemClick += PopupMenu_MenuItemClick;
        //    string username = _sessionManager.GetSavedUsername();

        //    // Find the "Hello" menu item
        //    IMenuItem helloMenuItem = popupMenu.Menu.FindItem(Resource.Id.menu_hello);

        //    helloMenuItem.SetTitle("Hello: " + username);
        //    popupMenu.Show();

        //}

        //private void PopupMenu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        //{
        //    switch (e.Item.ItemId)
        //    {
        //        case Resource.Id.menu_logout:
        //            // Handle logout button click
        //            // Add your logout logic here
        //            Logout();
        //            Toast.MakeText(this, "Logged out", ToastLength.Short).Show();
        //            Intent mainActivityIntent = new Intent(this, typeof(MainActivity));
        //            StartActivity(mainActivityIntent);

        //            break;
        //        case Resource.Id.menu_stats:
        //            // Navigate to PlayerStatsActivity
        //            Intent statsActivityIntent = new Intent(this, typeof(PlayerStatsActivity));
        //            StartActivity(statsActivityIntent);
        //            break;
        //    }
        //}
        private void Logout()
        {
            _sessionManager.ClearSession();
            Finish(); // Finish the current activity
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}