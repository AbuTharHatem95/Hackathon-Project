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
            BranchzDict.Add(Char, new clsBranch(Char));
            return this;
        }

        public clsQuestion AddPointToBranch(char Char, clsPoint point)
        {
            if (BranchzDict == null) BranchzDict = new();
            if (!BranchzDict.ContainsKey(Char)) return this;

            clsBranch branch = BranchzDict[Char];
            branch.PointList.Add(point);
            return this;
        }

        public clsQuestion AddPoint(clsPoint point)
        {
            if (PointList == null) PointList = new();
            PointList.Add(point);
            return this;
        }

        public void CreateQuestion()
        {
            this.__DistributeScore();
            if (QuestionsDict.ContainsKey(Title.Number))
                QuestionsDict[Title.Number] = this;
            else
                QuestionsDict.Add(Title.Number, this);
        }

        private void __DistributeScore()
        {
            float totalScore = Title.Score;

            if (BranchzDict != null && BranchzDict.Count > 0)
            {
                float branchScore = totalScore / BranchzDict.Count;
                foreach (var branch in BranchzDict.Values)
                {
                    branch.Score = branchScore;

                    if (branch.PointList.Count > 0)
                    {
                        float pointScore = branchScore / branch.PointList.Count;
                        foreach (var point in branch.PointList)
                        {
                            point.Score = pointScore;
                        }
                    }
                }
            }
            else if (PointList != null && PointList.Count > 0)
            {
                float pointScore = totalScore / PointList.Count;
                foreach (var point in PointList)
                {
                    point.Score = pointScore;
                }
            }
        }
    }

}
