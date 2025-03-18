using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Interface
{
   public class clsPoint :absScore
    {

        public string Text { get; set; }

        public clsPoint(string text)
        {
            Text = text;
        }
    }
}
