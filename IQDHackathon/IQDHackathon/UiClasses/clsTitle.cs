using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Interface
{
    public  class clsTitle :absNumberOfAnswer
    {
        public byte Number { get; set; }

        public string? QuestionTitle { get; set; }

        public byte ScoreForBranchOrPint { get; private set; }

        public clsTitle(byte number, byte score, byte numberOfAnswer, string? questionTitle = null)
        {
            Number = number;
            Score = score;
            NumberOfAnswer = numberOfAnswer;
            QuestionTitle = questionTitle;
            ScoreForBranchOrPint = (byte)(Score / NumberOfAnswer);
        }

    }
}
