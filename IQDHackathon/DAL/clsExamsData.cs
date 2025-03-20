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

        public static async Task<int?> AddAsync(int? stageSubjectId, int? teacherId, char branch, DateTime examDate, int? examTypeId, TimeSpan examTime)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@StageSubjectId", stageSubjectId),
                new SqlParameter("@TeacherId", teacherId),
                new SqlParameter("@Branch", branch),
                new SqlParameter("@ExamDate", examDate),
                new SqlParameter("@ExamTypeId", examTypeId),
                new SqlParameter("@ExamTime", examTime),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int? examId, int? stageSubjectId, int? teacherId, char branch, DateTime examDate, int? examTypeId, TimeSpan examTime)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ExamId", examId),
                new SqlParameter("@StageSubjectId", stageSubjectId),
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

        //انشاء ستورد بوسيجر تبحث هل هناك بيانات في الجدول اذا نعم ترجع دشنري مليان اذا لا ترجع نلل
        public static async Task<Dictionary<string, object>?> GetLastExam() => await CRUD.GetAsync("Sp_SelectTop 1 From Exams", null);


        //ستور بروسيجر تحذف تيبلات التيجر و تحذف ايضا الاكزام تيبلز 
        public static async Task<bool> DeleteAllTablesConnectsWithTheExamTable() => await CRUD.DeleteAsync("Sp_DeleteAllDataInTables", null);


    }
}
