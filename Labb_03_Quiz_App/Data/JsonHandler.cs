using Labb_03_Quiz_App.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace Labb_03_Quiz_App.Data
{
    internal class JsonHandler
    {
        private static string pathToFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Labb_03_Quiz_App");
        private static string pathToFile = Path.Combine(pathToFolder, "QuestionPacks.json");
        // Setup a generalized Json Handler.

        public void SaveToFile(ObservableCollection<QuestionPackViewModel> packs)
        {
            string jsonString = JsonSerializer.Serialize(packs);
            File.WriteAllText(pathToFile, jsonString);
        }

        //public async Task LoadPacks()
        //{
        //    Directory.CreateDirectory(pathToFolder);
        //    if (!File.Exists(pathToFile)) File.WriteAllText(pathToFile, "[]");

        //    string json = await File.ReadAllTextAsync(pathToFile);

        //    var loadedPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(json);
        //    foreach (QuestionPackViewModel pack in loadedPacks) Packs.Add(pack);
        //    if (ActivePack is null && Packs.Count > 0) ActivePack = Packs[^1];
        //}

        //TODO:  ??? hur ska jag göra med översta...
        //TODO: Anropa en metod som anropar tasksen eller ?
    }
}
