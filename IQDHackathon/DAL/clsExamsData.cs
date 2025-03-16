using Microsoft.Data.SqlClient;
using MyLib_DotNet.DatabaseExecutor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class clsExamsData
    {
        public static async Task<int?> AddAsync(int stageInfoId, int teacherId, char branch, DateTime examDate, int examTypeId, TimeOnly examTime)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@StageInfoId", stageInfoId),
                new SqlParameter("@TeacherId", teacherId),
                new SqlParameter("@Branch", branch),
                new SqlParameter("@ExamDate", examDate),
                new SqlParameter("@ExamTypeId", examTypeId),
                new SqlParameter("@ExamTime", examTime),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int stageInfoId, int teacherId, char branch, DateTime examDate, int examTypeId, TimeOnly examTime)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@StageInfoId", stageInfoId),
                new SqlParameter("@TeacherId", teacherId),
                new SqlParameter("@Branch", branch),
                new SqlParameter("@ExamDate", examDate),
                new SqlParameter("@ExamTypeId", examTypeId),
                new SqlParameter("@ExamTime", examTime),
            };
            return await CRUD.UpdateAsync("Sp_Update", parameters);
        }

        public static async Task<bool> DeleteAsync(int? examId)
        {
            if (examId == null) return false;
            return await CRUD.DeleteAsync("Sp_Delete", "ExamId", examId);
        }

        public static async Task<Dictionary<string, object>?> GetByIdAsync(int examId) => await CRUD.GetByColumnValueAsync("Sp_GetSettingsById", "ExamId", examId);

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("Sp_GetAll");


    }
}
