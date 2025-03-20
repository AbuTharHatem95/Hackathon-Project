using DAL;
using System.Data;

namespace BLL
{
    public class clsExam : absBaseEntity
    {

        public clsStageSubject StageSubject { get; set; }
        public clsTeacher Teacher { get; set; }
        public char Branch { set; get; }
        public DateTime ExamDate { set; get; }
        public clsExamType ExamType { set; get; }
        public TimeSpan ExamTime { set; get; }


        public clsExam(clsStageSubject stageSubject, clsTeacher teacher, char branch, DateTime examDate, clsExamType examType, TimeSpan examTime)
        {
            StageSubject = stageSubject;
            Teacher = teacher;
            Branch = branch;
            ExamDate = examDate;
            ExamType = examType;
            ExamTime = examTime;

        }

        public clsExam(int examId, clsStageSubject stageSubject, clsTeacher teacher, char branch, DateTime examDate, clsExamType examType, TimeSpan examTime)
        {
            Id = examId;
            StageSubject = stageSubject;
            Teacher = teacher;
            Branch = branch;
            ExamDate = examDate;
            ExamType = examType;
            ExamTime = examTime;

        }


        public async Task<bool> SaveAsync()
        {
            if (!Id.HasValue)
                return await __AddAsync();
            return await __UpdateAsync();
        }

        private async Task<bool> __AddAsync()
        {
            Id = await clsExamsData.AddAsync(StageSubject.Id, Teacher.Id, Branch, ExamDate, ExamType.Id, ExamTime);
            return Id.HasValue;
        }

        public static async Task<clsExam?> GetByIdAsync(int examId)
        {
            var data = await clsExamsData.GetByIdAsync(examId);
            if (data == null) return null;
            return FetchData(ref data);
        }

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await clsExamTypeData.GetAllAsDataTableAsync();

        public static async Task<clsExam?> GetLastData()
        {
            var data = await clsExamsData.GetLastExam();
            if(data == null) return null;
            return FetchData(ref data);
        }

        private async Task<bool> __UpdateAsync() => await clsExamsData.UpdateAsync(Id, StageSubject.Id, Teacher.Id, Branch, ExamDate, ExamType.Id, ExamTime);

        public static async Task<bool> DeleteByIdAsync(int? stageSubjectId) => await clsExamTypeData.DeleteAsync(stageSubjectId);

        public async Task<bool> DeleteAsync() => await DeleteByIdAsync(Id);
        public static async Task<bool> DeleteSavedDataAsync() => await clsExamsData.DeleteAllTablesConnectsWithTheExamTable();

        internal static clsExam FetchData(ref Dictionary<string, object> dict) =>
            new clsExam((int)dict["ExamId"], clsStageSubject.FetchUserData(ref dict), clsTeacher.FetchData(ref dict), (char)dict["Branch"],
                (DateTime)dict["ExamDate"] , clsExamType.FetchData(ref dict), (TimeSpan) dict["ExamTime"]);




























    }
}
