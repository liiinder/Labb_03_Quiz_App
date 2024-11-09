using System.Windows.Controls;

namespace Labb_03_Quiz_App.View
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class ConfigView : UserControl
    {
        public ConfigView()
        {
            InitializeComponent();
        }

        private void questions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox box)
            {
                if (e.AddedItems.Count > 0) box.ScrollIntoView(e.AddedItems[0]);
            }
        }
    }
}

//TODO: Look into change the listbox selection from single select to multi.
// Try out what feels best between have the first or the last selection when you multiselect to be the active question on the right side.
// And then make it able to select more than one with ctrl and remove all with the button / delete
// Dialog if you remove more than one question that you can cancle

//TODO: Move difficulty from QuestionPack to Question and add Combobox on each question then set QuestionPacks difficulty to the average