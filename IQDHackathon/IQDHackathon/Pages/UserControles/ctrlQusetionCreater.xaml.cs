using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using IQD_UI_Library;
using Microsoft.Win32;

namespace Interface.Pages.UserControles
{
    public partial class QusetionCreater : UserControl
    {
        ctrlAddBrach? AddNewBranch = null;
        ctrlQustionListView? qusetionList = null;
        public clsQuestion? Questions = null;
        private TestScenarioGeneratorPage? Test = null;
        private AddQustiones addQustion = null;
        clsTitle? title = null;


        public event EventHandler<(string QustionNum, string QustionTitle, string QustionScor, string NumberOfAnswer)>? DataLoaded;

        public QusetionCreater(ref TestScenarioGeneratorPage test, AddQustiones AddQustion)
        {
            InitializeComponent();
            this.DataContext = this;
            Test = test;
            this.addQustion = AddQustion;
        }

        private void GetDataFromListView(object? sender, (bool IsCheck, string Qustion) e)
        {
            if (e.IsCheck)
            {
                Questions?.AddPoint(new clsPoint(e.Qustion));
            }
            else
            {
                foreach (var item in Questions.PointList)
                {
                    if (item.Text == e.Qustion)
                    {
                        Questions.PointList.Remove(item);
                        return;
                    }
                }
            }
        }

        //اول اجراء سيحدث بعد ملئ المعلومات
        private void btnAddPointes_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtQustionTitle.Text) || string.IsNullOrEmpty(txtQscore.Text) || string.IsNullOrEmpty(txtQNum.Text) || string.IsNullOrEmpty(txtNumberOfAnswers.Text))
            {
                MessageBox.Show("يجب ملئ معلومات السؤال", "تحذير", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (title == null && !string.IsNullOrEmpty(txtQNum.Text))
            {
                 title = new clsTitle(byte.Parse(txtQNum.Text), byte.Parse(txtQscore.Text), byte.Parse(txtNumberOfAnswers.Text), txtQustionTitle.Text);
                Questions = new clsQuestion(title);
            }

            if (qusetionList != null)
            {
                qusetionList.QuestionIsSelected -= GetDataFromListView;
            }

            qusetionList = new ctrlQustionListView(this); 
            MainGrid.Visibility = Visibility.Collapsed;
            SubGrid.Visibility = Visibility.Visible;
            qusetionList.QuestionIsSelected += GetDataFromListView;
            SubGrid.Children.Clear();
            SubGrid.Children.Add(qusetionList);
            qusetionList.Visibility = Visibility.Visible;
        }
       
        //اجراء اضافة افرع الى السؤال
        private void btnAddNewBrach_Click(object sender, RoutedEventArgs e)
        {
            AddNewBranch = new ctrlAddBrach(Questions, this);
            MainGrid.Visibility = Visibility.Collapsed;
            SubGrid.Visibility = Visibility.Visible;
            SubGrid.Children.Clear();
            SubGrid.Children.Add(AddNewBranch);
        }

        //انشاء نموذج سؤال
        private void btnCreateQustion_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtQustionTitle.Text) || string.IsNullOrEmpty(txtQscore.Text) || string.IsNullOrEmpty(txtQNum.Text) || string.IsNullOrEmpty(txtNumberOfAnswers.Text))
            {
                MessageBox.Show("يجب ملئ معلومات السؤال", "تحذير", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Questions?.CreateQuestion();
            title = null;
            MessageBox.Show("تم انشاء السؤال بنجاح", "نجاح العملية");
            txtNumberOfAnswers.Clear();
            txtQNum.Clear();
            txtQustionTitle.Clear();
            txtQscore.Clear();
        }

        //طباعة نموذج الاسئلة
        private void btnPrintQustiones_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("حفظ نموذج الاسئلة", "حفظ ");

            var saveFileDialog = new SaveFileDialog
            {
                DefaultExt = ".pdf",
                Filter = "PDF Files (*.pdf)|*.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string fullPath = saveFileDialog.FileName;
                try
                {
                    Test?.GeneratePdf(ref fullPath);
                    OpenPdf(fullPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ", $"لم يتم حفظ الاسئلة, حدث خطأ: {ex.Message}",MessageBoxButton.OK,MessageBoxImage.Error);
                }
                return;
            }
            else
            {
                MessageBox.Show("تحذير", "لم تقم بحـفظ الاسئـلة!!",MessageBoxButton.OK , MessageBoxImage.Error);
            }

            this.Visibility=Visibility.Collapsed;
            addQustion.MainGrid.Visibility = Visibility.Visible;
        }

        private void OpenPdf(string fullPath)
        {
            try
            {
                var process = new Process();
                process.StartInfo = new ProcessStartInfo(fullPath)
                {
                    UseShellExecute = true
                };
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRetuntomainmenue_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility= Visibility.Collapsed;
            addQustion.SubGrid.Children.Clear();
            addQustion.MainGrid.Visibility= Visibility.Visible;
        }
    }
}
