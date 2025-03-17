using Microsoft.Data.SqlClient;
using MyLib_DotNet.DatabaseExecutor;
using System.Data;

namespace DAL
{
    public static  class clsSubjectQuestionTypesStagesData
    {
        public static async Task<int?> AddAsync(int stageSubjectId, int? questionTypeId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@StageSubjectId", stageSubjectId),
                new SqlParameter("@QuestionTypeId", questionTypeId),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int questionTypeSubjectStageId, int stageSubjectId, int? questionTypeId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@QuestionTypeSubjectStageId", questionTypeSubjectStageId),
                new SqlParameter("@StageSubjectId", stageSubjectId),
                new SqlParameter("@QuestionTypeId", questionTypeId),

            };
            return await CRUD.UpdateAsync("Sp_Update", parameters);
        }

        public static async Task<bool> DeleteAsync(int? questionTypeSubjectStageId)
        {
            if (questionTypeSubjectStageId == null) return false;
            return await CRUD.DeleteAsync("SP_Delete", "QuestionTypeSubjectStageId", questionTypeSubjectStageId);
        }

        public static async Task<Dictionary<string, object>?> GetByIdAsync(int questionTypeSubjectStageId) => await CRUD.GetByColumnValueAsync("Sp_GetSettingsById", "QuestionTypeSubjectStageId", questionTypeSubjectStageId);

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("Sp_GetAll");

    }
}
