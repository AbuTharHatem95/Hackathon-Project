using System.Text.RegularExpressions;

namespace Interface.Helper
{
    public static class clsValidationHelper
    {

        // التحقق من صحة الإيميل
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // استخدام تعبير منتظم للتحقق من صحة الإيميل
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        // التحقق من أن النص غير فارغ
        public static bool IsNotEmpty(string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }

        // التحقق من أن النص يحتوي على أرقام فقط
        public static bool IsNumeric(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return Regex.IsMatch(text, @"^\d+$");
        }

        // التحقق من أن النص يحتوي على أحرف فقط
        public static bool IsAlphabetic(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return Regex.IsMatch(text, @"^[a-zA-Z\s]+$");
        }

        // التحقق من أن النص يحتوي على أحرف وأرقام فقط
        public static bool IsAlphaNumeric(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return Regex.IsMatch(text, @"^[a-zA-Z0-9\s]+$");
        }

        // التحقق من أن النص يحتوي على طول محدد
        public static bool IsLengthValid(string text, int minLength, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return text.Length >= minLength && text.Length <= maxLength;
        }

        // التحقق من أن النص يطابق تعبيرًا منتظمًا محددًا
        public static bool MatchesPattern(string text, string pattern)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(pattern))
                return false;

            return Regex.IsMatch(text, pattern);
        }

        // التحقق من أن النص يبدأ بحرف معين
        public static bool StartsWith(string text, string prefix)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(prefix))
                return false;

            return text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
        }

        // التحقق من أن النص ينتهي بحرف معين
        public static bool EndsWith(string text, string suffix)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(suffix))
                return false;

            return text.EndsWith(suffix, StringComparison.OrdinalIgnoreCase);
        }

        // التحقق من أن النص يحتوي على حرف معين
        public static bool Contains(string text, string substring)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(substring))
                return false;

            return text.Contains(substring, StringComparison.OrdinalIgnoreCase);
        }

        // التحقق من أن النص لا يحتوي على أحرف خاصة
        public static bool NoSpecialCharacters(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return !Regex.IsMatch(text, @"[^a-zA-Z0-9\s]");
        }

        // التحقق من أن النص يحتوي على حرف كبير وحرف صغير ورقم
        public static bool IsStrongPassword(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return Regex.IsMatch(text, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$");
        }


    }

}



