using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Interface.Pages;

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

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                border.RenderTransform = new ScaleTransform(1.05, 1.05);
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                border.RenderTransform = new ScaleTransform(1, 1);
            }
        }


        private void OpenPresentationGenerator(object sender, MouseButtonEventArgs e)
        {
            Content = new PresentationGeneratorPage();
        }

        private void OpenReportAssistant(object sender, MouseButtonEventArgs e)
        {
            Content = new ReportAssistantPage();
        }

        private void OpenTestScenarioGenerator(object sender, MouseButtonEventArgs e)
        {
            Content = new TestScenarioGeneratorPage();
        }

        private void OpenUIDesigner(object sender, MouseButtonEventArgs e)
        {
            Content = new UIDesignerPage();
        }

        private void OpenStoryCharacterGenerator(object sender, MouseButtonEventArgs e)
        {
            Content = new StoryCharacterPage();
        }

        private void OpenFinancialAnalyzer(object sender, MouseButtonEventArgs e)
        {
            Content = new FinancialAnalyzerPage();
        }

        private void OpenEducationMaterialGenerator(object sender, MouseButtonEventArgs e)
        {
            Content = new EducationMaterialPage();
        }

        private void OpenMaintenanceAssistant(object sender, MouseButtonEventArgs e)
        {
            Content = new MaintenanceAssistantPage();
        }

        private void OpenImageGenerator(object sender, MouseButtonEventArgs e)
        {
            Content = new ImageGeneratorPage();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}