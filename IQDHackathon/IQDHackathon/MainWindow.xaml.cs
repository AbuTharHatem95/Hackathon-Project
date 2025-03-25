using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BLL;
using Interface;
using Interface.Logic;
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
            DataTable dt = new DataTable();
            dt = clsSettings.GetAll();
            if (dt.Rows.Count>0)
            {
                clsGlobal.AISetting = new clsSettings(dt.Rows[0]["ApiKey"].ToString()!, dt.Rows[0]["SecretKey"].ToString()!, dt.Rows[0]["ModelName"].ToString()!);

            }

        }

        // خاصية لإغلاق النافذة
        public static bool CloseWindow
        {
            get { return _closeWindow; }
            set
            {
                _closeWindow = value;
                if (_closeWindow)
                {
                    // إغلاق النافذة الحالية
                    Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Close();
                }
            }
        }
        private static bool _closeWindow;



        private  void btnClose_Click(object sender, RoutedEventArgs e)
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
            MessageBox.Show("سيتم توفير هذا الميزة قريبا...","رسالة",MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Presentations_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("سيتم توفير هذا الميزة قريبا...", "رسالة", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void ReportingAssistant_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("سيتم توفير هذا الميزة قريبا...", "رسالة", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void TestScenarioGenerator_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TestScenarioGeneratorPage test = new TestScenarioGeneratorPage();
            GridMain.Visibility = Visibility.Collapsed;
            GridSubMain.Visibility = Visibility.Visible;
            MainFrame.Navigate(test);
        }

        private void CharacterCreator_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("سيتم توفير هذا الميزة قريبا...", "رسالة", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void MaintenanceAssistant_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("سيتم توفير هذا الميزة قريبا...", "رسالة", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void InterfaceDesignAssistant_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("سيتم توفير هذا الميزة قريبا...", "رسالة", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void EducationalMaterialCreator_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("سيتم توفير هذا الميزة قريبا...", "رسالة", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void ImageGeneration_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("سيتم توفير هذا الميزة قريبا...", "رسالة", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow setting = new SettingWindow();
            setting.ShowDialog();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}