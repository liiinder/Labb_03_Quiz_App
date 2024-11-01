﻿using System.Collections;
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

        /// <summary>
        /// This method is for the selections on the listbox to be clicked again to deselect and just have one selected at all time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

//TODO: Make selection of listbox able to select more than one with ctrl...
// have the right form controll the last selected and delete will remove all.