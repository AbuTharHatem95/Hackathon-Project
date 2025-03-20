namespace Interface
{
    public class clsQuestion
    {
        public clsTitle Title { get; set; }

        public Dictionary<char, clsBranch>? BranchzDict { get; private set; }

        public List<clsPoint>? PointList { get; private set; }

        public static Dictionary<byte, clsQuestion> QuestionsDict { get; private set; } = new Dictionary<byte, clsQuestion>();

        public clsQuestion(clsTitle title)
        {
            Title = title;
        }

        public clsQuestion AddBranch(char Char)
        {
            if (BranchzDict == null) BranchzDict = new();
            BranchzDict.Add(Char, new clsBranch(Char) { Score = Title.ScoreForBranchOrPint });
            return this;
        }

        public clsQuestion AddPointToBranch(char Char, clsPoint point)
        {
            if (BranchzDict == null) BranchzDict = new();
            point.Score = (float)(BranchzDict[Char].Score / BranchzDict[Char].NumberOfAnswer);
            BranchzDict[Char].PointList.Add(point);
            return this;
        }

        public clsQuestion AddPoint(clsPoint point)
        {
            if (PointList == null) PointList = new();
            point.Score = Title.ScoreForBranchOrPint;
            PointList.Add(point);
            return this;
        }

        public void CreateQuestion()
        {
            if (QuestionsDict.ContainsKey(Title.Number)) 
                QuestionsDict[Title.Number] = this;
            else
                QuestionsDict.Add(Title.Number, this);
        }
    }
}
