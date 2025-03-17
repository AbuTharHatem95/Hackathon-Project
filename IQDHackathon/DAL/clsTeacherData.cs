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
    public static class clsTeacherData
    {
        public static async Task<int?> AddAsync(string teacherName)
        {

            SqlParameter[] parameters =
            {
                new SqlParameter("@TeacherName", teacherName),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int? teacherId, string teacherName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TeacherId", teacherId),
                new SqlParameter("@TeacherName", teacherName),
            };
            return await CRUD.UpdateAsync("Sp_Update", parameters);
        }

        public static async Task<bool> DeleteAsync(int? teacherId)
        {
            if (teacherId == null) return false;
            return await CRUD.DeleteAsync("SP_Delete", "TeacherId", teacherId);
        }

        public static async Task<Dictionary<string, object>?> GetByIdAsync(int teacherId) => await CRUD.GetByColumnValueAsync("Sp_GetSettingsById", "TeacherId", teacherId);

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("Sp_GetAll");

    }
}
