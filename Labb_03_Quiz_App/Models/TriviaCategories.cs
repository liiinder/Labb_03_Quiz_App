using System.Collections.ObjectModel;

namespace Labb_03_Quiz_App.Models
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

//TODO: Currently not in use... Implement this.