using System.Windows;
using Interface.Properties;
using IQDHackathon;
using IQDHackathon.Themes;

namespace IQDHackathon
{

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string savedLang = Settings.Default.Language;
            if (!string.IsNullOrEmpty(savedLang))
            {
                if (savedLang != "EN")
                {
                    LanguageControler.ChangeLanguage(savedLang);
                }
            }

            string savedTheme = Settings.Default.Theme;
            if (!string.IsNullOrEmpty(savedTheme))
            {
                if (savedTheme != "LightTheme")
                {
                    ThemesController.ChangeTheme(savedTheme);
                }
            }
        }
    }
}
