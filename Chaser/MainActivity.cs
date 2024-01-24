using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Content;
using System;
namespace Chaser
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button chaserButton;
        Button fastQuiz;
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}