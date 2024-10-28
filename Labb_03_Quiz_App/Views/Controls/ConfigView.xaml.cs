using Labb_03_Quiz_App.View.Windows;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Labb_03_Quiz_App.View.Controls
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

        private void Pack_Options(object sender, RoutedEventArgs e)
        {
            PackOptions pack = new();

            pack.ShowDialog();
        }

        private void questions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox box)
            {
                if (e.AddedItems.Count > 0)
                {
                    var last = e.AddedItems[0];
                    foreach (var item in new ArrayList(box.SelectedItems))
                    {
                        if (item != last) box.SelectedItems.Remove(item);
                    }
                }
            }
        }
    }
}
