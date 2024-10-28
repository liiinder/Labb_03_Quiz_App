using System.Collections.ObjectModel;

namespace Labb_3_Quiz_App.Models
{
    internal class TriviaCategories
    {
        public ObservableCollection<Category> listOfCategories = new();

        public void Load()
        {
            //TODO: fetch categories from ... https://opentdb.com/api_category.php
        }
    }
}
