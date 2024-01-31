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
        SQLiteConnection database;
        const string tableName = "QAndA";
        public DatabaseHelper(string dbPath)
        {
            database = new SQLiteConnection(dbPath);
            if (!TableExists())
            {
                database.CreateTable<QAndA>();
                InitializeDatabase();
            }
        }
        void InitializeDatabase()
        {
            QAndA qAndA1 = new QAndA("What is your favorite color?", new Answer("Red", false), new Answer("Blue", true), new Answer("Green", false), new Answer("Yellow", false), "easy");
            QAndA qAndA2 = new QAndA("Which planet is known as the Red Planet?", new Answer("Mars", true), new Answer("Venus", false), new Answer("Jupiter", false), new Answer("Saturn", false), "easy");
            QAndA qAndA3 = new QAndA("What is the capital of France?", new Answer("Berlin", false), new Answer("Rome", false), new Answer("Paris", true), new Answer("Madrid", false), "easy");
            QAndA qAndA4 = new QAndA("Who wrote 'Romeo and Juliet'?", new Answer("Charles Dickens", false), new Answer("William Shakespeare", true), new Answer("Jane Austen", false), new Answer("Mark Twain", false), "easy");
            QAndA qAndA5 = new QAndA("What is the largest mammal on Earth?", new Answer("Elephant", false), new Answer("Blue Whale", true), new Answer("Giraffe", false), new Answer("Hippopotamus", false), "easy");
            SaveQAnda(qAndA1);
            SaveQAnda(qAndA2);
            SaveQAnda(qAndA3);
            SaveQAnda(qAndA4);
            SaveQAnda(qAndA5);
        }
        public List<QAndA> GetQuestions()
        {
            var questions = database.Table<QAndA>().ToList();
            foreach (var qAndA in questions)
            {
                qAndA.answers = JsonConvert.DeserializeObject<Answer[]>(qAndA.SerializedAnswers);
            }
            return questions;
        }
        public int SaveQAnda(QAndA q)
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

        public List<QAndA> GetQuestionsByDifficulty(string difficulty)
        {
            var questions = database.Table<QAndA>().Where(q => q.diff == difficulty).ToList();
            foreach (var qAndA in questions)
            {
                qAndA.answers = JsonConvert.DeserializeObject<Answer[]>(qAndA.SerializedAnswers);
            }
            return questions;
        }
        public bool TableExists()
        {
            var tableInfo = database.GetTableInfo(tableName);
            return tableInfo.Count > 0;
        }
    }
}