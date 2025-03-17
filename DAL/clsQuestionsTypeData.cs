using Microsoft.Data.SqlClient;
using MyLib_DotNet.DatabaseExecutor;
using System.Data;

namespace DAL
{
    public static class clsQuestionsTypeData
    {
        public static async Task<int?> AddAsync(string type)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Type", type),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int? questionTypeId, string type)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@QuestionTypeId", questionTypeId),
                new SqlParameter("@Type", type),
            };
            return await CRUD.UpdateAsync("Sp_Update", parameters);
        }

        public static async Task<bool> DeleteAsync(int? questionTypeId)
        {
            if (questionTypeId == null) return false;
            return await CRUD.DeleteAsync("SP_Delete", "QuestionTypeId", questionTypeId);
        }

        public static async Task<Dictionary<string, object>?> GetByIdAsync(int questionTypeId) => await CRUD.GetByColumnValueAsync("Sp_GetSettingsById", "QuestionTypeId", questionTypeId);

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("Sp_GetAll");

    }
}
