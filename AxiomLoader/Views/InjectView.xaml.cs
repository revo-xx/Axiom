using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AxiomLoader.Services;
using Microsoft.Win32;

namespace AxiomLoader.Views
{
    public partial class InjectView : UserControl
    {
        public InjectView()
        {
            InitializeComponent();
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            string gameName = "Cheat";
            string processName = "";

            if (sender is Button btn && btn.Tag != null)
            {
                gameName = btn.Tag.ToString() ?? "Cheat";
            }

            // Map game names to process names
            switch (gameName)
            {
                case "Minecraft":
                    processName = "javaw"; // Minecraft Java Edition
                    break;
                case "CS2":
                    processName = "cs2";
                    break;
                case "Valorant":
                    processName = "VALORANT-Win64-Shipping";
                    break;
                default:
                    processName = "";
                    break;
            }

            if (gameName == "Minecraft")
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "DLL Files (*.dll)|*.dll";
                ofd.Title = "Select Minecraft Cheat DLL";

                if (ofd.ShowDialog() == true)
                {
                    string dllPath = ofd.FileName;
                    
                    StatusBorder.Visibility = Visibility.Visible;
                    StatusTitle.Text = $"Injecting into {gameName}...";
                    InjectionProgress.Value = 10;
                    StatusDetail.Text = "Waiting for process...";

                    await Task.Delay(1000);

                    string error;
                    bool success = Injector.Inject(processName, dllPath, out error);

                    if (success)
                    {
                        InjectionProgress.Value = 100;
                        StatusDetail.Text = "Successfully injected!";
                        MessageBox.Show($"{gameName} DLL successfully injected!", "Axiom Loader", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        StatusDetail.Text = "Injection failed.";
                        MessageBox.Show($"Error: {error}", "Injection Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    await Task.Delay(2000);
                    StatusBorder.Visibility = Visibility.Collapsed;
                }
                return;
            }

            // Mock injection for other games
            StatusBorder.Visibility = Visibility.Visible;
            StatusTitle.Text = $"Injecting {gameName}...";
            InjectionProgress.Value = 0;

            try
            {
                StatusDetail.Text = "Checking for updates...";
                await Task.Delay(1000);
                InjectionProgress.Value = 20;

                StatusDetail.Text = "Bypassing anti-cheat...";
                await Task.Delay(1500);
                InjectionProgress.Value = 50;

                StatusDetail.Text = "Downloading latest offsets...";
                await Task.Delay(1000);
                InjectionProgress.Value = 80;

                StatusDetail.Text = "Finalizing injection...";
                await Task.Delay(800);
                InjectionProgress.Value = 100;

                StatusDetail.Text = "Successfully injected!";
                MessageBox.Show($"Axiom {gameName} has been successfully injected!", "Axiom Loader", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                StatusDetail.Text = "Error: " + ex.Message;
                MessageBox.Show("An error occurred during injection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Optionally hide after some time
                await Task.Delay(2000);
                StatusBorder.Visibility = Visibility.Collapsed;
            }
        }
    }
}
