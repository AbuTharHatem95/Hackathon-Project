using Interface.Pages.UserControles;
using IQD_UI_Library;
using IQDHackathon;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Win32;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace Interface.Pages
{
    //يجب العمل على ان يجلب الاسئلة بناء على النمط المختار من الواجهه وبناء عليها يصممها الجات جي بي تي 
    //بيج لاملائ معلومات المدرسه ومعلومات الاستاذ ورفع مادة الامتحان 
    public partial class TestScenarioGeneratorPage : Page
    {
        private readonly string? __openAiApiKey =  Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        public List<StyleModel> Styles { get; set; }

        public static Dictionary<string, List<string>> QuestionsDictFromChatGPT = new Dictionary<string, List<string>>();

        private string extractedText;

        IQD_LoadingControl load;


        public TestScenarioGeneratorPage()
        {
            InitializeComponent();
            FillComboBoxSubject();

            // تعبئة قائمة الأنماط
            Styles = new List<StyleModel>
            {
                new StyleModel { Name = "أسئلة اختيار من متعدد", IsSelected = false },
                new StyleModel { Name = "أسئلة صح/خطأ", IsSelected = false },
                new StyleModel { Name = "أسئلة ملء الفراغ", IsSelected = false },
                new StyleModel { Name = "أسئلة مقالية", IsSelected = false }
            };

            // ربط القائمة بـ ItemsControl
            CheckBoxList.ItemsSource = Styles;

            // جلب البيانات من قاعدة البيانات
            //var databaseService = new DatabaseService();
            //QuestionStyles = new ObservableCollection<QuestionStyle>(databaseService.GetQuestionStyles());

            //// تعيين مصدر البيانات لـ ItemsControl
            //CheckBoxList.ItemsSource = QuestionStyles;
        }

        private List<string> GetSelectedStyles()
        {
            return Styles.Where(style => style.IsSelected).Select(style => style.Name).ToList();
        }

        private bool VauldationText()
        {
            if (
                string.IsNullOrEmpty(txtNote.Text) ||
                string.IsNullOrEmpty(CombClass.Text) ||
                string.IsNullOrEmpty(CombGrade.Text) ||
                string.IsNullOrEmpty(CombStage.Text) ||
                string.IsNullOrEmpty(txtSchoolName.Text) ||
                string.IsNullOrEmpty(txtExampleTime.Text) ||
                string.IsNullOrEmpty(txtTeacherName.Text) ||
                string.IsNullOrEmpty(txtTypeQuze.Text))
            {
                IQD_MessageBox.Show("تحذير", "يجب ملئ المعلومات", MessageBoxType.Warning);
                return true;
            }
            return false;
        }

        private void FillComboBox()
        {
            // إضافة عناصر إلى ComboBox
        }

        //انشاء نموذج بشكل دينمايكي باستخدام GPT
        private async void GenretWithGPT_Click(object sender, RoutedEventArgs e)
        {
            //if (VauldationText())
            //    return;

            IQD_MessageBox.Show("اختيار", "يرجى اختيار ملف المادة");

            await LoadPdfFile();
        }

        private async Task LoadPdfFile()
        {
            // جمع الأنماط المختارة
            var selectedStyles = Styles.Where(style => style.IsSelected).Select(style => style.Name).ToList();

            if (selectedStyles.Count == 0)
            {
                IQD_MessageBox.Show("تحذير", "لم تقم باختيار أي أنماط!", MessageBoxType.Warning);
                return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    extractedText = ExtractTextFromPdf(openFileDialog.FileName);

                    if (string.IsNullOrEmpty(extractedText))
                    {
                        IQD_MessageBox.Show("تحذير", "يجب رفع ملف المادة!!", MessageBoxType.Warning);
                        return;
                    }

                    //load = new IQD_LoadingControl();
                    //Task.Run(async () => load.ShowDialog());

                    // إنشاء الأسئلة بناءً على الأنماط المختارة
                    await GenerateQuestionsFromText(extractedText, selectedStyles);

                  // load.Close();

                    //من المفترض ان نقوم باستدعاء هذه الميثود من داخل بيج انشاء الاسئلة 
                   // CreateModles();
                }
                catch (Exception ex)
                {
                   // load.Close();
                    IQD_MessageBox.Show("خطأ", $"لم يتم الحفظ بنجاح: {ex.Message}", MessageBoxType.Error);
                }
            }
        }

        // بناء نموذج اسئلة بشكل يدوي 
        private async void Gentet_Click(object sender, RoutedEventArgs e)
        {
            if (VauldationText())
                return;

            await LoadPdfFile();

            MainPageGrid.Visibility = Visibility.Collapsed;
            SubMain.Visibility = Visibility.Visible;
            SubMain.Children.Clear();
            SubMain.Children.Add(new AddQustiones(this));
        }


        private void FillComboBoxStage()
        {
            // إضافة عناصر إلى ComboBox
            CombStage.Items.Add("الاول");
            CombStage.Items.Add("الثاني");
            CombStage.Items.Add("الثالث");
            CombStage.Items.Add("الرابع");
            CombStage.SelectedIndex = 0;
        }

        private void FillComboBoxSubject()
        {
            // إضافة عناصر إلى ComboBox
            CombSubject.Items.Add("الرياضيات");
            CombSubject.Items.Add("الاحياء");
            CombSubject.Items.Add("الفيزياء");
            CombSubject.Items.Add("اليكمياء");
            CombSubject.SelectedIndex = 0;
        }

        //اجراء خاص بجات جي بي تي ,حيث يتم بناء النموذج بشكل يدوي مع تمرير انماط الاسئلة

        public static string ExtractTextFromPdf(string filePath)
        {
            StringBuilder extractedText = new StringBuilder();

            using (var reader = new PdfReader(filePath))
            using (var pdfDoc = new PdfDocument(reader))
            {
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    // استخدام LocationTextExtractionStrategy للحفاظ على ترتيب النصوص
                    ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                    string pageText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), strategy);
                    extractedText.AppendLine(pageText);
                }
            }
            return extractedText.ToString();
        }

        private async Task GenerateQuestionsFromText(string text, List<string> selectedStyles)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                IQD_MessageBox.Show("⚠️ الرجاء إدخال نص قبل المتابعة.", "تحذير", MessageBoxType.Warning);
                return;
            }

            //مسح الدكشنري الحالي للأسئلة
            QuestionsDictFromChatGPT.Clear();

            await FillQuestionsDictionary(text, selectedStyles);
        }

        private async Task FillQuestionsDictionary(string text, List<string> selectedStyles)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("قم بإنشاء 6 أسئلة لكل من الأنماط التالية: ");
            foreach (var style in selectedStyles)
            {
                stringBuilder.Append($"{style}, ");
            }
            stringBuilder.AppendLine($"بناءً على هذا المحتوى:\n{text}");
            stringBuilder.AppendLine("بدون ترقيم الاسئلة");


            // استدعاء ChatGPT للحصول على الأسئلة (يجب أن تكون لديك دالة GetQuestionsFromChatGPT تعمل بشكل صحيح)
            string questionsResponse = await GetQuestionsFromChatGPT(stringBuilder.ToString());

            // تقسيم الأسئلة المسترجعة إلى قائمة
            List<string> questionList = questionsResponse.Split('\n')
                                                        .Where(q => !string.IsNullOrWhiteSpace(q))
                                                        .ToList();

            // توزيع الأسئلة على الأنماط وإضافتها إلى الدكشنري
            int index = 0;
            foreach (var style in selectedStyles)
            {
                if (!QuestionsDictFromChatGPT.ContainsKey(style))
                {
                    QuestionsDictFromChatGPT[style] = new List<string>();
                }

                // أخذ 6 أسئلة لكل نمط (إذا كانت متوفرة)
                var styleQuestions = questionList.Skip(index).Take(6).ToList();
                QuestionsDictFromChatGPT[style].AddRange(styleQuestions);

                index += 6; // الانتقال إلى الأسئلة التالية لكل نمط
            }
        }

        private async Task<string> GetQuestionsFromChatGPT(string prompt)
        {
            try
            {
                using HttpClient client = new HttpClient();

                if (string.IsNullOrEmpty(__openAiApiKey))
                    throw new Exception("API Key غير مضبوط! تأكد من إضافته.");

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {__openAiApiKey}");

                // "gpt-4"
                var requestBody = new
                {
                    model = "gpt-4-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = "أنت مساعد ذكاء اصطناعي قم بتحليل النصوص وترتيبها حتى اذا كانت معكوسه  وإنشاء أسئلة فقط بدون إجابات." },
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 400
                };

                string jsonBody = JsonConvert.SerializeObject(requestBody);
                HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                }
                catch (HttpRequestException httpEx)
                {
                    throw new Exception("❌ فشل الاتصال بـ OpenAI. تحقق من اتصالك بالإنترنت.", httpEx);
                }

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"خطأ في الاتصال بـ OpenAI: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

                string responseString = await response.Content.ReadAsStringAsync();
                dynamic? responseData = JsonConvert.DeserializeObject(responseString);

                if (responseData?.choices == null || responseData?.choices.Count == 0)
                    throw new Exception("لم يتم استلام بيانات صحيحة من OpenAI!");

                return responseData?.choices[0].message.content.ToString();
            }
            catch (Exception ex)
            {
                IQD_MessageBox.Show("خطأ", ex.Message, MessageBoxType.Error);
                return "حدث خطأ أثناء جلب الأسئلة من ChatGPT.";
            }
        }

        // يستخدم بتعبة النموذج الاسئلة هارد كود  
        private void GenerateExamPdf(ref string fullPath,Dictionary<byte, clsQuestion> questionsDict1)
        {

            Dictionary<byte, clsQuestion> questionsDict = new Dictionary<byte, clsQuestion>();

            // السؤال الأول: 6 نقاط تعاريف، درجته 25، عدد الإجابات المطلوبة 5
            var q1 = new clsQuestion(new clsTitle(1, 25, 5, "عرف المفاهيم التالية"))
                .AddPoint(new clsPoint("التكامل"))
                .AddPoint(new clsPoint("التفاضل"))
                .AddPoint(new clsPoint("المصفوفات"))
                .AddPoint(new clsPoint("الاحتمالات"))
                .AddPoint(new clsPoint("المعادلات التفاضلية"))
                .AddPoint(new clsPoint("النهايات"));
            q1.CreateQuestion();
            questionsDict.Add(q1.Title.Number, q1);

            // السؤال الثاني: 3 أفرع، درجته 25، المطلوب الإجابة عن فرعين، وكل فرع يحتوي على 3 نقاط، الإجابة عن نقطتين
            var q2 = new clsQuestion(new clsTitle(2, 25, 2, "أجب عن فرعين فقط مما يلي"))
                .AddBranch('أ').AddPointToBranch('أ', new clsPoint("مفهوم الاحتمالات"))
                .AddPointToBranch('أ', new clsPoint("أنواع الاحتمالات"))
                .AddPointToBranch('أ', new clsPoint("تطبيقات الاحتمالات"))
                .AddBranch('ب').AddPointToBranch('ب', new clsPoint("التفاضل العادي"))
                .AddPointToBranch('ب', new clsPoint("التفاضل الجزئي"))
                .AddPointToBranch('ب', new clsPoint("قاعدة السلسلة"))
                .AddBranch('ج').AddPointToBranch('ج', new clsPoint("التكامل المحدد"))
                .AddPointToBranch('ج', new clsPoint("التكامل غير المحدد"))
                .AddPointToBranch('ج', new clsPoint("حساب المساحات"));
            q2.CreateQuestion();
            questionsDict.Add(q2.Title.Number, q2);

            // السؤال الثالث: سؤال مقالي
            var q3 = new clsQuestion(new clsTitle(3, 20, 1, "وضح أهمية علم الرياضيات في الحياة اليومية"));
            q3.CreateQuestion();
            questionsDict.Add(q3.Title.Number, q3);

            // السؤال الرابع: فراغات
            var q4 = new clsQuestion(new clsTitle(4, 15, 3, "أكمل الفراغات التالية"))
                .AddPoint(new clsPoint("إذا كانت المصفوفة مربعة فإن محددها يمكن أن يكون ___"))
                .AddPoint(new clsPoint("الزاوية القائمة تساوي ___ درجة"))
                .AddPoint(new clsPoint("المشتقة الثانية للدالة تعطي ___"));
            q4.CreateQuestion();
            questionsDict.Add(q4.Title.Number, q4);

            // السؤال الخامس: اختر الإجابة الصحيحة
            var q5 = new clsQuestion(new clsTitle(5, 15, 3, "اختر الإجابة الصحيحة"))
                .AddPoint(new clsPoint("ناتج تكامل الدالة \\(x^2\\) هو: أ) x^3/3 ب) 2x ج) x^2/2"))
                .AddPoint(new clsPoint("التفاضل يعبر عن: أ) المعدل الزمني للتغير ب) المساحة تحت المنحنى ج) التكامل العكسي"))
                .AddPoint(new clsPoint("النقطة التي تساوي فيها مشتقة الدالة صفر هي: أ) نقطة العظمى ب) نقطة الانعطاف ج) نقطة البداية"));
            q5.CreateQuestion();
            questionsDict.Add(q5.Title.Number, q5);

            //return questionsDict;
            GeneratePdf(ref fullPath);
        }

        //يستخدم مع جات جي بي تي 
        public void GeneratePdf(ref string fullPath)
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
                            row.RelativeItem().AlignLeft().Text($"المادة: {CombSubject.Text}\n\nالتاريخ: {txtExapleDate.Text}\n\nالوقت: {txtExampleTime.Text}\n").Bold().FontSize(12);
                            row.RelativeItem().AlignCenter().Text($"{curruntYear.Year}/{curruntYear.AddYears(-1)})\n\nنوع الأمتحان: {txtTypeQuze.Text}\n\n{txtNote.Text}").Bold().FontSize(12);
                            row.RelativeItem().AlignRight().Text($"المدرسة: {txtSchoolName.Text}\n\n:مدرس المادة {txtTeacherName.Text}\n").Bold().FontSize(12);
                        });

                        column.Item().LineHorizontal(2).LineColor(Colors.Black);
                        column.Item().PaddingVertical(10);

                        foreach (var questionEntry in clsQuestion.QuestionsDict)
                        {
                            var question = questionEntry.Value;
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().AlignRight().Text($"س{question.Title.Number}: {question.Title.QuestionTitle} (درجة {question.Title.ScoreForBranchOrPint})").Bold().FontSize(12);
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
                                            row.RelativeItem().AlignRight().Text($"- {point.Text} (درجة {point.Score})").FontSize(12);
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
                                        row.RelativeItem().AlignRight().Text($"- {point.Text} (درجة {point.Score})").FontSize(12);
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
        }

        private void CreateModles()
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
                    //يستخدم مع جات جي بي تي
                    GeneratePdf(ref fullPath);

                    //كتابة نموذج اسئلة هارد كود 
                   // GenerateExamPdf(ref fullPath,clsQuestion.QuestionsDict);
                   
                    OpenPdf(fullPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public  void OpenPdf(string fullPath)
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
                IQD_MessageBox.Show("Error", $"Failed to open PDF: {ex.Message}", MessageBoxType.Error);
            }
        }

       
        //اجراءات خاصة بلصفحة
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // إغلاق الصفحة إذا كانت معروضة داخل NavigationWindow أو Frame
            if (this.NavigationService != null)
                this.NavigationService.GoBack(); // العودة إلى الصفحة السابقة
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            // الوصول إلى النافذة التي تحتوي على الصفحة
            var window = Window.GetWindow(this);
            if (window != null)
            {
                // تغيير حالة النافذة بين التكبير والتصغير
                if (window.WindowState == WindowState.Normal)
                    window.WindowState = WindowState.Maximized;
                else
                    window.WindowState = WindowState.Normal;
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            // الوصول إلى النافذة التي تحتوي على الصفحة
            var window = Window.GetWindow(this);
            if (window != null)
                window.WindowState = WindowState.Minimized; // تصغير النافذة
        }

        private void btnRetuntomainmenue_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void txtSchoolName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

    public class StyleModel : INotifyPropertyChanged
    {
        private string _name;

        private bool _isSelected;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   
}