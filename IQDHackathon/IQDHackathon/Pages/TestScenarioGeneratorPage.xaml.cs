using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Interface.Pages.UserControles;
using IQD_UI_Library;
using IQDHackathon;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Interface.Pages
{
    public partial class TestScenarioGeneratorPage : Page
    {
        public static Dictionary<string, List<string>> QuestionsDictFromChatGPT = new Dictionary<string, List<string>>();

        // public ObservableCollection<QuestionStyle> QuestionStyles { get; set; }

        private List<QuestionItem> __generatedQuestions = new List<QuestionItem>();

        private readonly string? __openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");


        //بيج لاملائ معلومات المدرسه ومعلومات الاستاذ ورفع مادة الامتحان 
        public TestScenarioGeneratorPage()
        {
            InitializeComponent();
            FillComboBox();

            // جلب البيانات من قاعدة البيانات
            //var databaseService = new DatabaseService();
            //QuestionStyles = new ObservableCollection<QuestionStyle>(databaseService.GetQuestionStyles());

            //// تعيين مصدر البيانات لـ ItemsControl
            //CheckBoxList.ItemsSource = QuestionStyles;
        }

        private void Gentet_Click(object sender, RoutedEventArgs e)
        {
            GenretQuestiones();

            if (QuestionsDictFromChatGPT.Count == 0)
            {
                IQD_MessageBox.Show("Erorr", "الدشكنري فااارغ", MessageBoxType.Error);
                return;
            }
            MainPageGrid.Visibility = Visibility.Collapsed;
            QuestionsPage.Visibility = Visibility.Visible;
            ContentFrame.Navigate(new AddQustiones()); 
        }

        private void FillComboBox()
        {
            // إضافة عناصر إلى ComboBox
            txtSubject1.Items.Add("الرياضيات");
            txtSubject1.Items.Add("الاحياء");
            txtSubject1.Items.Add("ثالث");
            txtSubject1.Items.Add("رابع");
            txtSubject1.SelectedIndex = 0;
        }

        private void BackToMainWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void LoadPdf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string extractedText = ExtractTextFromPdf(openFileDialog.FileName);
                GenerateQuestionsFromText(extractedText);
            }
        }

        private string ExtractTextFromPdf(string filePath)
        {
            using var reader = new iText.Kernel.Pdf.PdfReader(filePath);
            using var pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader);

            string extractedText = "";

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                extractedText += iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)) + "\n";
            }

            return extractedText;
        }

        private async void GenerateQuestionsFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                IQD_MessageBox.Show("⚠️ الرجاء إدخال نص قبل المتابعة.", "تحذير", MessageBoxType.Warning);
                return;
            }

            // تحديد الحد الأقصى للأحرف لتجنب استهلاك زائد لـ API
            int maxCharacters = 2000;
            if (text.Length > maxCharacters)
                text = text.Substring(0, maxCharacters);

            string prompt = $"قم بتحويل النص التالي إلى أسئلة بدون إجابات:\n{text}";

            string questions = await GetQuestionsFromChatGPT(prompt);

            __generatedQuestions = new List<QuestionItem>();
            foreach (string q in questions.Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(q))
                {
                    __generatedQuestions.Add(new QuestionItem { Question = q.Trim(), IsSelected = false });
                }
            }
            //  QuestionsList.ItemsSource = generatedQuestions;
        }


        private async Task<string> GetQuestionsFromChatGPT(string prompt)
        {
            try
            {
                using HttpClient client = new HttpClient();

                if (string.IsNullOrEmpty(__openAiApiKey))
                    throw new Exception("API Key غير مضبوط! تأكد من إضافته.");

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {__openAiApiKey}");

                // تقسيم النص إذا كان أكبر من 2000 حرف
                const int maxChunkSize = 2000;
                List<string> textChunks = new List<string>();
                for (int i = 0; i < prompt.Length; i += maxChunkSize)
                {
                    textChunks.Add(prompt.Substring(i, Math.Min(maxChunkSize, prompt.Length - i)));
                }

                List<string> results = new List<string>();

                foreach (var chunk in textChunks)
                {
                    var requestBody = new
                    {
                        model = "gpt-4",
                        messages = new[]
                        {
                            new { role = "system", content = "أنت مساعد ذكاء اصطناعي يقوم بتحليل النصوص وإنشاء أسئلة فقط بدون إجابات." },
                            new { role = "user", content = chunk }
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

                    results.Add(responseData?.choices[0].message.content.ToString());
                }

                return string.Join("\n\n", results);
            }
            catch (Exception ex)
            {
                IQD_MessageBox.Show("خطأ", ex.Message, MessageBoxType.Error);
                return "حدث خطأ أثناء جلب الأسئلة من ChatGPT.";
            }
        }

        private void SaveQuestions_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text File (*.txt)|*.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var selectedQuestions = __generatedQuestions.FindAll(q => q.IsSelected);
                File.WriteAllLines(saveFileDialog.FileName, selectedQuestions.ConvertAll(q => q.Question));
                MessageBox.Show("تم حفظ الأسئلة بنجاح!");
            }
        }

        //private void Gentet_Click(object sender, RoutedEventArgs e)
        //{
        //    GenretQuestiones();
        //    if (QustionesDict.Count <= 0)
        //    {
        //        IQD_MessageBox.Show("Erorr", "الدشكنري فااارغ", MessageBoxType.Error);
        //        return;
        //    }
        //    mainGrid.Visibility = Visibility.Collapsed;
        //    FramGrid.Visibility=Visibility.Visible;
        //    InnerFrame?.Navigate(new QuestionsStyles(QustionesDict));
        //}


        

        //السترنك الاول يحتوي على النمط
        //والليست تحتوي على الاسئلة المدرجة تحت النمط

        private void GenretQuestiones()
        {
            QuestionsDictFromChatGPT.Add("عدد", new List<string>
            {
                "معالج",
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

        private void PrintQuestions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRetuntomainmenue_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }
    }

  

    public class QuestionItem
    {
        public string? Question { get; set; }
        public bool IsSelected { get; set; }
    }
}