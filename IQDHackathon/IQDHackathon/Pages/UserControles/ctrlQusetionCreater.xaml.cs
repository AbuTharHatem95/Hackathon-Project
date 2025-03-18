using Microsoft.Win32;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Document = QuestPDF.Fluent.Document;

namespace Interface.Pages.UserControles
{
    public partial class QusetionCreater : UserControl
    {

        public clsQuestion Questions;

        public event EventHandler<(bool IsChecked, string Qustion)>? StateChanged; // حدث جديد للإعلام بحالة CheckBox وقيمة TextBox

        public event EventHandler<(string QustionNum, string QustionTitle, string QustionScor, string NumberOfAnswer)>? DataLoaded;

        public QusetionCreater()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        private void GetDataFromListView(object sender, (bool IsCheck, string Qustion) e)
        {

            if (e.IsCheck)
            {
                Questions.AddPoint(new clsPoint(e.Qustion));


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

            Questions.AddPoint(new clsPoint(e.Qustion));

        }

        private void btnAddQustion_Click_1(object sender, RoutedEventArgs e)
        {
            clsTitle title = new clsTitle(byte.Parse(txtQNum.Text), byte.Parse(txtQscore.Text), byte.Parse(txtNumberOfAnswers.Text), txtQustionTitle.Text);
            Questions = new clsQuestion(title);

            ctrlQustionListView qusetionList = new ctrlQustionListView();
            qusetionList.QuestionStateChanged += GetDataFromListView;
            QustionCreate.Visibility = Visibility.Collapsed;
            QustionList.Visibility = Visibility.Visible;
            ContentFrame.Navigate(qusetionList);
        }


        private void btnAddQustionPointes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadDataFromEvent(object sender, (string QNum, string Qtitle, string QAnswer, string Qscore) e)
        {
            //هنا نملئ الاوبجكت تبع clsQustion
        }


        private void btnAddPointes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddNewBrach_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCreateQustion_Click(object sender, RoutedEventArgs e)
        {

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
                    GeneratePdf(fullPath);
                    OpenPdf(fullPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }


        private void GeneratePdf(string fullPath)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            string School = "المادة: تجميع الحاسوب", date = "التاريخ : 2025/1/1", time = " الوقت : ساعتان";
            string title = "أسئلة طلبة الانتساب", yer = "للعام الدراسي (2025/2024)", mo = "إدارة إعدادية الحسين (ع)";

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin((float)0.4, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().PaddingVertical(5).Column(column =>
                    {

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().AlignLeft().Text($"{School}\n\n{date}\n \n{time}\n").Bold().FontSize(12);
                            row.RelativeItem().AlignCenter().Text($"{title}\n\n{yer}\n\n{"مالحظة / اجب عن خمسة فقط"}").Bold().FontSize(12);
                            row.RelativeItem().AlignRight().Text($"{mo}\n ").Bold().FontSize(12);

                        });


                        // خط فاصل تحت التفاصيل
                        column.Item().LineHorizontal(2).LineColor(Colors.Black);

                        column.Item().PaddingVertical(10);

                        string[] questions =
                        {
                        "املى الفراغات مما يلي؟",
                        "مراحل تصنيع المعالج",
                        "عرف مما يلي؟"
                    };

                        string[][] subQuestions =
                        {
                      new string[] { "(1) يوسف ______هواي", "(2)  اين تقع_________" }, // تفرعات السؤال الأول
                      new string[] { "(1) التصميم", "(2) التصنيع", "(3) الاختبار" }, // تفرعات السؤال الثاني
                      new string[] { " قدرة المعالجة؟", " ?استهلاك الطاقة" } // تفرعات السؤال الثالث
                   };

                        string[] grades = { "(درجة 10)", "(درجة 8)", "(درجة 12)" };

                        for (int i = 0; i < questions.Length; i++)
                        {
                            // السؤال الرئيسي
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().AlignRight().Text($"س{i + 1}: {questions[i]} {grades[i]}").Bold().FontSize(12);
                            });

                            // التفرعات إن وجدت
                            if (subQuestions[i].Length > 0)
                            {
                                foreach (var sub in subQuestions[i])
                                {
                                    column.Item().Row(row =>
                                    {
                                        row.RelativeItem().AlignRight().Text(sub).FontSize(12);
                                    });
                                }
                            }

                            // إضافة خط فقط بين الأسئلة الرئيسية
                            column.Item().LineHorizontal(2).LineColor(Colors.Black);
                            column.Item().PaddingVertical(10);
                        }



                        string TitleTecher = ":مدرس المادة", Techer = "زين العابدين";


                        column.Item().Row(row =>
                        {
                            row.RelativeItem().AlignLeft().Text($"\n\n{TitleTecher}\n{Techer}\t\t\t\t\t").Bold().FontSize(12);
                        });
                    });

                });
            })
            .GeneratePdf(fullPath);
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


        //private void LoadDataFromText()
        //{
        //    question = new clsQuestion(new clsTitle(byte.Parse(txtQNum.Text),byte.Parse(txtQscore.Text),byte.Parse(txtNumberOfAnswers.Text),txtQustionTitle.Text));
        //}

        //private void btnAddNewBrach_Click(object sender, RoutedEventArgs e)
        //{

        //    QustionCreate.Visibility = Visibility.Collapsed;
        //    QustionList.Visibility = Visibility.Visible;
        //    ContentFrame.Navigate(new ctrlAddBrach(QuestionsDictFromChatGPT, QStyle, question));

        //   // ContentFrame.Navigate(new AddQustiones(_qustion, AddQustiones.Mod.BrachMod,QStyle,question)); 
        //    //يجب فتح الليست بدون النمط الوقف عليه
        //}

        //private void btnAddQustion_Click(object sender, RoutedEventArgs e)
        //{
        //    LoadDataFromText();

        //    QustionCreate.Visibility = Visibility.Collapsed;
        //    QustionList.Visibility = Visibility.Visible;
        //    ContentFrame.Navigate(new AddQustiones(QuestionsDictFromChatGPT,QStyle,question)); 
        //}

        ////يتم انشاء اوبجكت عند حدوث هذا الايفنت
        //private void btnCreateQustion_Click(object sender, RoutedEventArgs e)
        //{
        //    //انشاء اوبجكت من التايتل 
        //    //باي بوتن تريد يصير هذا الايفنت تضيف هذا السطر
        //    DataLoaded?.Invoke(this, (txtQNum.Text, txtQustionTitle.Text, txtQscore.Text, txtNumberOfAnswers.Text));
        //}

        //private void GentetListViewComponat(Dictionary<string, List<string>> qustiones)
        //{
        //    foreach (var style in qustiones)
        //    {
        //        // إنشاء عنصر تحكم ديناميكي
        //        ctrlDynamicListControl listView = new ctrlDynamicListControl(style.Key, style.Value);

        //        // الاشتراك في الحدث الجديد
        //        //listView.QuestionStateChanged += ListView_QuestionStateChanged;

        //        //// إضافة العنصر إلى الواجهة
        //        //ItemsListBox.Items.Add(listView);
        //    }
        //}
    }
}
