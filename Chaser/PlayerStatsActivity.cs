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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Chaser
{
    [Activity(Label = "PlayerStatsActivity")]
    public class PlayerStatsActivity : Activity
    {
        ImageView profileImage;
        Button editImage;
        private SessionManager sessionManager;
        protected DatabaseHelper databaseHelper;
        Player player;
        string userName;
        private const int RequestWritePermissionCode = 100;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.player_stats);

            // Lock screen orientation to portrait
            RequestedOrientation = ScreenOrientation.Portrait;

            // Create your application here
            profileImage = FindViewById<ImageView>(Resource.Id.profileImage);
            editImage = FindViewById<Button>(Resource.Id.imageEdit);

            sessionManager = new SessionManager(GetSharedPreferences("LoginPrefs", FileCreationMode.Private));
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "trivia.db");
            databaseHelper = new DatabaseHelper(dbPath);

            userName = sessionManager.GetSavedUsername();
            player = databaseHelper.GetCurrentPlayer(userName); // Retrieve the current player from your database or session
            string playerProfileImagePath = player.ProfileImage; // Get the profile image URI of the player

            if (player != null)
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
                }
            }

            if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                // If permission is not granted, request it
                RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage }, RequestWritePermissionCode);
            }

            editImage.Click += ChangeImageButton_Click;

        }
        private void ChangeImageButton_Click(object sender, EventArgs e)
        {
            // Open image gallery to select a new profile image
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            intent.PutExtra("crop", "true"); // Enable cropping
            intent.PutExtra("aspectX", 1); // Aspect ratio X
            intent.PutExtra("aspectY", 1); // Aspect ratio Y
            intent.PutExtra("outputX", 100); // Output size X
            intent.PutExtra("outputY", 100); // Output size Y
            intent.PutExtra("return-data", true); // Return bitmap in the intent data
            StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), 0);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                if (requestCode == 0)
                {
                    // Get selected image URI
                    Android.Net.Uri selectedImageUri = data.Data;

                    // Convert URI to Bitmap
                    Android.Graphics.Bitmap profileBitmap = ImageHelper.GetBitmapFromUri(this, selectedImageUri);

                    if (profileBitmap != null)
                    {
                        // Crop the bitmap into a circular shape
                        Android.Graphics.Bitmap circularBitmap = ImageHelper.GetCircularBitmap(profileBitmap);

                        // Resize the circular bitmap to 100x100 dp
                        int diameterInPixels = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 100, Resources.DisplayMetrics);
                        Android.Graphics.Bitmap resizedBitmap = Android.Graphics.Bitmap.CreateScaledBitmap(circularBitmap, diameterInPixels, diameterInPixels, true);

                        // Save the bitmap to a file and update the player's profile image path in the database
                        string profileImagePath = ImageHelper.SaveBitmapToFile(resizedBitmap);

                        // Update the player's profile image view
                        profileImage.SetImageBitmap(resizedBitmap);

                        // Update the player's profile image path in the database
                        player.ProfileImage = profileImagePath; // Assuming you have a property like this in your Player class
                        databaseHelper.UpdateImageProfile(userName, player.ProfileImage); // Implement this method to update player info in the database

                        // Dispose the bitmaps to release memory
                        profileBitmap.Dispose();
                        circularBitmap.Dispose();
                        resizedBitmap.Dispose();
                    }
                    else
                    {
                        // Handle case where selected image cannot be converted to bitmap
                        // Display an error message or take appropriate action
                    }
                }
            }
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == RequestWritePermissionCode)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    editImage.Enabled = true;
                }
                else
                {
                    editImage.Enabled = false;
                    Toast.MakeText(this, "תאשר בבקשה ילד", ToastLength.Short).Show();
                }
            }
        }
    }
}