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
    public class GameHandler : TriviaHandler
    {
        // Additional members specific to GameHandler
        private int moveAnimation;
        private int playerPlacement;
        private int chaserPlacement;
        private int botCorrectnessProbability;
        private string diff;
        private Settings settings;
        public GameHandler() : base()
        {
            chaserPlacement = 7;
            questionList = setQuestionsList();
            settings = Settings.Instance;
            diff = settings.Diff;
            // The base class constructor (TriviaHandler constructor) is called using "base()"
            // No need to reinitialize members already initialized in TriviaHandler

            if (diff == "hard")
            {
                // Additional initialization specific to GameHandler for "hard" difficulty
                playerPlacement = 6;
                moveAnimation = 90;
            }
            if (diff == "medium")
            {
                // Additional initialization specific to GameHandler for "medium" difficulty
                moveAnimation = 100;
                playerPlacement = 5;
            }
            if (diff == "easy")
            {
                // Additional initialization specific to GameHandler for "easy" difficulty
                moveAnimation = 115;
                playerPlacement = 4;
            }
        }
        public List<QAndA> setQuestionsList()
        {
            return databaseHelper.GetQuestionsByDifficulty(diff);
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