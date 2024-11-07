using System.Windows;

namespace Labb_03_Quiz_App.View.Windows
{
    /// <summary>
    /// Interaction logic for Fetch.xaml
    /// </summary>
    public partial class Fetch : Window
    {
        public Fetch()
        {
            InitializeComponent();
        }
    }
}

//TODO: Currently not in use... Implement this.


// https://opentdb.com/api_config.php

// https://opentdb.com/api.php?amount=10&category=31&difficulty=easy&type=multiple

//type: "multiple",
//difficulty: "easy",
//category: "Entertainment: Japanese Anime &amp; Manga",
//question: "Who was a voice actor for the English dubbing of HOWL&#039;S MOVING CASTLE (2004)?",
//correct_answer: "Christian Bale",
//incorrect_answers: [
//        "Matt Damon",
//        "Joseph Gordon-Levitt",
//        "Willem Dafoe"
//    ]