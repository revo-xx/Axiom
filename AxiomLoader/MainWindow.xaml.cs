using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using AxiomLoader.Views;

namespace AxiomLoader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NewsView _newsView = new NewsView();
        private InjectView _injectView = new InjectView();
        private ProfileView _profileView = new ProfileView();
        private SettingsView _settingsView = new SettingsView();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            // Simple mock login logic
            if (username == "admin" && password == "admin")
            {
                StatusText.Text = "";
                LoginPanel.Visibility = Visibility.Collapsed;
                MainPanel.Visibility = Visibility.Visible;
                ShowView(_newsView);
            }
            else
            {
                StatusText.Text = "Invalid username or password!";
            }
        }

        private void ShowView(UserControl view)
        {
            ViewContainer.Content = view;
            Storyboard sb = (Storyboard)this.FindResource("FadeInAnimation");
            sb.Begin(view);
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                string target = btn.Tag.ToString() ?? "";

                switch (target)
                {
                    case "News":
                        ShowView(_newsView);
                        break;
                    case "Inject":
                        ShowView(_injectView);
                        break;
                    case "Profile":
                        ShowView(_profileView);
                        break;
                    case "Settings":
                        ShowView(_settingsView);
                        break;
                }
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
            UsernameBox.Clear();
            PasswordBox.Clear();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // This is now handled in InjectView.xaml.cs
        }
    }
}