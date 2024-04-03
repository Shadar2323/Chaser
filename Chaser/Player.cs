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

namespace Chaser
{
    [Table("Players")]
    public class Player
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Record { get; set; }

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
        }
    }
}