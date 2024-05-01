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
    public class Settings //מחלקה סינגלטונית שמטרתה לשמור את ההגדרות שנבחרו על ידי השחקן
    {
        // Properties
        public string Diff { get; set; }
        public int Duration { get; set; }

        // Singleton instance
        private static Settings instance;

        // Private constructor to prevent instantiation from outside
        private Settings() { }

        // Public method to get the instance or create it if it doesn't exist
        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Settings();
                }
                return instance;
            }
        }
    }
}