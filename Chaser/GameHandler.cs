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
    public class GameHandler
    {
        DatabaseHelper databaseHelper;
        private List<QAndA> questionList;
        private List<QAndA> usedQuestions;
        private Random random;
        string diff;
        Settings settings;
        int moveAnimation;
        int playerPlacement;
        int chaserPlacement;
        int botCorrectnessProbability;
        public int trueAnswer { get; set; }
        public GameHandler()
        {
            //need to add the static database of the questions
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "trivia.db");
            databaseHelper = new DatabaseHelper(dbPath);
            chaserPlacement = 7;
            
            random = new Random();
            settings = Settings.Instance;
            diff = settings.Diff;
            questionList = new List<QAndA>();
            questionList = setQuestionsList();
            usedQuestions = new List<QAndA>();
            
            if (diff == "hard")
            {
                //מחזיר אוסף שאלות קשות
                playerPlacement = 6;
                moveAnimation = 90;
            }
            if (diff == "medium")
            {
                //מחזיר אוסף שאלות בינוניות
                moveAnimation = 100;
                playerPlacement = 5;
            }
            if (diff == "easy")
            {
                //מחזיר אוסף שאלות קלות
                moveAnimation = 115;
                playerPlacement = 4;
            }
        }

        public int GetDuration()
        {
            return settings.Duration;
        }
        public string GetDiff()
        {
            return diff;
        }
        public int GetMoveAnimation()
        {           
            return moveAnimation; 
        }
        public QAndA GetRandomQuestion()
        {
            // Check if all questions have been used, if so, reset the used questions list
            if (usedQuestions.Count == questionList.Count)
            {
                usedQuestions.Clear();
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

        public List<QAndA> setQuestionsList()
        {
            return databaseHelper.GetQuestionsByDifficulty(diff);
        }
        public int answeredCorrectly()
        {
            playerPlacement--;
            bool chaserCorrect = chaserResault();
            if (playerPlacement==0)
            {
                if (chaserCorrect) //השחקן ניצח והצייסר צדק
                {
                    return 1;
                }
                return 2;// השחקן רק ניצח הצייסר טעה
            }
            else if (chaserCorrect) { chaserPlacement--; return 3; }
            return 4;//השחקן רק ענה נכון
        }
        public int answeredInCorrectly()
        {
            bool chaserCorrect = chaserResault();
            
            if (chaserCorrect)
            {
                chaserPlacement -= 1;
                if (chaserPlacement==playerPlacement)
                {
                    return 1;//הצ'ייסר ניצח
                }
                return 2;//הצ'ייסר ענה נכון אך עדיין לא תפס את השחקן
            }
            return 3;//הצ'ייסר לא ענה נכון - לא לשנות כלום
        }
        public bool chaserResault()
        {
            // Set bot correctness probability based on user difficulty
            switch (diff)
            {
                case "easy":
                    botCorrectnessProbability = 30; // Adjust as needed
                    break;
                case "medium":
                    botCorrectnessProbability = 20; // Adjust as needed
                    break;
                case "hard":
                    botCorrectnessProbability = 10; // Adjust as needed
                    break;
                default:
                    throw new ArgumentException("Invalid difficulty level");
            }

            // Simulate bot correctness based on probability
            Random random = new Random();
            int randomNumber = random.Next(0, 100); // Generate a random number between 0 and 99

            return randomNumber > botCorrectnessProbability;
        }
    }
}