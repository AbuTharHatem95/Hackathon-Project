using System.Windows;
using System.Windows.Controls;
using IQDHackathon;

namespace Interface.Pages
{
    public partial class EducationMaterialPage : Page
    {
        public EducationMaterialPage()
        {
            InitializeComponent();
        }

        private void BackToMainWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }
    }
}
