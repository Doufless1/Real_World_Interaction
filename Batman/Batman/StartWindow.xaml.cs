using System;
using System.Windows;
using System.Windows.Controls;
using Batman.ViewModels;
namespace Batman
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();

        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
