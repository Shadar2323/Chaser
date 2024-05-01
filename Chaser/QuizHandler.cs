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
using System.IO;

namespace Chaser
{
    internal class QuizHandler : TriviaHandler //המחלקה שעוזרת לנהל את החידון המהיר
    {
        public int currentRecord {get;set;}//השיא הנוכחי של השחקן
        public string username;//המשתמש שמשחק
        
        public QuizHandler(string userName): base()
        {
            questionList = GetQuestions();
            Player p = databaseHelper.GetCurrentPlayer(userName);
            currentRecord = p.Record;
            this.username = userName;
        }
        public List<QAndA> GetQuestions()
        {
            return databaseHelper.GetQuestions();
        }
        public bool IsRecordHigher(int record)
        {
            if (record>this.currentRecord)
            {
                databaseHelper.UpdateRecord(this.username, record);
                return true;
            }
            return false;
        }
    }
}