using System.Reflection;
using System.Windows;

namespace Labb_03_Quiz_App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() { SetDropDownMenuToBeRightAligned(); }

        /// <summary>
        /// This method is to make the menus to force open to the right instead of the left, solution taken from:
        /// https://stackoverflow.com/questions/37326546/why-are-my-menus-opening-right-to-left-instead-of-left-to-right
        /// </summary>
        private static void SetDropDownMenuToBeRightAligned()
        {
            var menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            Action setAlignmentValue = () =>
            {
                if (SystemParameters.MenuDropAlignment && menuDropAlignmentField != null) menuDropAlignmentField.SetValue(null, false);
            };

            setAlignmentValue();

            SystemParameters.StaticPropertyChanged += (sender, e) => { setAlignmentValue(); };
        }
    }

}
