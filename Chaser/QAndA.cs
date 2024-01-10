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
    public class QAndA
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string question { get; set;}       
        public string diff { get; set; }
        // SQLite-Net-PCL compatible property
        [Ignore] // Ignore the answers during SQLite operations
        public Answer[] answers { get; set; }
        public string SerializedAnswers
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