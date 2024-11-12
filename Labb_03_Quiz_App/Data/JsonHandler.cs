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

        //TODO: Setup a generalized Json Handler.
        // And make it less dependent on MainWindowViewModel

        public void SaveToFile(ObservableCollection<QuestionPackViewModel> packs)
        {
            string jsonString = JsonSerializer.Serialize(packs);
            File.WriteAllText(pathToFile, jsonString);
        }

        public async Task LoadPacks(MainWindowViewModel mainWindowVM)
        {
            Directory.CreateDirectory(pathToFolder);
            if (!File.Exists(pathToFile)) File.WriteAllText(pathToFile, "[]");

            string json = await File.ReadAllTextAsync(pathToFile);

            var loadedPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(json);
            foreach (QuestionPackViewModel pack in loadedPacks) mainWindowVM.Packs.Add(pack);
            if (mainWindowVM.ActivePack is null && mainWindowVM.Packs.Count > 0) mainWindowVM.ActivePack = mainWindowVM.Packs[^1];

            mainWindowVM.LoadedPacks = true;
            mainWindowVM.RaisePropertyChanged("NoPacks");
        }
    }
}
