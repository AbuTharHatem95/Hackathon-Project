using Interface.Pages.UserControles;
using IQD_UI_Library;
using IQDHackathon;
using Microsoft.Win32;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages
{
    /// <summary>
    /// Interaction logic for QuestionsPage.xaml
    /// </summary>
    public partial class QuestionsPage : Page
    {
        //يحتوي على البيج على ليست فيو الرئيسية
        private Dictionary<string, List<string>> qustiones;

        public QuestionsPage(Dictionary<string, List<string>> dict)
        {
            InitializeComponent();
            GentetListViewComponat(dict);
        }

        private void GentetListViewComponat(Dictionary<string, List<string>> qustiones)
        {
            foreach (var style in qustiones)
            {
                // إنشاء عنصر تحكم ديناميكي
                ctrlDynamicListControl listView = new ctrlDynamicListControl(style.Key, style.Value);

                // الاشتراك في الحدث الجديد
                listView.QuestionStateChanged += ListView_QuestionStateChanged;

                // إضافة العنصر إلى الواجهة
                ItemsListBox.Items.Add(listView);
            }
        }

        private void ListView_QuestionStateChanged(object sender, (string QuestionStyle, bool IsChecked, string Score) e)
        {
            // هنا يمكنك معالجة البيانات الواردة
            var questionStyle = e.QuestionStyle;
            var isChecked = e.IsChecked;
            var score = e.Score;

            // عرض البيانات في MessageBox (أو حفظها في قائمة أو أي شيء آخر)
            MessageBox.Show($"نمط السؤال: {questionStyle}\nالتحديد: {isChecked}\nالدرجة: {score}");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                if (this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack(); 
                }
                else
                {
                    MainWindow.CloseWindow = true;
                }
            }
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

        private void BackToMainWindowButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCreateQustion1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrintQustion_Click(object sender, RoutedEventArgs e)
        {

        }



        //// تنفيذ دالة GetEnumerator من واجهة IEnumerable
        //public IEnumerator GetEnumerator()
        //{
        //    return ItemsListBox.Items.GetEnumerator(); // إرجاع مكرر لعناصر ItemsListBox
        //}



        //private List<QuestionItem> generatedQuestions = new List<QuestionItem>();

        //private readonly string? OpenAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        //private void LoadPdf_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog
        //    {
        //        Filter = "PDF Files (*.pdf)|*.pdf"
        //    };

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        string extractedText = ExtractTextFromPdf(openFileDialog.FileName);
        //        GenerateQuestionsFromText(extractedText);
        //    }
        //}

        //private string ExtractTextFromPdf(string filePath)
        //{
        //    using var reader = new iText.Kernel.Pdf.PdfReader(filePath);
        //    using var pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader);

        //    string extractedText = "";

        //    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
        //    {
        //        extractedText += iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)) + "\n";
        //    }

        //    return extractedText;
        //}

        //private async void GenerateQuestionsFromText(string text)
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

        //    string prompt = $"قم بتحويل النص التالي إلى أسئلة بدون إجابات:\n{text}";

        //    string questions = await GetQuestionsFromChatGPT(prompt);

        //    generatedQuestions = new List<QuestionItem>();
        //    foreach (string q in questions.Split('\n'))
        //    {
        //        if (!string.IsNullOrWhiteSpace(q))
        //        {
        //            generatedQuestions.Add(new QuestionItem { Question = q.Trim(), IsSelected = false });
        //        }
        //    }

        //  //  QuestionsList.ItemsSource = generatedQuestions;
        //}


        //private async Task<string> GetQuestionsFromChatGPT(string prompt)
        //{
        //    try
        //    {
        //        using HttpClient client = new HttpClient();

        //        if (string.IsNullOrEmpty(OpenAiApiKey))
        //            throw new Exception("API Key غير مضبوط! تأكد من إضافته.");

        //        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {OpenAiApiKey}");

        //        // تقسيم النص إذا كان أكبر من 2000 حرف
        //        const int maxChunkSize = 2000;
        //        List<string> textChunks = new List<string>();
        //        for (int i = 0; i < prompt.Length; i += maxChunkSize)
        //        {
        //            textChunks.Add(prompt.Substring(i, Math.Min(maxChunkSize, prompt.Length - i)));
        //        }

        //        List<string> results = new List<string>();

        //        foreach (var chunk in textChunks)
        //        {
        //            var requestBody = new
        //            {
        //                model = "gpt-4",
        //                messages = new[]
        //                {
        //            new { role = "system", content = "أنت مساعد ذكاء اصطناعي يقوم بتحليل النصوص وإنشاء أسئلة فقط بدون إجابات." },
        //            new { role = "user", content = chunk }
        //        },
        //                max_tokens = 300
        //            };

        //            string jsonBody = JsonConvert.SerializeObject(requestBody);
        //            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //            HttpResponseMessage response;
        //            try
        //            {
        //                response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
        //            }
        //            catch (HttpRequestException httpEx)
        //            {
        //                throw new Exception("❌ فشل الاتصال بـ OpenAI. تحقق من اتصالك بالإنترنت.", httpEx);
        //            }

        //            if (!response.IsSuccessStatusCode)
        //                throw new Exception($"خطأ في الاتصال بـ OpenAI: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

        //            string responseString = await response.Content.ReadAsStringAsync();
        //            dynamic? responseData = JsonConvert.DeserializeObject(responseString);

        //            if (responseData?.choices == null || responseData?.choices.Count == 0)
        //                throw new Exception("لم يتم استلام بيانات صحيحة من OpenAI!");

        //            results.Add(responseData?.choices[0].message.content.ToString());
        //        }

        //        return string.Join("\n\n", results);
        //    }
        //    catch (Exception ex)
        //    {
        //        IQD_MessageBox.Show("خطأ", ex.Message, MessageBoxType.Error);
        //        return "حدث خطأ أثناء جلب الأسئلة من ChatGPT.";
        //    }
        //}

        //private void SaveQuestions_Click(object sender, RoutedEventArgs e)
        //{
        //    SaveFileDialog saveFileDialog = new SaveFileDialog
        //    {
        //        Filter = "Text File (*.txt)|*.txt"
        //    };

        //    if (saveFileDialog.ShowDialog() == true)
        //    {
        //        var selectedQuestions = generatedQuestions.FindAll(q => q.IsSelected);
        //        File.WriteAllLines(saveFileDialog.FileName, selectedQuestions.ConvertAll(q => q.Question));
        //        MessageBox.Show("تم حفظ الأسئلة بنجاح!");
        //    }
        //}
    }















    //public class Title
    //{
    //    public byte Number { get; set; }

    //    public string? QuestionTitle { get; set; }

    //    public byte Score { get; set; }

    //    public List<Question> QuestionList { get; private set; }

    //    public Title(byte number, byte score, string? questionTitle = null)
    //    {
    //        Number = number;
    //        QuestionTitle = questionTitle;
    //        Score = score;
    //        QuestionList = new List<Question>();
    //    }
    //}

    //public class Question
    //{
    //    public char Branch { get; set; }

    //    public string Text { get; set; }

    //    public string QuestionStyle { get; set; }

    //    public byte Score { get; set; }

    //    public Question(char branch, string text, string questionStyle, byte score)
    //    {
    //        Branch = branch;
    //        Text = text;
    //        QuestionStyle = questionStyle;
    //        Score = score;
    //    }
    //}














    //public class QuestionItem
    //{
    //    public string? Question { get; set; }
    //    public bool IsSelected { get; set; }
    //}

}

