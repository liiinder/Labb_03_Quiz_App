using Labb_03_Quiz_App.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Labb_03_Quiz_App.Dialogs
{
    /// <summary>
    /// Interaction logic for Fetch.xaml
    /// </summary>
    public partial class FetchQuestionsDialog : Window
    {
        public FetchQuestionsDialog()
        {
            InitializeComponent();

            DataContext = (App.Current.MainWindow as MainWindow)?.DataContext;
            Owner = App.Current.MainWindow;
        }
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e) => Close();

        private async void Import_Categories(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowVM)
            {
                await mainWindowVM.ImportCategories();
            }
        }
        private async void Import_Questions(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowVM)
            {
                await mainWindowVM.ImportQuestions();
                Close();
            }
        }
    }
}

//TODO: Implement ImportQuestions, Currently not fully in use...


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