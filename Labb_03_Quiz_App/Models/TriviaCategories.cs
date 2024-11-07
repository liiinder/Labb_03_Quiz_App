using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Policy;

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
