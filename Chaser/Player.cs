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
using SQLite;
using AndroidX.ProfileInstallers;

namespace Chaser
{
    [Table("Players")]
    public class Player //מחלקה המתארת את השחקן:
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; } //שם משתמש
        public string Password { get; set; }// סיסמה
        public int Record { get; set; }//שיא תשובות נכונות בחידון מהיר
        public string ProfileImage {get;set;}//שירשור המתאר את המסלול למיקום של התמונת פרופיל של השחקן
        // Default profile image path

        // Parameterless constructor
        public Player()
        {
        }

        // Constructor with parameters
        public Player(string userName, string password)
        {
            UserName = userName;
            Password = password;
            Record = 0;
            ProfileImage = null; 
        }
    }
}