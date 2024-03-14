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
    public class SessionManager
    {
        private const string SessionKey = "IsLoggedIn";
        private const string UsernameKey = "Username";

        private readonly ISharedPreferences _prefs;

        public SessionManager(ISharedPreferences prefs)
        {
            _prefs = prefs;
        }

        public bool IsLoggedIn()
        {
            return _prefs.GetBoolean(SessionKey, false);
        }

        public void SaveLoggedInState(bool isLoggedIn)
        {
            var editor = _prefs.Edit();
            editor.PutBoolean(SessionKey, isLoggedIn);
            editor.Apply();
        }

        public string GetSavedUsername()
        {
            return _prefs.GetString(UsernameKey, "");
        }

        public void SaveUsername(string username)
        {
            var editor = _prefs.Edit();
            editor.PutString(UsernameKey, username);
            editor.Apply();
        }

        public void ClearSession()
        {
            var editor = _prefs.Edit();
            editor.Clear();
            editor.Apply();
        }
    }
}