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
    public static class clsExamTypeData
    {
        public static async Task<int?> AddAsync(string typeName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeName", typeName),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int examTypeId, string typeName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ExamTypeId", examTypeId),
                new SqlParameter("@TypeName", typeName),
            };
            return await CRUD.UpdateAsync("Sp_Update", parameters);
        }

        public static async Task<bool> DeleteAsync(int? examTypeId)
        {
            if (examTypeId == null) return false;
            return await CRUD.DeleteAsync("SP_Delete", "examTypeId", examTypeId);
        }

        public static async Task<Dictionary<string, object>?> GetByIdAsync(int examTypeId) => await CRUD.GetByColumnValueAsync("Sp_GetSettingsById", "ExamTypeId", examTypeId);

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("Sp_GetAll");

    }
}
