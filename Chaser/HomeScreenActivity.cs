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

namespace Chaser
{
    [Activity(Label = "HomeScreenActivity")]
    public class HomeScreenActivity : Activity
    {
        Button chaserButton;
        Button fastQuiz;
        Button register;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            fastQuiz = FindViewById<Button>(Resource.Id.fastQuizButton);
            fastQuiz.Click += OpenQuizClick;
            chaserButton = FindViewById<Button>(Resource.Id.chaserButton);
            chaserButton.Click += OpenSettingsClick;
            register = FindViewById<Button>(Resource.Id.registerButton);
            register.Click += OpenRegisterClick;
        }
        private void OpenQuizClick(object sender, EventArgs e)
        {
            Intent fastQuizIntent = new Intent(this, typeof(FastQuizActivity));
            StartActivity(fastQuizIntent);
        }
        private void OpenSettingsClick(object sender, EventArgs e)
        {
            //start the SettingsActivity
            Intent chaserSettingsIntent = new Intent(this, typeof(ChaserSettingsActivity));
            StartActivity(chaserSettingsIntent);
        }
        private void OpenRegisterClick(object sender, EventArgs e)
        {
            Intent registerIntent = new Intent(this, typeof(RegisterActivity));
            StartActivity(registerIntent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}