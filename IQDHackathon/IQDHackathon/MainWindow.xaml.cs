using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Interface.Pages;
using IQD_UI_Library;

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

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Border border = (Border)sender;
            if (border != null)
            {
                border.RenderTransform = new ScaleTransform(1.05, 1.05);
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Border border = (Border)sender;
            if (border != null)
            {
                border.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        //private void OpenPresentationGenerator(object sender, MouseButtonEventArgs e)
        //{
        //    Content = new PresentationGeneratorPage();
        //}

        //private void OpenReportAssistant(object sender, MouseButtonEventArgs e)
        //{
        //    Content = new ReportAssistantPage();
        //}

        //private void OpenTestScenarioGenerator(object sender, MouseButtonEventArgs e)
        //{
        //    Content = new TestScenarioGeneratorPage();
        //}

        //private void OpenUIDesigner(object sender, MouseButtonEventArgs e)
        //{
        //    Content = new UIDesignerPage();
        //}

        //private void OpenStoryCharacterGenerator(object sender, MouseButtonEventArgs e)
        //{
        //    Content = new StoryCharacterPage();
        //}

        //private void OpenFinancialAnalyzer(object sender, MouseButtonEventArgs e)
        //{
        //    Content = new FinancialAnalyzerPage();
        //}

        //private void OpenEducationMaterialGenerator(object sender, MouseButtonEventArgs e)
        //{
        //    Content = new EducationMaterialPage();
        //}

        //private void OpenMaintenanceAssistant(object sender, MouseButtonEventArgs e)
        //{
        //    Content = new MaintenanceAssistantPage();
        //}

        //private void OpenImageGenerator(object sender, MouseButtonEventArgs e)
        //{
        //    Content = new ImageGeneratorPage();
        //}






        private void SmartFinancialAnalysis_MouseDown(object sender, MouseButtonEventArgs e)
        {
          // IQD_MessageBox.Show("رسالة", "ستتوفر قريبا....");
        }

        private void Presentations_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // IQD_UI_Library.IQD_MessageBox.Show("رسالة", "ستتوفر قريبا....");

        }

        private void ReportingAssistant_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // IQD_UI_Library.IQD_MessageBox.Show("رسالة", "ستتوفر قريبا....");

        }

        private void TestScenarioGenerator_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Content = new TestScenarioGeneratorPage();
        }

        private void CharacterCreator_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // IQD_UI_Library.IQD_MessageBox.Show("رسالة", "ستتوفر قريبا....");

        }

        private void MaintenanceAssistant_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // IQD_UI_Library.IQD_MessageBox.Show("رسالة", "ستتوفر قريبا....");

        }

        private void InterfaceDesignAssistant_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // IQD_UI_Library.IQD_MessageBox.Show("رسالة", "ستتوفر قريبا....");

        }

        private void EducationalMaterialCreator_MouseDown(object sender, MouseButtonEventArgs e)
        {
          //  IQD_UI_Library.IQD_MessageBox.Show("رسالة", "ستتوفر قريبا....");

        }

        private void ImageGeneration_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // IQD_UI_Library.IQD_MessageBox.Show("رسالة", "ستتوفر قريبا....");

        }
    }
}