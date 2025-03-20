namespace Interface
{
    public class clsTitle : absNumberOfAnswer
    {
        public byte Number { get; set; }

        public string? QuestionTitle { get; set; }

        public clsTitle(byte number, float score, byte numberOfAnswer, string? questionTitle = null)
        {
            Number = number;
            Score = score;
            NumberOfAnswer = numberOfAnswer;
            QuestionTitle = questionTitle;
        }
    }

}
