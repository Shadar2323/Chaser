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
    public class TriviaHandler
    {
        protected DatabaseHelper databaseHelper;
        protected List<QAndA> questionList;
        protected List<QAndA> usedQuestions;
        protected Random random;        

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
            // Check if all questions have been used, if so, reset the used questions list
            if (usedQuestions.Count == questionList.Count)
            {
                usedQuestions.Clear();
                //צריך להוריד
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