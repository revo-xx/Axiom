using System.Windows;
using System.Windows.Controls;

namespace AxiomLoader.Views
{
    public partial class InjectView : UserControl
    {
        public InjectView()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            string gameName = "Cheat";
            if (sender is Button btn && btn.Tag != null)
            {
                gameName = btn.Tag.ToString() ?? "Cheat";
            }
            MessageBox.Show($"Axiom {gameName} is being injected...", "Axiom Loader", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
