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
        public List<StyleModel> Styles { get; set; }

        public static Dictionary<string, List<string>> QuestionsDictFromChatGPT = new Dictionary<string, List<string>>();

        private string extractedText;
        IQD_LoadingControl load;
        private List<QuestionItem> __generatedQuestions = new List<QuestionItem>();


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
            if (string.IsNullOrEmpty(CombClass.Text) || string.IsNullOrEmpty(CombGrade.Text) || string.IsNullOrEmpty(CombStage.Text) || string.IsNullOrEmpty(txtExampleTime.Text) ||
             string.IsNullOrEmpty(txtNote.Text) || string.IsNullOrEmpty(txtSchoolName.Text) || string.IsNullOrEmpty(txtTeacherName.Text) ||
             string.IsNullOrEmpty(txtTypeQuze.Text))
            {
                IQD_MessageBox.Show("تحذير", "يجب ملئ المعلومات", MessageBoxType.Warning);
                return true;
            }
            return false;

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


                    if(string.IsNullOrEmpty(extractedText))
                    {
                        IQD_MessageBox.Show("تحذير", "يجب رفع ملف المادة!!", MessageBoxType.Warning);
                        return;
                    }

                    load = new IQD_LoadingControl();
                    load.ShowDialog();
                    // إنشاء الأسئلة بناءً على الأنماط المختارة
                    await GenerateQuestionsFromText(extractedText, selectedStyles);

                    load.Close();

                    CreateModles();

                   
                }
                catch (Exception ex)
                {
                    load.Close();
                    IQD_MessageBox.Show("خطأ", $"لم يتم الحفظ بنجاح: {ex.Message}", MessageBoxType.Error);
                }
            }
          
        }

        //بناء نموذج اسئلة بشكل يدوي
        private async void Gentet_Click(object sender, RoutedEventArgs e)
        {
            if (VauldationText())
                return;

            await LoadPdfFile();

           
            //اجراء لملئ اسئلة هارد كود
            GenretQuestionesHardCode();
            MainPageGrid.Visibility = Visibility.Collapsed;
            QuestionsPage.Visibility = Visibility.Visible;
            ContentFrame.Navigate(new AddQustiones());
            //




            //اجراء الجات جي بي تي
            //load = new IQD_LoadingControl();
            //load.ShowDialog();

            //await Task.Delay(2000);

            //if (QuestionsDictFromChatGPT.Count > 0)
            //{
            //    load.Close();
            //    MainPageGrid.Visibility = Visibility.Collapsed;
            //    QuestionsPage.Visibility = Visibility.Visible;
            //    ContentFrame.Navigate(new AddQustiones());
            //    return;
            //}
            //else
            //{
            //    IQD_MessageBox.Show("⚠️  لم يتم إنشاء أي أسئلة بعد,حاول مرة اخرى", "تحذير", MessageBoxType.Warning);
            //}

            //load.Close();

            //

        }

        private void GenretQuestionesHardCode()
        {
            QuestionsDictFromChatGPT.Add("تعاريف", new List<string>
            {
                "المعالج",
                "سعيد بن عمر",
                "محمد الفاتح",
                "سعيد ابن الجبير",
                "يوسف محمد",
                "سعيد بن عمر",
                "محمد الفاتح",
                "سعيد ابن الجبير سعيد ابن الجبير سعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبيرسعيد ابن الجبير"

            }
            );
            QuestionsDictFromChatGPT.Add("فراغات", new List<string>
            {
                "يوسف ______هواي",
                "اين تقع_________",
                "كم كيلو_______",
                "ماذا تفعل_______",

            }


          );

            QuestionsDictFromChatGPT.Add("تعاليل", new List<string>
            {
                "ماذا لو",
                "هي او تلك",
                "لماذا نحن هنا",
                "سبب الصلع",

            }
            );


            QuestionsDictFromChatGPT.Add("صح او خطأ", new List<string>
            {
                "العراق سعيد",
                "البصرة بارده",
                "يوسف ما يسوي مشاكل",
                "الزواج حلو؟",

            }
           );
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
        //


        //اجراء خاص بجات جي بي تي ,حيث يتم بناء النموذج بشكل يدوي مع تمرير انماط الاسئلة
        private readonly string? __openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");


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

           
            // مسح الدكشنري الحالي للأسئلة
            QuestionsDictFromChatGPT.Clear();

            await FillQuestionsDictionary(text, selectedStyles);




        }

        private  async Task FillQuestionsDictionary(string text, List<string> selectedStyles)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("قم بإنشاء 6 أسئلة لكل من الأنماط التالية: ");

            foreach (var style in selectedStyles)
            {
                stringBuilder.Append($"{style}, ");
            }

            stringBuilder.AppendLine($"بناءً على هذا المحتوى:\n{text}");

            IQD_MessageBox.Show("وصول", "تم الوصول الى دالة Fill");
            // استدعاء ChatGPT للحصول على الأسئلة (يجب أن تكون لديك دالة GetQuestionsFromChatGPT تعمل بشكل صحيح)
            string questionsResponse = await GetQuestionsFromChatGPT(stringBuilder.ToString());

            // تقسيم الأسئلة المسترجعة إلى قائمة
            List<string> questionList = questionsResponse.Split('\n')
                                                        .Where(q => !string.IsNullOrWhiteSpace(q))
                                                        .ToList();

            IQD_MessageBox.Show("دالة Fill", "تم الخروج من دالة جلب الاسئلة الان");

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
                IQD_MessageBox.Show("دالة GPT","تم الدخول الى الدالة", MessageBoxType.Warning);

                using HttpClient client = new HttpClient();

                if (string.IsNullOrEmpty(__openAiApiKey))
                    throw new Exception("API Key غير مضبوط! تأكد من إضافته.");

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {__openAiApiKey}");

                var requestBody = new
                {
                    model = "gpt-4-turbo",
                    messages = new[]
                    {
                new { role = "system", content = "أنت مساعد ذكاء اصطناعي يقوم بتحليل النصوص وإنشاء أسئلة فقط بدون إجابات." },
                new { role = "user", content = prompt }
            },
                    max_tokens = 300
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

            IQD_MessageBox.Show("دالة GPT", "تم الخروج من الدالة", MessageBoxType.Warning);

        }
        //


        //اجراء خاص بانشاء نموذج دينماكي بناء على تصميم جات جي بي تي

        private void GeneratePdfDynmaic(string fullPath)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            // معلومات المدرسة والامتحان
            string School = $"المادة: {CombStage.Text}, التاريخ: {DateTime.Now.ToShortDateString()}, الوقت: {txtExampleTime.Text}";
            string title = $"أسئلة طلبة الانتساب للعام الدراسي ({DateTime.Now.Year}/{DateTime.Now.Year - 1}), إدارة {txtSchoolName.Text}";

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
                        // العنوان والتاريخ والوقت
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().AlignLeft().Text($"{School}\n\n{DateTime.Now.ToShortDateString()}\n \n{DateTime.Now.ToShortTimeString()}\n").Bold().FontSize(12);
                            row.RelativeItem().AlignCenter().Text($"{title}\n\n{DateTime.Now.Year}\n\n{$"{txtNote.Text}/ملاحظة"}").Bold().FontSize(12);
                            row.RelativeItem().AlignRight().Text($"{DateTime.Now.Month}\n ").Bold().FontSize(12);
                        });

                        // خط فاصل تحت التفاصيل
                        column.Item().LineHorizontal(2).LineColor(Colors.Black);
                        column.Item().PaddingVertical(10);

                        // التحقق من وجود أسئلة في الدكشنري
                        if (QuestionsDictFromChatGPT.Count > 0)
                        {
                            int questionNumber = 1; // عداد لترقيم الأسئلة
                            int totalScore = 100; // المجموع الكلي للدرجات
                            int scorePerQuestion = totalScore / 6; // الدرجة لكل سؤال (بدون فواصل عشرية)

                            // أخذ أول 6 أسئلة فقط
                            var questions = QuestionsDictFromChatGPT
                                .SelectMany(kvp => kvp.Value) // دمج جميع الأسئلة في قائمة واحدة
                                .Take(6); // أخذ أول 6 أسئلة

                            // التكرار على الأسئلة
                            foreach (var question in questions)
                            {
                                // السؤال الرئيسي
                                column.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignRight().Text($"س{questionNumber}: {question} ({scorePerQuestion} درجة)").FontSize(12);
                                });

                                // فروع السؤال (يمكنك تعديلها حسب الحاجة)
                                column.Item().PaddingLeft(20).Column(subColumn =>
                                {
                                    subColumn.Item().Text("أ) فرع 1").FontSize(10);
                                    subColumn.Item().Text("ب) فرع 2").FontSize(10);
                                    subColumn.Item().Text("ج) فرع 3").FontSize(10);
                                });

                                questionNumber++; // زيادة العداد لترقيم الأسئلة
                            }
                        }
                        else
                        {
                            // إذا لم تكن هناك أسئلة
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().AlignCenter().Text("لا توجد أسئلة متاحة.").FontSize(12);
                            });
                        }

                        // تذييل الصفحة (مدرس المادة)
                        string TitleTecher = ":مدرس المادة", Techer = $"{txtTeacherName.Text}";
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().AlignLeft().Text($"\n\n{TitleTecher}\n{Techer}\t\t\t\t\t").Bold().FontSize(12);
                        });
                    });
                });
            })
            .GeneratePdf(fullPath);

            // إعلام المستخدم بنجاح الحفظ
            IQD_MessageBox.Show("تم حفظ ملف PDF بنجاح!", "نجاح", MessageBoxType.Info);
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
                IQD_MessageBox.Show("Error", $"Failed to open PDF: {ex.Message}", MessageBoxType.Error);
            }
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
                    GeneratePdfDynmaic(fullPath);
                    OpenPdf(fullPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }


        // تحديد الحد الأقصى للأحرف لتجنب استهلاك زائد لـ API
        //int maxCharacters = 3000;
        //if (text.Length > maxCharacters)
        //    text = text.Substring(0, maxCharacters);


        //private async Task GenerateQuestionsFromText(string text)
        //{
        //    if (string.IsNullOrWhiteSpace(text))
        //    {
        //        IQD_MessageBox.Show("⚠️ الرجاء إدخال نص قبل المتابعة.", "تحذير", MessageBoxType.Warning);
        //        return;
        //    }

        //    // تحديد الحد الأقصى للأحرف لتجنب استهلاك زائد لـ API
        //    int maxCharacters = 2000;
        //    if (text.Length > maxCharacters)
        //        text = text.Substring(0, maxCharacters);

        //    // أنماط الأسئلة المطلوبة
        //    string[] questionStyles = { "أسئلة اختيار من متعدد", "أسئلة صح/خطأ", "أسئلة ملء الفراغ", "أسئلة مقالية" };

        //    foreach (var style in questionStyles)
        //    {
        //        string prompt = $"قم بإنشاء 6 أسئلة من النمط التالي: {style} بناءً على النص التالي:\n{text}";

        //        string questions = await GetQuestionsFromChatGPT(prompt);

        //        // تقسيم الأسئلة إلى قائمة
        //        List<string> questionList = questions.Split('\n')
        //                                             .Where(q => !string.IsNullOrWhiteSpace(q))
        //                                             .Take(6) // تأكد من أن عدد الأسئلة لا يتجاوز 6
        //                                             .ToList();

        //        // إضافة النمط والأسئلة إلى الدكشنري
        //        QuestionsDictFromChatGPT[style] = questionList;
        //    }


        //}


        //private async Task GenerateQuestionsFromTextDaynmic(string text, List<string> selectedStyles)
        //{
        //    if (string.IsNullOrWhiteSpace(text))
        //    {
        //        IQD_MessageBox.Show("⚠️ الرجاء إدخال نص قبل المتابعة.", "تحذير", MessageBoxType.Warning);
        //        return;
        //    }

        //    // تحديد الحد الأقصى للأحرف لتجنب استهلاك زائد لـ API
        //    int maxCharacters = 2000;
        //    if (text.Length > maxCharacters)
        //        text = text.Substring(0, maxCharacters);

        //    // مسح الدكشنري الحالي للأسئلة
        //    QuestionsDictFromChatGPT.Clear();

        //    // إنشاء الأسئلة لكل نمط مختار
        //    foreach (var style in selectedStyles)
        //    {
        //        string prompt = $"قم بإنشاء 6 أسئلة من النمط التالي: {style} بناءً على النص التالي:\n{text}";

        //        string questions = await GetQuestionsFromChatGPT(prompt);

        //        // تقسيم الأسئلة إلى قائمة
        //        List<string> questionList = questions.Split('\n')
        //                                            .Where(q => !string.IsNullOrWhiteSpace(q))
        //                                            .Take(6) // تأكد من أن عدد الأسئلة لا يتجاوز 6
        //                                            .ToList();

        //        // إضافة النمط والأسئلة إلى الدكشنري
        //        QuestionsDictFromChatGPT[style] = questionList;
        //    }
        //}
        //

        ////رفع ملف المادة
        //private async void  UploadQustiones_Click(object sender, RoutedEventArgs e)
        //{
        //    // فتح ملف PDF
        //    OpenFileDialog openFileDialog = new OpenFileDialog
        //    {
        //        Filter = "PDF Files (*.pdf)|*.pdf"
        //    };

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        //عرض يوزر كنترول الانتظار
        //        load = new IQD_LoadingControl();
        //        load.ShowDialog();

        //        //الانتظار ثانتين
        //       await Task.Delay(2000);

        //        // استخراج النص من PDF
        //         extractedText = ExtractTextFromPdf(openFileDialog.FileName);

        //         await GenerateQuestionsFromText(extractedText);

        //        load.Close();

        //        IQD_MessageBox.Show("تم انشاء الاسئلة بنجاح!", "نجاح");
        //    }

        //}
    }



    public class QuestionItem
    {
        public string? Question { get; set; }
        public bool IsSelected { get; set; }
    }
}