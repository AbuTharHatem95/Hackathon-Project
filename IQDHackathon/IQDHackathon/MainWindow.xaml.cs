
using System.Windows;
using System.Windows.Controls;

namespace IQDHackathon
{
    public partial class MainWindow : Window
    {
        public static Frame? MainFrameInstance { get; private set; }
        private enum enLanguage { English, Arabic }

        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LanguageControler.SetLanguage(LanguageControler.enLanguage.English);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LanguageControler.SetLanguage(LanguageControler.enLanguage.English);
        }

        private void rdSettings_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}