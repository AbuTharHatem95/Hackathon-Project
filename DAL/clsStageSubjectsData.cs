using Microsoft.Data.SqlClient;
using MyLib_DotNet.DatabaseExecutor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class clsStageSubjectsData
    {

        public static async Task<int?> AddAsync(byte stClass, int? StageId, int? subjectId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Class", stClass),
                new SqlParameter("@StageId", StageId),
                new SqlParameter("@SubjectId", subjectId),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int? stageSubjectId, byte stClass, int? StageId, int? subjectId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@StageSubjectId", stageSubjectId),
                new SqlParameter("@StageId", StageId),
                new SqlParameter("@Class", stClass),
                new SqlParameter("@SubjectId", subjectId)

            };
            return await CRUD.UpdateAsync("Sp_Update", parameters);
        }

        public static async Task<bool> DeleteAsync(int? subjectStageId)
        {
            if (subjectStageId == null) return false;
            return await CRUD.DeleteAsync("SP_Delete", "SubjectStageId", subjectStageId);
        }

        public static async Task<Dictionary<string, object>?> GetByIdAsync(int subjectStageId) => await CRUD.GetByColumnValueAsync("Sp_GetSettingsById", "StageInfoId", subjectStageId);

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("Sp_GetAll");

    }
}
