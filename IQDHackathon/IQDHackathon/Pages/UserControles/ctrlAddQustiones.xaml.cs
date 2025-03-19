using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    public partial class AddQustiones : UserControl
    {
      
        public AddQustiones()
        {
            InitializeComponent();
            
        }

        //اختيار اسئلة بشكل يدوي
        private void btnAddQustion_Click(object sender, RoutedEventArgs e)
        {
            QusetionCreater qusetionCreater = new QusetionCreater();
            MainGrid.Visibility = Visibility.Collapsed;
            SubGrid.Visibility = Visibility.Visible;
            SubGrid.Children.Add(qusetionCreater);
        }

        //اعادة توليد اسئلة
        private void btnRelodGenric_Click(object sender, RoutedEventArgs e)
        {
            //هنا يكتب كود ليعيد ملئ دكشنري الاسئلة من خلال جات جي بي تي 
        }




        //كود زيون 
        //private void GeneratePdfFromGhatGPT(string fullPath)
        //{
        //    QuestPDF.Settings.License = LicenseType.Community;
        //    string School = "المادة: تجميع الحاسوب", date = "التاريخ : 2025/1/1", time = " الوقت : ساعتان";
        //    string title = "أسئلة طلبة الانتساب", yer = "للعام الدراسي (2025/2024)", mo = "إدارة إعدادية الحسين (ع)";

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
        //                    row.RelativeItem().AlignLeft().Text($"{School}\n\n{date}\n \n{time}\n").Bold().FontSize(12);
        //                    row.RelativeItem().AlignCenter().Text($"{title}\n\n{yer}\n\n{"مالحظة / اجب عن خمسة فقط"}").Bold().FontSize(12);
        //                    row.RelativeItem().AlignRight().Text($"{mo}\n ").Bold().FontSize(12);

        //                });


        //                // خط فاصل تحت التفاصيل
        //                column.Item().LineHorizontal(2).LineColor(Colors.Black);

        //                column.Item().PaddingVertical(10);

        //                string[] questions =
        //                {
        //                "املى الفراغات مما يلي؟",
        //                "مراحل تصنيع المعالج",
        //                "عرف مما يلي؟"
        //            };

        //                string[][] subQuestions =
        //                {
        //              new string[] { "(1) يوسف ______هواي", "(2)  اين تقع_________" }, // تفرعات السؤال الأول
        //              new string[] { "(1) التصميم", "(2) التصنيع", "(3) الاختبار" }, // تفرعات السؤال الثاني
        //              new string[] { " قدرة المعالجة؟", " ?استهلاك الطاقة" } // تفرعات السؤال الثالث
        //           };

        //                string[] grades = { "(درجة 10)", "(درجة 8)", "(درجة 12)" };

        //                for (int i = 0; i < questions.Length; i++)
        //                {
        //                    // السؤال الرئيسي
        //                    column.Item().Row(row =>
        //                    {
        //                        row.RelativeItem().AlignRight().Text($"س{i + 1}: {questions[i]} {grades[i]}").Bold().FontSize(12);
        //                    });

        //                    // التفرعات إن وجدت
        //                    if (subQuestions[i].Length > 0)
        //                    {
        //                        foreach (var sub in subQuestions[i])
        //                        {
        //                            column.Item().Row(row =>
        //                            {
        //                                row.RelativeItem().AlignRight().Text(sub).FontSize(12);
        //                            });
        //                        }
        //                    }

        //                    // إضافة خط فقط بين الأسئلة الرئيسية
        //                    column.Item().LineHorizontal(2).LineColor(Colors.Black);
        //                    column.Item().PaddingVertical(10);
        //                }



        //                string TitleTecher = ":مدرس المادة", Techer = "زين العابدين";


        //                column.Item().Row(row =>
        //                {
        //                    row.RelativeItem().AlignLeft().Text($"\n\n{TitleTecher}\n{Techer}\t\t\t\t\t").Bold().FontSize(12);
        //                });
        //            });

        //        });
        //    })
        //    .GeneratePdfFromGhatGPT(fullPath);
        //}


        //private void OpenPdf(string fullPath)
        //{
        //    try
        //    {
        //        var process = new Process();
        //        process.StartInfo = new ProcessStartInfo(fullPath)
        //        {
        //            UseShellExecute = true
        //        };
        //        process.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Failed to open PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}



    }
}
