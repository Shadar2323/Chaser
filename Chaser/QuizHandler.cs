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
    internal class QuizHandler: TriviaHandler
    {
        private int count = 0;
        private int record;
        public QuizHandler(): base()
        {
            questionList = GetQuestions(); 
            //record = database.Record;
        }
        public List<QAndA> GetQuestions()
        {
            return databaseHelper.GetQuestions();
        }

    }
}