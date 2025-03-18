using Interface.Properties;
using IQDHackathon.Themes;
using IQDHackathon;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Interface.Pages
{
    /// <summary>
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.Language == "EN")
            {
                rdEnglish.IsChecked = true;
            }
            else
            {
                rdArabic.IsChecked = true;
            }
            if (Settings.Default.Theme == "LightTheme")
            {
                rdLight.IsChecked = true;
            }
            else
            {
                rdDark.IsChecked = true;
            }
        }

        private void rdLight_Checked(object sender, RoutedEventArgs e)
        {
            ThemesController.SetTheme(ThemesController.ThemeTypes.Light);
        }

        private void rdDark_Checked(object sender, RoutedEventArgs e)
        {
            ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
        }

        private void rdEnglish_Checked(object sender, RoutedEventArgs e)
        {
            LanguageControler.SetLanguage(LanguageControler.enLanguage.English);
        }

        private void rdArabic_Checked(object sender, RoutedEventArgs e)
        {
            LanguageControler.SetLanguage(LanguageControler.enLanguage.Arabic);
        }

        private void rchecked_Checked(object sender, RoutedEventArgs e)
        {

           

        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();

        }
    }
}
