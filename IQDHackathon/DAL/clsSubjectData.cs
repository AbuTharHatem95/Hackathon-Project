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
        public static async Task<bool> UpdateAsync(int subjectId, string subjectName)
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

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("Sp_GetAll");


    }
}
