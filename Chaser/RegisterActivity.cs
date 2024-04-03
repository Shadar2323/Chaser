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
using System.IO;
namespace Chaser
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        private EditText usernameEditText;
        private EditText passwordEditText;
        private Button registerButton;
        private DatabaseHelper databaseHelper;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.register);
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "trivia.db");
            databaseHelper = new DatabaseHelper(dbPath);

            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            registerButton = FindViewById<Button>(Resource.Id.registerButton);

            registerButton.Click += RegisterButton_Click;
        }
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string username = usernameEditText.Text;
            string password = passwordEditText.Text;

            // Validate input fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Toast.MakeText(this, "Please enter username and password", ToastLength.Short).Show();
                return;
            }

            // Register user
            bool success = databaseHelper.RegisterUser(username, password);
            if (success)
            {
                Toast.MakeText(this, "Registration successful", ToastLength.Short).Show();
                Intent mainActivityIntent = new Intent(this, typeof(MainActivity));
                StartActivity(mainActivityIntent);
            }
            else
            {
                Toast.MakeText(this, "Username already exists", ToastLength.Short).Show();
            }
        }
    }
}