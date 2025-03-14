using Interface.Properties;
using System;
using System.Linq;
using System.Windows;


namespace IQDHackathon
{
    public static class LanguageControler
    {
        public enum enLanguage
        {
            English, Arabic
        }
        public static ResourceDictionary LanguageDictionary
        {
            get
            {
                return Application.Current.Resources.MergedDictionaries.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Languages/StringLanguage")) ?? new ResourceDictionary();
            }
            private set
            {
                if (LanguageDictionary != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(value);
                }
                Application.Current.Resources.MergedDictionaries.Add(value);
            }
        }
        public static void ChangeLanguage(string LanguageName)
        {
            try
            {
               LanguageDictionary = new ResourceDictionary() { Source = new Uri($"Languages/StringLanguage.{LanguageName}.xaml", UriKind.Relative) };
                Settings.Default.Language = LanguageName;
                Settings.Default.Save();

            }
            catch { }
        }
        public static void SetLanguage(enLanguage language)
        {
            switch (language)
            {
                case enLanguage.English: ChangeLanguage("EN"); break;
                case enLanguage.Arabic: ChangeLanguage("AR"); break;
            }
        }
    }
}
