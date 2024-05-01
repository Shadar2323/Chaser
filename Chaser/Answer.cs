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
    public class Answer //מחלקה המתארת תשובה לשאלה - 4 אפשרויות והאם האפשרות נכונה או לא
    {
        public string answerText { get; set; } //התשובה
        public bool isTrue { get; set; } //האם התשובה נכונה או לא?

        public Answer() { }
        public Answer(string answerText) { this.answerText = answerText; isTrue = false; }
        public Answer(string answerText, bool isTrue) {  this.answerText = answerText; this.isTrue = isTrue; }
    }
}