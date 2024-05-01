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
using SQLite;
using System.IO;
using Android.Graphics;
using Newtonsoft.Json;
using static Android.Content.ClipData;

namespace Chaser
{
    public class DatabaseHelper
    {
        private readonly SQLiteConnection database; //מקשר לבסיס הנתונים
        const string questionTableName = "QAndA";
        const string playerTableName = "Players";
        public DatabaseHelper(string dbPath) //הפעולה בודקת אם הטבלאות כבר נוצרו ויוצרת את הטבלאות בהתאם לכך
        {
            database = new SQLiteConnection(dbPath);
            if (!TableExists(questionTableName))
            {
                database.CreateTable<QAndA>();
                InitializeDatabase();
            }
            if (!TableExists(playerTableName))
            {
                database.CreateTable<Player>();
            }
        }
        void InitializeDatabase() //פעולה המוסיפה את כל השאלות במקרה בו הטבלה ריקה
        {
            //easy questions==>
            QAndA qAndA1 = new QAndA("What is your favorite color?", new Answer("Red", false), new Answer("Blue", true), new Answer("Green", false), new Answer("Yellow", false), "easy");
            QAndA qAndA2 = new QAndA("Which planet is known as the Red Planet?", new Answer("Mars", true), new Answer("Venus", false), new Answer("Jupiter", false), new Answer("Saturn", false), "easy");
            QAndA qAndA3 = new QAndA("What is the capital of France?", new Answer("Berlin", false), new Answer("Rome", false), new Answer("Paris", true), new Answer("Madrid", false), "easy");
            QAndA qAndA4 = new QAndA("Who wrote 'Romeo and Juliet'?", new Answer("Charles Dickens", false), new Answer("William Shakespeare", true), new Answer("Jane Austen", false), new Answer("Mark Twain", false), "easy");
            QAndA qAndA5 = new QAndA("What is the largest mammal on Earth?", new Answer("Elephant", false), new Answer("Blue Whale", true), new Answer("Giraffe", false), new Answer("Hippopotamus", false), "easy");
            QAndA qAndA6 = new QAndA("What is the largest ocean on Earth?", new Answer("Pacific Ocean", true), new Answer("Atlantic Ocean", false), new Answer("Indian Ocean", false), new Answer("Arctic Ocean", false), "easy");
            QAndA qAndA7 = new QAndA("Who painted the Mona Lisa?", new Answer("Leonardo da Vinci", true), new Answer("Pablo Picasso", false), new Answer("Vincent van Gogh", false), new Answer("Michelangelo", false), "easy");
            QAndA qAndA8 = new QAndA("What is the chemical symbol for water?", new Answer("H2O", true), new Answer("Wa", false), new Answer("Wt", false), new Answer("H2", false), "easy");
            QAndA qAndA9 = new QAndA("Which animal is known as the 'King of the Jungle'?", new Answer("Lion", true), new Answer("Elephant", false), new Answer("Tiger", false), new Answer("Gorilla", false), "easy");
            QAndA qAndA10 = new QAndA("What is the main ingredient in guacamole?", new Answer("Avocado", true), new Answer("Tomatoes", false), new Answer("Onions", false), new Answer("Peppers", false), "easy");

            SaveQAnda(qAndA1);
            SaveQAnda(qAndA2);
            SaveQAnda(qAndA3);
            SaveQAnda(qAndA4);
            SaveQAnda(qAndA5);
            SaveQAnda(qAndA6);
            SaveQAnda(qAndA7);
            SaveQAnda(qAndA8);
            SaveQAnda(qAndA9);
            SaveQAnda(qAndA10);

            //medium questions==>
            QAndA qAndA11 = new QAndA("In which year did the Titanic sink?", new Answer("1912", true), new Answer("1914", false), new Answer("1910", false), new Answer("1916", false), "medium");
            QAndA qAndA12 = new QAndA("Who painted the 'Starry Night'?", new Answer("Vincent van Gogh", true), new Answer("Leonardo da Vinci", false), new Answer("Pablo Picasso", false), new Answer("Michelangelo", false), "medium");
            QAndA qAndA13 = new QAndA("Which country is known as the Land of the Rising Sun?", new Answer("Japan", true), new Answer("China", false), new Answer("Korea", false), new Answer("Vietnam", false), "medium");
            QAndA qAndA14 = new QAndA("What is the chemical symbol for gold?", new Answer("Au", true), new Answer("Ag", false), new Answer("Fe", false), new Answer("Cu", false), "medium");
            QAndA qAndA15 = new QAndA("Who was the first woman to win a Nobel Prize?", new Answer("Marie Curie", true), new Answer("Mother Teresa", false), new Answer("Rosa Parks", false), new Answer("Jane Goodall", false), "medium");
            QAndA qAndA16 = new QAndA("What is the largest desert in the world?", new Answer("Sahara Desert", true), new Answer("Arabian Desert", false), new Answer("Gobi Desert", false), new Answer("Antarctic Desert", false), "medium");
            QAndA qAndA17 = new QAndA("Who was the first president of the United States?", new Answer("George Washington", true), new Answer("Thomas Jefferson", false), new Answer("Abraham Lincoln", false), new Answer("John Adams", false), "medium");
            QAndA qAndA18 = new QAndA("Which element is said to be essential for life on Earth?", new Answer("Carbon", true), new Answer("Oxygen", false), new Answer("Hydrogen", false), new Answer("Nitrogen", false), "medium");
            QAndA qAndA19 = new QAndA("What is the tallest mountain in the world?", new Answer("Mount Everest", true), new Answer("K2", false), new Answer("Kangchenjunga", false), new Answer("Lhotse", false), "medium");
            QAndA qAndA20 = new QAndA("Which famous physicist developed the theory of relativity?", new Answer("Albert Einstein", true), new Answer("Isaac Newton", false), new Answer("Galileo Galilei", false), new Answer("Stephen Hawking", false), "medium");

            SaveQAnda(qAndA11);
            SaveQAnda(qAndA12);
            SaveQAnda(qAndA13);
            SaveQAnda(qAndA14);
            SaveQAnda(qAndA15);
            SaveQAnda(qAndA16);
            SaveQAnda(qAndA17);
            SaveQAnda(qAndA18);
            SaveQAnda(qAndA19);
            SaveQAnda(qAndA20);

            //hard questions==>
            QAndA qAndA21 = new QAndA("Who developed the first successful polio vaccine?", new Answer("Jonas Salk", true), new Answer("Louis Pasteur", false), new Answer("Edward Jenner", false), new Answer("Alexander Fleming", false), "hard");
            QAndA qAndA22 = new QAndA("What is the smallest bone in the human body?", new Answer("Stapes", true), new Answer("Femur", false), new Answer("Tibia", false), new Answer("Radius", false), "hard");
            QAndA qAndA23 = new QAndA("Which philosopher wrote 'Thus Spoke Zarathustra'?", new Answer("Friedrich Nietzsche", true), new Answer("Socrates", false), new Answer("Plato", false), new Answer("Aristotle", false), "hard");
            QAndA qAndA24 = new QAndA("In which year did the French Revolution begin?", new Answer("1789", true), new Answer("1776", false), new Answer("1804", false), new Answer("1798", false), "hard");
            QAndA qAndA25 = new QAndA("Who composed 'The Four Seasons'?", new Answer("Antonio Vivaldi", true), new Answer("Wolfgang Amadeus Mozart", false), new Answer("Ludwig van Beethoven", false), new Answer("Johann Sebastian Bach", false), "hard");
            QAndA qAndA26 = new QAndA("What is the chemical formula for table salt?", new Answer("NaCl", true), new Answer("H2O", false), new Answer("CO2", false), new Answer("CaCO3", false), "hard");
            QAndA qAndA27 = new QAndA("Which Roman emperor built the Colosseum?", new Answer("Vespasian", true), new Answer("Augustus", false), new Answer("Nero", false), new Answer("Trajan", false), "hard");
            QAndA qAndA28 = new QAndA("Who discovered penicillin?", new Answer("Alexander Fleming", true), new Answer("Louis Pasteur", false), new Answer("Jonas Salk", false), new Answer("Edward Jenner", false), "hard");
            QAndA qAndA29 = new QAndA("Which famous composer was deaf?", new Answer("Ludwig van Beethoven", true), new Answer("Johann Sebastian Bach", false), new Answer("Wolfgang Amadeus Mozart", false), new Answer("Antonio Vivaldi", false), "hard");
            QAndA qAndA30 = new QAndA("Who wrote '1984'?", new Answer("George Orwell", true), new Answer("Aldous Huxley", false), new Answer("Ray Bradbury", false), new Answer("Margaret Atwood", false), "hard");

            SaveQAnda(qAndA21);
            SaveQAnda(qAndA22);
            SaveQAnda(qAndA23);
            SaveQAnda(qAndA24);
            SaveQAnda(qAndA25);
            SaveQAnda(qAndA26);
            SaveQAnda(qAndA27);
            SaveQAnda(qAndA28);
            SaveQAnda(qAndA29);
            SaveQAnda(qAndA30);

        }
        public List<QAndA> GetQuestions() //פעולה המחזירה את כל השאלות תוך די-סטרילזציה של התשובות
        {
            var questions = database.Table<QAndA>().ToList();
            foreach (var qAndA in questions)
            {
                qAndA.answers = JsonConvert.DeserializeObject<Answer[]>(qAndA.SerializedAnswers);
            }
            return questions;
        }
        public int SaveQAnda(QAndA q)//מעדכנת שאלות חדשות בטבלה
        {
            if (q.Id != 0)
            {
                return database.Update(q);
            }
            else
            {
                return database.Insert(q);
            }
        }
        public List<QAndA> GetQuestionsByDifficulty(string difficulty)//לקיחת כל השאלות ברמת קושי מסוימת
        {
            var questions = database.Table<QAndA>().Where(q => q.diff == difficulty).ToList();
            foreach (var qAndA in questions)
            {
                qAndA.answers = JsonConvert.DeserializeObject<Answer[]>(qAndA.SerializedAnswers);
            }
            return questions;
        }
        public bool TableExists(string name)//פעולה הבודקת אם טבלה נוצרה או לא
        {
            var tableInfo = database.GetTableInfo(name);
            return tableInfo.Count > 0;
        }
        public bool RegisterUser(string userName, string password)//פעולה המוסיפה משתמש למבנה נתונים
        {
            if (!database.Table<Player>().Any(x => x.UserName == userName))
            {
                var player = new Player (userName,password);
                database.Insert(player);
                return true;
            }
            return false; // Username already exists
        }
        public bool AuthenticateUser(string userName, string password) //בודקת האם המשתמש עם הסיסמה נמצא במערכת
        {
            return database.Table<Player>().Any(x => x.UserName == userName && x.Password == password);
        }
        public void UpdateRecord(string userName, int newRecord)
        {
            var player = database.Table<Player>().FirstOrDefault(x => x.UserName == userName);
            if (player != null)
            {
                player.Record = newRecord;
                database.Update(player);
            }
        }
        public void UpdateImageProfile(string userName, string profileImage) //מעדכנת תמונת פרופיל
        {
            var player = database.Table<Player>().FirstOrDefault(x => x.UserName == userName);
            if (player != null)
            {
                player.ProfileImage = profileImage;
                database.Update(player);
            }
        }
        public bool UpdateUserName(string userName, string newUserName) //מעדכנת תמונת פרופיל
        {
            if (!database.Table<Player>().Any(x => x.UserName == newUserName))
            {
                var player = database.Table<Player>().FirstOrDefault(x => x.UserName == userName);
                if (player != null)
                {
                    player.UserName = newUserName;
                    database.Update(player);
                    return true;
                }
            }
            return false;
                
        }
        public Player GetCurrentPlayer(string userName) //מחזירה את השם משתמש בעל אותו שם
        {
            Player player = database.Table<Player>().Where(player => player.UserName == userName).FirstOrDefault();
            return player;
        }
    }
}