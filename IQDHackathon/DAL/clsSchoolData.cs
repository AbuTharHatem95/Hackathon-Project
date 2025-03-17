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
    public static class clsSchoolData
    {

        public static async Task<int?> AddAsync(string schoolName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@SchoolName", schoolName),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int? schoolId, string schoolName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@SchoolId", schoolId),
                new SqlParameter("@SchoolName", schoolName),
            };
            return await CRUD.UpdateAsync("Sp_Update", parameters);
        }

        public static async Task<bool> DeleteAsync(int? schoolId)
        {
            if (schoolId == null) return false;
            return await CRUD.DeleteAsync("SP_Delete", "SchoolId", schoolId);
        }

        public static async Task<Dictionary<string, object>?> GetByIdAsync(int schoolId) => await CRUD.GetByColumnValueAsync("Sp_GetSettingsById", "SchoolId", schoolId);

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("Sp_GetAll");


    }
}
