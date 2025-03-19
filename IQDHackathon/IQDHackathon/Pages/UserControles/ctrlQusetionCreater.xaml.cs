using IQD_UI_Library;
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
        ctrlAddBrach? AddNewBranch = null;
        ctrlQustionListView? qusetionList = null;
        public clsQuestion? Questions = null;

        public event EventHandler<(string QustionNum, string QustionTitle, string QustionScor, string NumberOfAnswer)>? DataLoaded;

        public QusetionCreater()
        {
            InitializeComponent();
            this.DataContext = this;
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

            Questions?.AddPoint(new clsPoint(e.Qustion));

        }

        //اول اجراء سيحدث بعد ملئ المعلومات
        private void btnAddPointes_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtQustionTitle.Text) || string.IsNullOrEmpty(txtQscore.Text) || string.IsNullOrEmpty(txtQNum.Text) || string.IsNullOrEmpty(txtNumberOfAnswers.Text))
            {
                IQD_MessageBox.Show("تحذير", "يجب ملئ معلومات السؤال", MessageBoxType.Warning);
                return;
            }

            //هنا اضفنا الشرط للتاكيد بان هذا اليوزر كنترول غير مهيئة في الخلفية
            if (qusetionList == null)
            {
                clsTitle title = new clsTitle(byte.Parse(txtQNum.Text), byte.Parse(txtQscore.Text), byte.Parse(txtNumberOfAnswers.Text), txtQustionTitle.Text);
                Questions = new clsQuestion(title);

                qusetionList = new ctrlQustionListView();
                qusetionList.QuestionIsSelected += GetDataFromListView;
                QustionCreate.Visibility = Visibility.Collapsed;
                QustionList.Visibility = Visibility.Visible;

            }
            else
            {
                qusetionList.QuestionIsSelected += GetDataFromListView;
                QustionCreate.Visibility = Visibility.Collapsed;
                QustionList.Visibility = Visibility.Visible;
            }

            QustionList.Children.Clear();
            QustionList.Children.Add(qusetionList);
        }
       
        //اجراء اضافة افرع الى السؤال
        private void btnAddNewBrach_Click(object sender, RoutedEventArgs e)
        {
            if(AddNewBranch==null)
            {
                AddNewBranch = new ctrlAddBrach(Questions);
                QustionCreate.Visibility = Visibility.Collapsed;
                QustionList.Visibility = Visibility.Visible;
               
            }
            else
            {
                QustionCreate.Visibility = Visibility.Collapsed;
                QustionList.Visibility = Visibility.Visible;

            }

            QustionList.Children.Clear();
            QustionList.Children.Add(AddNewBranch);

        }

        //انشاء نموذج سؤال
        private void btnCreateQustion_Click(object sender, RoutedEventArgs e)
        {
            //هنا نتاكد من انه لا يتخطا العدد المسموح له بأنشاء اسئلة فقط 6
            if(Questions?.BranchzDict?.Count >= 6)
            {
                IQD_MessageBox.Show("تحذير","تم انشاء 6 اسئلة",MessageBoxType.Warning);

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
                        GeneratePdf(fullPath);
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

            if(string.IsNullOrEmpty(txtQustionTitle.Text)|| string.IsNullOrEmpty(txtQscore.Text)|| string.IsNullOrEmpty(txtQNum.Text)|| string.IsNullOrEmpty(txtNumberOfAnswers.Text))
            {
                IQD_MessageBox.Show("تحذير", "يجب ملئ معلومات السؤال", MessageBoxType.Warning);
                return;
            }

            //هنا يتم اضافة اوبجكت الى دكشنري الاسئلة 
            Questions?.CreateQuestion();

            //اعلام المستخدم بنجاح اضافة السؤال 
            IQD_MessageBox.Show("نجاح", "تم انشاء السؤال بنجاح");

            //تنظيف التيكست لاضافة سؤال اخر
            txtNumberOfAnswers.Clear();
            txtQNum.Clear();
            txtQustionTitle.Clear();
            txtQscore.Clear();

            //هنا يجب ان نتاكد من ان اليوزر كنترول الواقف عليه غير مكرر في الخلفية
            //QustionList.Children.Clear();
            //QustionList.Children.Add(this);


        }



        //يجب تمرير اوبجكت السؤال لكي يتم الملئ هنا في هذا النموذج  
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



        //حاليا مهمش
        private void btnAddQustion_Click_1(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrEmpty(txtQustionTitle.Text) || string.IsNullOrEmpty(txtQscore.Text) || string.IsNullOrEmpty(txtQNum.Text) || string.IsNullOrEmpty(txtNumberOfAnswers.Text))
            //{
            //    IQD_MessageBox.Show("تحذير", "يجب ملئ معلومات السؤال", MessageBoxType.Warning);
            //    return;
            //}


            //clsTitle title = new clsTitle(byte.Parse(txtQNum.Text), byte.Parse(txtQscore.Text), byte.Parse(txtNumberOfAnswers.Text), txtQustionTitle.Text);
            //Questions = new clsQuestion(title);

            //if(qusetionList==null)
            //{
            //    qusetionList=new ctrlQustionListView();
            //    qusetionList.QuestionIsSelected += GetDataFromListView;
            //    QustionCreate.Visibility = Visibility.Collapsed;
            //    QustionList.Visibility = Visibility.Visible;

            //}
            //else
            //{
            //    qusetionList.QuestionIsSelected += GetDataFromListView;
            //    QustionCreate.Visibility = Visibility.Collapsed;
            //    QustionList.Visibility = Visibility.Visible;
            //}

            //QustionList.Children.Clear();
            //QustionList.Children.Add(qusetionList);
        }
    }
}
