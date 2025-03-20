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
            stringBuilder.Append("قم بإنشاء 6 أسئلة لكل من الأنماط التالية: ");
            foreach (string Type in QuestionTypes)
            {
                stringBuilder.Append($"{Type}, ");
            }
            stringBuilder.AppendLine($"بناءً على هذا المحتوى:\n{pdfContent}");
            stringBuilder.AppendLine("بدون ترقيم الاسئلة");

            return stringBuilder;

        }


        public  static async Task<string?> GenerateQuestionsByPrompt(string prompt)
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


        public static async Task<Dictionary<string, List<string>>?> QuestionsWithTypes(List<string>QuestionTypes, string pdfContent)
        {
            Dictionary<string, List<string>> questionsDictFromChatGPT = new Dictionary<string, List<string>>();
            List<string>? questions = await GenerateQuestionsInListByPdfContent(pdfContent, QuestionTypes);
            if (questions == null || questions.Count == 0) return null;
            int index = 0;
            foreach (var style in QuestionTypes)
            {
                if (!questionsDictFromChatGPT.ContainsKey(style))
                {
                    questionsDictFromChatGPT[style] = new List<string>();
                }

                // أخذ 6 أسئلة لكل نمط (إذا كانت متوفرة)
                var styleQuestions = questions.Skip(index).Take(6).ToList();
                questionsDictFromChatGPT[style].AddRange(styleQuestions);

                index += 6; // الانتقال إلى الأسئلة التالية لكل نمط
            }

            return questionsDictFromChatGPT;
        }



    }
}
