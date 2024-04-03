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
    internal class QuizHandler : TriviaHandler
    {
        public int currentRecord {get;set;}
        public string username;
        
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