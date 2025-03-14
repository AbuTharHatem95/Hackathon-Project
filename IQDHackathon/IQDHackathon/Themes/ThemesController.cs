using Interface.Properties;
using System;
using System.Linq;
using System.Windows;


namespace IQDHackathon.Themes
{
    public static class ThemesController
    {
        

        public enum ThemeTypes
        {
            Light, Dark
        }

        public static ResourceDictionary ThemeDictionary
        {
            get
            {

                return Application.Current.Resources.MergedDictionaries.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Themes/"))?? new ResourceDictionary();
            }
            private set
            {

                if (ThemeDictionary != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(value);
                }


                Application.Current.Resources.MergedDictionaries.Add(value);

            }
        }

        public static void ChangeTheme(string ThemeName)
        {

            try
            {
                ThemeDictionary = new ResourceDictionary() { Source = new Uri($"Themes/{ThemeName}.xaml", UriKind.Relative) };
                Settings.Default.Theme = ThemeName;
                Settings.Default.Save();

            }
            catch { }

        }

        public static void SetTheme(ThemeTypes theme)
        {

            switch (theme)
            {
                case ThemeTypes.Dark: ChangeTheme("DarkTheme"); break;
                case ThemeTypes.Light: ChangeTheme("LightTheme"); break;
            }


        }
    }
}
