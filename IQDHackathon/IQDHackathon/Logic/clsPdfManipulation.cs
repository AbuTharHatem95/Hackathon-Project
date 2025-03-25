using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Win32;
using QuestPDF.Infrastructure;
using System.Text;
using System.Windows;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Microsoft.IdentityModel.Tokens;


namespace Interface.LogicClasses
{
    public static  class clsPdfManipulation
    {
        public static string GetPdfPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf"
            };
            
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    return openFileDialog.FileName;
                }
                catch(Exception ex )
                {
                    MessageBox.Show($"There is Error to load pdf file {ex.Message}");
                    return "";
                }
            }
            return "";


        
        }

        //اجي هنا احل المشكلة بكل سهولة 
        //الصيانة حاليا اسهل 

        static bool IsArabicText(string text)
        {
            foreach (char c in text)
            {
                if ((c >= 0x0600 && c <= 0x06FF) ||
                    (c >= 0x0750 && c <= 0x077F) ||
                    (c >= 0x08A0 && c <= 0x08FF) ||
                    (c >= 0xFB50 && c <= 0xFDFF) ||
                    (c >= 0xFE70 && c <= 0xFEFF)) // دعم كامل للأحرف العربية
                {
                    return true;
                }
            }
            return false;
        }
        static string FixArabicText(string input)
        {
            if (IsArabicText(input)) // إذا كان النص يحتوي على العربية
            {
                // تقسيم النص إلى أسطر، لأن بعض الفقرات قد يتم خلطها أثناء الاستخراج
                string[] lines = input.Split('\n');
                List<string> fixedLines = new List<string>();

                foreach (string line in lines)
                {
                    // فصل العناوين والفقرات بإصلاح ترتيب الكلمات داخل كل سطر
                    string[] words = line.Split(' ');
                    Array.Reverse(words);

                    StringBuilder fixedText = new StringBuilder();
                    foreach (string word in words)
                    {
                        if (IsArabicText(word))
                        {
                            char[] charArray = word.ToCharArray();
                            Array.Reverse(charArray);
                            fixedText.Append(new string(charArray) + " ");
                        }
                        else
                        {
                            fixedText.Append(word + " "); // الاحتفاظ بالكلمات الإنجليزية كما هي
                        }
                    }

                    fixedLines.Add(fixedText.ToString().Trim());
                }

                return string.Join("\n", fixedLines);
            }
            return input; // لا تعكس النصوص الإنجليزية
        }
        static string ExtractTextFromPdfPath(string pdfPath)
        {
            List<string> pages = new List<string>();

            using (PdfReader reader = new PdfReader(pdfPath))
            using (PdfDocument pdfDoc = new PdfDocument(reader))
            {
                int pageCount = pdfDoc.GetNumberOfPages();

                for (int i = 1; i <= pageCount; i++) // قراءة الصفحات بالترتيب الصحيح
                {
                    string pageText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), new SimpleTextExtractionStrategy());

                    // حل مشكلة ترتيب النصوص العربية داخل كل صفحة
                    string fixedPageText = FixArabicText(pageText);
                    pages.Add(fixedPageText);
                }
            }

            // إعادة ترتيب الصفحات إذا كانت تظهر بالمقلوب
            pages.Reverse();

            // حفظ النص المعالج إلى ملف
            //using (StreamWriter writer = new StreamWriter("output.txt", false, Encoding.UTF8))
            //{
            //    writer.Write(string.Join("\n\n", pages));
            //}

            return string.Join("\n\n", pages); // دمج الصفحات مع الحفاظ على الترتيب الصحيح
        }

        public static string SelectPdfPathFolder()
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
                    return saveFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return "";
                }

            }
            return "";
        }

        public static string ExtractPdfData()
        {
            string pdfPath = GetPdfPath();
            if(pdfPath != "")
                return ExtractTextFromPdfPath(pdfPath);
            return "";
        }

        public static bool CreatePdfQuestionsUsingData(ref string fullPath, Dictionary<string, List<string>> QuestionsDictFromChatGPT, string subjectName, string examDate, string examTime, string examType, string notes, string teacherName, string schoolName)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            DateTime curruntYear = DateTime.Now;

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
                            row.RelativeItem().AlignLeft().Text($"المادة: {subjectName}\n\nالتاريخ: {examDate}\n\nالوقت: {examTime}\n").Bold().FontSize(12);
                            row.RelativeItem().AlignCenter().Text($"{curruntYear.Year}/{curruntYear.AddYears(-1)})\n\nنوع الأمتحان: {examType}\n\n{notes}").Bold().FontSize(12);
                            row.RelativeItem().AlignRight().Text($"المدرسة: {teacherName}\n\n:مدرس المادة {schoolName}\n").Bold().FontSize(12);
                        });

                        column.Item().LineHorizontal(2).LineColor(Colors.Black);
                        column.Item().PaddingVertical(10);

                        foreach (var questionEntry in clsQuestion.QuestionsDict)
                        {
                            var question = questionEntry.Value;
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().AlignRight().Text($"س{question.Title.Number}: {question.Title.QuestionTitle} (درجة {question.Title.Score})").Bold().FontSize(12);
                            });

                            if (question.BranchzDict != null)
                            {
                                foreach (var branchEntry in question.BranchzDict)
                                {
                                    var branch = branchEntry.Value;
                                    column.Item().Row(row =>
                                    {
                                        row.RelativeItem().AlignRight().Text($"({branch.Char}) {branch.BranchTitle} (درجة {branch.Score})").Bold().FontSize(12);
                                    });

                                    foreach (var point in branch.PointList)
                                    {
                                        column.Item().Row(row =>
                                        {
                                            //تم التعديل هنا
                                            row.RelativeItem().AlignRight().Text($"- {point.Text} (درجة {Math.Round(point.Score)})").FontSize(12);
                                        });
                                    }

                                    // إضافة سطر فارغ بين الفروع
                                    column.Item().PaddingVertical(5);
                                }
                            }

                            if (question.PointList != null)
                            {
                                foreach (var point in question.PointList)
                                {
                                    column.Item().Row(row =>
                                    {
                                        //تم التعديل هنا
                                        row.RelativeItem().AlignRight().Text($"- {point.Text} (درجة {Math.Round(point.Score)})").FontSize(12);
                                    });
                                }
                            }

                            // إضافة سطرين فارغين قبل الخط الأسود
                            column.Item().PaddingVertical(10);
                            column.Item().LineHorizontal(2).LineColor(Colors.Black);
                            column.Item().PaddingVertical(10);
                        }


                    });
                });
            })
            .GeneratePdf(fullPath);
            return true;
        }


        public static async Task<Dictionary<string, List<string>>?> GenerateQuestionsFromPdfUsingAiGpt( List<string> QuestionTypes,string FilePath)
        {
            if (FilePath.Length <= 0)  return null;
            return await clsGptManipulation.QuestionsWithTypes(QuestionTypes, FilePath);
        }


    }
}
