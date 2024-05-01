using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.Views.InputMethods;

namespace Chaser
{
    [Activity(Label = "ProfileSettingsActivity")]
    public class ProfileSettingsActivity : Activity
    {
        ImageView profileImage;
        Button editImage;
        EditText usernameEditText;
        ImageButton changeNameButton;
        ImageButton returnHome;
        TextView logOut;

        private SessionManager sessionManager;
        protected DatabaseHelper databaseHelper;
        Player player;
        string userName;
        private const int RequestWritePermissionCode = 100;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.playerSettings_layout);
            // Lock screen orientation to portrait
            RequestedOrientation = ScreenOrientation.Portrait;

            // Create your application here
            profileImage = FindViewById<ImageView>(Resource.Id.profileImage);
            editImage = FindViewById<Button>(Resource.Id.imageEdit);
            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            logOut = FindViewById<TextView>(Resource.Id.logOut);
            logOut.PaintFlags |= PaintFlags.UnderlineText;
            logOut.Click += Logout;

            // Find the ImageButton for changing the username
            changeNameButton = FindViewById<ImageButton>(Resource.Id.changeName);
            changeNameButton.Click += (sender, e) =>
            {
                ChangeUsername();
            };
            returnHome = FindViewById<ImageButton>(Resource.Id.returnHome);
            returnHome.Click += (sender, e) =>
            {
                Intent homeScreenIntent = new Intent(this, typeof(HomeScreenActivity));
                StartActivity(homeScreenIntent);
                Finish(); // Finish MainActivity so that pressing back doesn't return here
            };

            sessionManager = new SessionManager(GetSharedPreferences("LoginPrefs", FileCreationMode.Private));
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "trivia.db");
            databaseHelper = new DatabaseHelper(dbPath);

            userName = sessionManager.GetSavedUsername();
            usernameEditText.Text = userName;
            player = databaseHelper.GetCurrentPlayer(userName); // Retrieve the current player from your database or session
            string playerProfileImagePath = player.ProfileImage; // Get the profile image URI of the player

            // Check if player is not null before loading profile image
            if (player != null)
            {
                LoadProfileImage(playerProfileImagePath);
            }
            // Request write external storage permission
            RequestWriteExternalStoragePermission();

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
                        databaseHelper.UpdateImageProfile(userName, profileImagePath); // Implement this method to update player info in the database

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
        private void ChangeUsername()
        {
            usernameEditText.Enabled = true;
            usernameEditText.RequestFocus();
            InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
            imm.ShowSoftInput(usernameEditText, ShowFlags.Implicit);
            usernameEditText.EditorAction += (sender, e) =>
            {
                if (e.ActionId == ImeAction.Done)
                {
                    // Hide the keyboard
                    InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
                    imm.HideSoftInputFromWindow(usernameEditText.WindowToken, HideSoftInputFlags.None);

                    // Show the confirmation dialog
                    ShowConfirmationDialog();
                }
            };

        }
        private void ShowConfirmationDialog()
        {
            // Create a dialog builder
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Confirm Username Change");
            builder.SetMessage("Are you sure you want to change your username?");

            // Set positive button and its click listener
            builder.SetPositiveButton("Confirm", (sender, args) =>
            {
                // Get the updated username from the EditText
                string updatedUsername = usernameEditText.Text;
                bool success = databaseHelper.UpdateUserName(userName, updatedUsername);
                if (success)
                {
                    player.UserName = updatedUsername;
                    userName = updatedUsername;
                    player.UserName = userName;
                    sessionManager.SaveUsername(userName);
                    // Disable the EditText for editing
                    usernameEditText.Enabled = false;
                    usernameEditText.ClearFocus();
                }
                else

                {
                    Toast.MakeText(this, "Username already exists", ToastLength.Short).Show();
                    usernameEditText.Text = userName; // assuming you have the original username stored somewhere
                    usernameEditText.Enabled = false;
                    usernameEditText.ClearFocus();
                }
            });

            // Set negative button and its click listener
            builder.SetNegativeButton("Cancel", (sender, args) =>
            {
                // Revert any changes made to the EditText
                usernameEditText.Text = userName; // assuming you have the original username stored somewhere
                usernameEditText.Enabled = false;
                usernameEditText.ClearFocus();
            });

            // Create and show the dialog
            AlertDialog dialog = builder.Create();
            dialog.Show();
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
        private void RequestWriteExternalStoragePermission()
        {
            if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                // If permission is not granted, request it
                RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage }, RequestWritePermissionCode);
            }
        }
        private void Logout(object sender, EventArgs e)
        {
            sessionManager.ClearSession();
            // Navigate to MainActivity
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish(); // Finish the current activity
        }
    }
}