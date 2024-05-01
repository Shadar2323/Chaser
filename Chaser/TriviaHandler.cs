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
using Android.Icu.Text;

namespace Chaser
{
    public class TriviaHandler //מחלקת הבסיס של כל המשחקים
    {
        protected DatabaseHelper databaseHelper;//יוצר חיבור למבנה נתונים
        protected List<QAndA> questionList;//השאלות שישתמשו בהן במשחק
        protected List<QAndA> usedQuestions;//השאלות שכבר השתמשו (ששאלה לא תוצג פעמיים
        protected Random random; //בוחר שאלה רנדומלית      

        public TriviaHandler()
        {
            // Initialize members
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "trivia.db");
            databaseHelper = new DatabaseHelper(dbPath);
            
            questionList = new List<QAndA>();
            
            usedQuestions = new List<QAndA>();
            random = new Random();
        }
        public QAndA GetRandomQuestion()
        {
            // בודק אם כל השאלות שומשו - לא אמור לקרות
            if (usedQuestions.Count == questionList.Count)
            {
                usedQuestions.Clear();
                //אם במקרה קורה שהשחקן סיים את כל השאלות חוזר משתנה מיוחד עם טקסט ייחודי ותוכנית באקטיביטי תטפל בזה
                QAndA qAndA1 = new QAndA("Out of questions", new Answer("Red", false), new Answer("Blue", true), new Answer("Green", false), new Answer("Yellow", false), "easy");
                return qAndA1;
            }

            // Get a random index from the available questions
            int randomIndex;
            do
            {
                randomIndex = random.Next(questionList.Count);
            } while (usedQuestions.Contains(questionList[randomIndex]));

            // Mark the question as used
            usedQuestions.Add(questionList[randomIndex]);

            // Return the selected question
            return questionList[randomIndex];
        }
        public void RestartGame()
        {
            usedQuestions.Clear();
        }
    }
}