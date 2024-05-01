using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace Chaser
{
    [Table ("QAndA")]
    public class QAndA //מחלקה שמתארת שאלה והתשובות שלה
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string question { get; set;} //השאלה עצמה      
        public string diff { get; set; }// רמת הקושי
        // SQLite-Net-PCL compatible property
        [Ignore] // Ignore the answers during SQLite operations
        public Answer[] answers { get; set; } //התשובות: במבנה נתונים נשמרות כשירשור ולכן בעת הכנסת העצם למבנה הנתונים התוכנה מתבקשת להתעלם מהעצם הזה
        public string SerializedAnswers //התשובות כשירשור סטרילי
        {
            get => JsonConvert.SerializeObject(answers);
            set => answers = JsonConvert.DeserializeObject<Answer[]>(value);
        }

        public QAndA(string question, Answer answer1, Answer answer2, Answer answer3, Answer answer4, string diff)
        {
            this.question = question;
            answers = new Answer[4];
            answers[0] = answer1;
            answers[1] = answer2;
            answers[2] = answer3;
            answers[3] = answer4;
            this.diff = diff;
        }
        public QAndA() { answers = new Answer[4]; }
    }
}