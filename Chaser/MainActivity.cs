﻿using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Content;
using System;
using System.IO;
namespace Chaser
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private EditText usernameEditText;
        private EditText passwordEditText;
        private Button loginButton;
        private Button registerButton;
        private DatabaseHelper databaseHelper;
        private CheckBox rememberUsernameCheckbox;
        private SessionManager _sessionManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.logIn);

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "trivia.db");
            databaseHelper = new DatabaseHelper(dbPath);
            _sessionManager = new SessionManager(GetSharedPreferences("LoginPrefs", FileCreationMode.Private));


            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            loginButton = FindViewById<Button>(Resource.Id.loginButton);
            registerButton = FindViewById<Button>(Resource.Id.registerButton);
            rememberUsernameCheckbox = FindViewById<CheckBox>(Resource.Id.rememberUsernameCheckbox);

            loginButton.Click += LoginButton_Click;
            registerButton.Click += RegisterButton_Click;
            // Auto-login if the user is already logged in
            if (_sessionManager.IsLoggedIn())
            {
                StartHomeScreenActivity();
            }
            else
            {
                LoadRememberedUsername();
            }
        }
        private void LoadRememberedUsername()
        {
            if (_sessionManager.IsLoggedIn())
            {
                string savedUsername = _sessionManager.GetSavedUsername();
                usernameEditText.Text = savedUsername;
                rememberUsernameCheckbox.Checked = true;
            }
        }
        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = usernameEditText.Text;
            string password = passwordEditText.Text;

            // Validate input fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Toast.MakeText(this, "Please enter username and password", ToastLength.Short).Show();
                return;
            }

            bool isAuthenticated = databaseHelper.AuthenticateUser(username, password);
            if (isAuthenticated)
            {
                // Save username preference after successful authentication
                _sessionManager.SaveUsername(username);

                // Save session state
                _sessionManager.SaveLoggedInState(true);

                Toast.MakeText(this, "Login successful", ToastLength.Short).Show();
                StartHomeScreenActivity();
            }
            else
            {
                Toast.MakeText(this, "Invalid username or password", ToastLength.Short).Show();
            }
        }
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }
        private void StartHomeScreenActivity()
        {
            Intent homeScreenIntent = new Intent(this, typeof(HomeScreenActivity));
            StartActivity(homeScreenIntent);
            Finish(); // Finish MainActivity so that pressing back doesn't return here
        }
    }
}