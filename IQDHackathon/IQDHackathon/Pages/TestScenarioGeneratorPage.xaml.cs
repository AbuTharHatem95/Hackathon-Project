using System.Windows;
using System.Windows.Controls;
using IQDHackathon;

namespace Interface.Pages   
{
    public partial class TestScenarioGeneratorPage : Page
    {
        public TestScenarioGeneratorPage()
        {
            InitializeComponent();
        }

        private void BackToMainWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }


        private void Gentet_Click(object sender, RoutedEventArgs e)
        {
            Content = new QuestionsPage();
        }
   }
}
