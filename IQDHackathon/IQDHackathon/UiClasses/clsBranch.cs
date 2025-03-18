using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
   public  class clsBranch : absNumberOfAnswer
    {
        public char Char { get; set; }

        public string? BranchTitle { get; set; }

        public List<clsPoint> PointList { get; private set; }

        public clsBranch(char Char, string? branchTitle = null)
        {
            this.Char = Char;
            BranchTitle = branchTitle;
            PointList = new();
        }


    }
}
