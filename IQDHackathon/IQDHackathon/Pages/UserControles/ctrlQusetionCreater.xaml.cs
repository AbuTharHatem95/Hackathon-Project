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
        AddQustiones _ctrladd;
        clsTitle? title = null;


        public event EventHandler<(string QustionNum, string QustionTitle, string QustionScor, string NumberOfAnswer)>? DataLoaded;

        public QusetionCreater(ref TestScenarioGeneratorPage test, AddQustiones ctrladd)
        {
            InitializeComponent();
            this.DataContext = this;
            //btnPrintQustiones.IsEnabled= false;                  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            Test = test;
            _ctrladd = ctrladd;
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
                IQD_MessageBox.Show("تحذير", "يجب ملئ معلومات السؤال", MessageBoxType.Warning);
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
                IQD_MessageBox.Show("تحذير", "يجب ملئ معلومات السؤال", MessageBoxType.Warning);
                return;
            }

            IQD_MessageBox.Show("تنبية", $"عدد الاسئلة {clsQuestion.QuestionsDict.Keys.Count} اسئلة؟؟", MessageBoxType.Question);

            if (clsQuestion.QuestionsDict.Keys.Count >= 3)
            {
                bool? Contnie = IQD_MessageBox.Show("تنبية", $"هل تريد اضافه اكثر من {clsQuestion.QuestionsDict.Keys.Count-1} اسئلة؟؟", MessageBoxType.Question);

                //// عدم الموافقه
                //if (Contnie == null)
                //{
                //    btnPrintQustiones.IsEnabled = true;                  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                //    return;
                //}
                //btnPrintQustiones.IsEnabled = true;
            }
            //هنا يتم اضافة اوبجكت الى دكشنري الاسئلة 
            Questions?.CreateQuestion();
            title = null;

            //اعلام المستخدم بنجاح اضافة السؤال 
            IQD_MessageBox.Show("نجاح", "تم انشاء السؤال بنجاح");

            //تنظيف التيكست لاضافة سؤال اخر
            txtNumberOfAnswers.Clear();
            txtQNum.Clear();
            txtQustionTitle.Clear();
            txtQscore.Clear();
        }

        //طباعة نموذج الاسئلة
        private void btnPrintQustiones_Click(object sender, RoutedEventArgs e)
        {
            IQD_MessageBox.Show("حفظ ", "حفظ نموذج الاسئلة");

            //هنا بعد ان انشئ 6 اسئلة يتم اعلام المستخدم بحفظ نموذج الاسئلة
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
                    IQD_MessageBox.Show("خطأ", $"لم يتم حفظ الاسئلة, حدث خطأ: {ex.Message}", MessageBoxType.Error);
                }
                return;
            }
            else
            {
                IQD_MessageBox.Show("تحذير", "لم تقم بحـفظ الاسئـلة!!", MessageBoxType.Warning);
            }
        }

        //private void GeneratePdf()
        //{

        //    QuestPDF.Settings.License = LicenseType.Community;
        //    DateTime curruntYear = DateTime.Now;

        //    Document.Create(container =>
        //    {

        //        container.Page(page =>
        //        {
        //            page.Size(PageSizes.A4);
        //            page.Margin((float)0.4, Unit.Centimetre);
        //            page.PageColor(Colors.White);
        //            page.DefaultTextStyle(x => x.FontSize(12));

        //            page.Content().PaddingVertical(5).Column(column =>
        //            {
        //                column.Item().Row(row =>
        //                {
        //                    row.RelativeItem().AlignLeft().Text($"المادة: {Test?.CombStage.Text}\n\nالتاريخ: {Test?.txtExapleDate.Text}\n\nالوقت: {Test?.txtExampleTime.Text}\n").Bold().FontSize(12);
        //                    row.RelativeItem().AlignCenter().Text($"{curruntYear.Year}/{curruntYear.AddYears(-1)})\n\nنوع الأمتحان: {Test?.txtTypeQuze.Text}\n\n{Test?.txtNote.Text}").Bold().FontSize(12);
        //                    row.RelativeItem().AlignRight().Text($"المدرسة: {Test?.txtSchoolName.Text}\n\n:مدرس المادة {Test?.txtTeacherName.Text}\n").Bold().FontSize(12);
        //                });

        //                column.Item().LineHorizontal(2).LineColor(Colors.Black);
        //                column.Item().PaddingVertical(10);

        //                foreach (var questionEntry in clsQuestion.QuestionsDict)
        //                {
        //                    var question = questionEntry.Value;
        //                    column.Item().Row(row =>
        //                    {
        //                        row.RelativeItem().AlignRight().Text($"س{question.Title.Number}: {question.Title.QuestionTitle} (درجة {question.Title.ScoreForBranchOrPint})").Bold().FontSize(12);
        //                    });

        //                    if (question.BranchzDict != null)
        //                    {
        //                        foreach (var branchEntry in question.BranchzDict)
        //                        {
        //                            var branch = branchEntry.Value;
        //                            column.Item().Row(row =>
        //                            {
        //                                row.RelativeItem().AlignRight().Text($"({branch.Char}) {branch.BranchTitle} (درجة {branch.Score})").Bold().FontSize(12);
        //                            });

        //                            foreach (var point in branch.PointList)
        //                            {
        //                                column.Item().Row(row =>
        //                                {
        //                                    row.RelativeItem().AlignRight().Text($"- {point.Text} (درجة {point.Score})").FontSize(12);
        //                                });
        //                            }

        //                            // إضافة سطر فارغ بين الفروع
        //                            column.Item().PaddingVertical(5);
        //                        }
        //                    }

        //                    if (question.PointList != null)
        //                    {
        //                        foreach (var point in question.PointList)
        //                        {
        //                            column.Item().Row(row =>
        //                            {
        //                                row.RelativeItem().AlignRight().Text($"- {point.Text} (درجة {point.Score})").FontSize(12);
        //                            });
        //                        }
        //                    }

        //                    // إضافة سطرين فارغين قبل الخط الأسود
        //                    column.Item().PaddingVertical(10);
        //                    column.Item().LineHorizontal(2).LineColor(Colors.Black);
        //                    column.Item().PaddingVertical(10);
        //                }


        //            });
        //        });
        //    })
        //    .GeneratePdf(fullPath);
        //}

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
    }
}
