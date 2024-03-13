using Android.App;
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

        private const string RememberUsernameKey = "RememberUsername";
        private const string UsernameKey = "Username";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.logIn);

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "trivia.db");
            databaseHelper = new DatabaseHelper(dbPath);

            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            loginButton = FindViewById<Button>(Resource.Id.loginButton);
            registerButton = FindViewById<Button>(Resource.Id.registerButton);
            rememberUsernameCheckbox = FindViewById<CheckBox>(Resource.Id.rememberUsernameCheckbox);

            loginButton.Click += LoginButton_Click;
            registerButton.Click += RegisterButton_Click;
            // Load remembered username
            LoadRememberedUsername();
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

            // Authenticate user
            bool isAuthenticated = databaseHelper.AuthenticateUser(username, password);
            if (isAuthenticated)
            {
                // Save username preference after successful authentication
                SaveUsernamePreference(username);

                Toast.MakeText(this, "Login successful", ToastLength.Short).Show();
                Intent homeScreenIntent = new Intent(this, typeof(HomeScreenActivity));
                StartActivity(homeScreenIntent);
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
        private void LoadRememberedUsername()
        {
            var prefs = GetSharedPreferences("LoginPrefs", FileCreationMode.Private);
            bool rememberUsername = prefs.GetBoolean(RememberUsernameKey, false);
            if (rememberUsername)
            {
                string savedUsername = prefs.GetString(UsernameKey, "");
                usernameEditText.Text = savedUsername;
                rememberUsernameCheckbox.Checked = true;
            }
        }

        private void SaveUsernamePreference(string username)
        {
            var prefs = GetSharedPreferences("LoginPrefs", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutBoolean("RememberUsername", rememberUsernameCheckbox.Checked);
            if (rememberUsernameCheckbox.Checked)
            {
                editor.PutString("Username", username);
            }
            else
            {
                editor.Remove("Username");
            }
            editor.Apply(); // Use Apply instead of Commit for asynchronous saving
        }
    }
}