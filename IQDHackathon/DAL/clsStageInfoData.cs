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
    public static class clsStageInfoData
    {
        public static async Task<int?> AddAsync(int questionPermssions, int stage)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@QuestionPermssions", questionPermssions),
                new SqlParameter("@Stage", stage),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int stageInfoId, int questionPermssions, int stage)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@StageInfoId", stageInfoId),
                new SqlParameter("@QuestionPermssions", questionPermssions),
                new SqlParameter("@Stage", stage)
            };
            return await CRUD.UpdateAsync("Sp_Update", parameters);
        }

        public static async Task<bool> DeleteAsync(int? stageInfoId)
        {
            if (stageInfoId == null) return false;
            return await CRUD.DeleteAsync("SP_Delete", "StageInfoId", stageInfoId);
        }

        public static async Task<Dictionary<string, object>?> GetByIdAsync(int stageInfoId) => await CRUD.GetByColumnValueAsync("Sp_GetSettingsById", "TeacherId", stageInfoId);

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("Sp_GetAll");

    }
}
