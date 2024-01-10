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

namespace Chaser
{
    [Table("Answers")]
    public class Answer
    {
        public string answerText { get; set; }
        public bool isTrue { get; set; }

        public Answer() { }
        public Answer(string answerText) { this.answerText = answerText; isTrue = false; }
        public Answer(string answerText, bool isTrue) {  this.answerText = answerText; this.isTrue = isTrue; }
    }
}