using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AxiomLoader.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                string colorCode = btn.Tag.ToString();
                Color color = (Color)ColorConverter.ConvertFromString(colorCode);
                
                // Update the resource in the main window
                if (Application.Current.MainWindow is MainWindow mainWin)
                {
                    mainWin.Resources["PrimaryColorBrush"] = new SolidColorBrush(color);
                }
            }
        }
    }
}
