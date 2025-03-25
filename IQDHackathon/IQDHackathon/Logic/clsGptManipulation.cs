using Interface.Logic;
using IQD_UI_Library;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Interface.LogicClasses
{
    public static class clsGptManipulation
    {
        public static StringBuilder GetGptPromptByPdfContent(string pdfContent, List<string> QuestionTypes)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("قم بإنشاء 6 أسئلة لكل من الأنماط التالية بناءً على هذا المحتوى دون إضافة أرقام تلقائية في بداية الأسئلة:");
            stringBuilder.AppendLine("------------------------------------------------");
            stringBuilder.AppendLine(pdfContent);
            stringBuilder.AppendLine("------------------------------------------------");
            stringBuilder.AppendLine("يجب أن يكون التنسيق كما يلي:");
            stringBuilder.AppendLine("نمط السؤال: [اسم النمط]");
            stringBuilder.AppendLine("- نص السؤال الأول");
            stringBuilder.AppendLine("....");
            stringBuilder.AppendLine("------------------------------------------------");

            foreach (string Type in QuestionTypes)
            {
                stringBuilder.AppendLine($"[نمط السؤال: {Type}]");
            }

            return stringBuilder;
        }



        public static async Task<string?> GenerateQuestionsByPrompt(string prompt)
        {
            if (string.IsNullOrEmpty(prompt)) return null;
            if (string.IsNullOrEmpty(clsGlobal.AISetting.ApiKey))
                throw new Exception("API Key غير مضبوط! تأكد من إضافته.");
            try
            {

                using HttpClient client = new HttpClient();

               

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {clsGlobal.AISetting.ApiKey}");

                // "gpt-4"
                //"gpt-4-turbo"
                var requestBody = new
                {
                    model = clsGlobal.AISetting.ModelName,
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

        public static async Task<List<string>?> GenerateQuestionsInListByPdfContent(string pdfContent, List<string> QuestionTypes)
        {
            string prompt = GetGptPromptByPdfContent(pdfContent, QuestionTypes).ToString();
            if (string.IsNullOrEmpty(prompt)) return null;
            string? Questions = await GenerateQuestionsByPrompt(prompt);
            if(Questions == null) return null;
            return Questions.Split('\n').Where(q => !string.IsNullOrWhiteSpace(q)).ToList();

        }


        public static async Task<Dictionary<string, List<string>>?> QuestionsWithTypes(List<string> QuestionTypes, string pdfContent)
        {
            Dictionary<string, List<string>> questionsDictFromChatGPT = new Dictionary<string, List<string>>();

            string? responseText = await GenerateQuestionsByPrompt(GetGptPromptByPdfContent(pdfContent, QuestionTypes).ToString());

            if (string.IsNullOrEmpty(responseText)) return null;

            // 🔍 طباعة النتيجة للتحقق من شكل البيانات القادمة من ChatGPT
            Console.WriteLine("Response from ChatGPT:\n" + responseText);

            // تقسيم النص بناءً على الأنماط
            string[] sections = responseText.Split(new[] { "[نمط السؤال:" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var section in sections)
            {
                string[] lines = section.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length < 2) continue;

                // استخراج اسم النمط
                string style = lines[0].Split(']').FirstOrDefault()?.Trim();
                if (string.IsNullOrEmpty(style)) continue;

                // البحث عن أقرب تطابق مع الأنماط المتاحة
                string? matchedStyle = QuestionTypes.FirstOrDefault(qt => qt.Contains(style, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(matchedStyle)) continue;

                // استخراج الأسئلة
                List<string> extractedQuestions = lines.Skip(1).Select(q => q.Trim()).Where(q => q.Length > 3).ToList();
                questionsDictFromChatGPT[matchedStyle] = extractedQuestions;
            }

            return questionsDictFromChatGPT;
        }



    }
}
