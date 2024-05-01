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
    public class SessionManager //מחלקה שעוזרת לנהל את ה SP
    {
        // Constants for keys used in SharedPreferences
        private const string SessionKey = "IsLoggedIn";
        private const string UsernameKey = "Username";

        // עצם כדי לשמור את המידע הנשמר
        private readonly ISharedPreferences _prefs;

        public SessionManager(ISharedPreferences prefs)
        {
            _prefs = prefs;
        }

        // פעולה הבודקת האם משתמש מחובר ואפשר לדלג על שלב ההתחברות
        public bool IsLoggedIn()
        {
            // מחזיר את הערך הבוליאני המזוהה עם המפתח, מחזיר שקר אלא אם כן נאמר אחרת במפתח
            return _prefs.GetBoolean(SessionKey, false);
        }

        // פעולה השומרת אם משתמש מחובר
        public void SaveLoggedInState(bool isLoggedIn)
        {
            // יוצר עורך כדי לערוך את התוכן הנשמר
            var editor = _prefs.Edit();
            // מכניס האם המשתמש בחר לזכור שישאר מחובר או לא
            editor.PutBoolean(SessionKey, isLoggedIn);
            editor.Apply();
        }

        // פעולה המחזירה את שם המשתמש שמחובר כרגע
        public string GetSavedUsername()
        {
            return _prefs.GetString(UsernameKey, "");
        }

        // שומר את המשתמש שהתחבר
        public void SaveUsername(string username)
        {
            var editor = _prefs.Edit();
            editor.PutString(UsernameKey, username);
            editor.Apply();
        }

        //מוחק את כל הנתונים
        public void ClearSession()
        {
            var editor = _prefs.Edit();
            editor.Remove(UsernameKey);
            editor.Remove(SessionKey);
            editor.Apply();
        }
    }
}