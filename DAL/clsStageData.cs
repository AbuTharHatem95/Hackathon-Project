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
    public static class clsStageData
    {


        public static async Task<int?> AddAsync(string stageName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@stageName", stageName),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int? stageId, string stageName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@StageId", stageId),
                new SqlParameter("@StageName", stageName),
            };
            return await CRUD.UpdateAsync("Sp_Update", parameters);
        }

        public static async Task<bool> DeleteAsync(int? stageId)
        {
            if (stageId == null) return false;
            return await CRUD.DeleteAsync("SP_Delete", "StageId", stageId);
        }

        public static async Task<Dictionary<string, object>?> GetByIdAsync(int stageId) => await CRUD.GetByColumnValueAsync("Sp_GetSettingsById", "StageId", stageId);

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("Sp_GetAll");




    }
}
