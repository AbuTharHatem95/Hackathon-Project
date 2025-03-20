using DVLD_DataAccess;
using Microsoft.Data.SqlClient;
using MyLib_DotNet.DatabaseExecutor;
using System.Data;

namespace DAL
{
    public static class clsSubjectData
    {

        public static async Task<int?> AddAsync(string subjectName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@SubjectName", subjectName),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int? subjectId, string subjectName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@SubjectId", subjectId),
                new SqlParameter("@SubjectName", subjectName),
            };
            return await CRUD.UpdateAsync("Sp_Update", parameters);
        }

        public static async Task<bool> DeleteAsync(int? subjectId)
        {
            if (subjectId == null) return false;
            return await CRUD.DeleteAsync("SP_Delete", "SubjectId", subjectId);
        }

        public static async Task<Dictionary<string, object>?> GetByIdAsync(int subjectId) => await CRUD.GetByColumnValueAsync("Sp_GetSettingsById", "SubjectId", subjectId);

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("sp_GetAllSubjects");

        public static DataTable GetAll()
        {
            string Query = "Select * From Subjects";
            SqlCommand cmd = new SqlCommand(Query);
            return CRUDs.GetAll(cmd);
        }
        public static DataTable GetSubjectsByStageAndClass(int  stageId)
        {
            string Query = @"Select distinct Subjects.SubjectId, Subjects.SubjectName from StageSubjects
                                inner join Subjects On Subjects.SubjectId = StageSubjects.SubjectId 
                                inner join Stages On Stages.StageId = StageSubjects.StageId
                                Where Stages.StageId = @StageId;";
            SqlCommand cmd = new SqlCommand(Query);
            cmd.Parameters.AddWithValue("@StageId", stageId);
            return CRUDs.GetAll(cmd);

        }
      


    }
}
