using DVLD_DataAccess;
using Microsoft.Data.SqlClient;
using MyLib_DotNet.DatabaseExecutor;
using System.Data;

namespace DAL
{
    public static  class clsSettingAiData
    {


        public static async Task<int?> AddAsync(string apiKey, string secretKey, string modelName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ApiKey", apiKey),
                new SqlParameter("@SecretKey", secretKey),
                new SqlParameter("@ModelName", modelName),
            };
            return await CRUD.AddAsync("Sp_Add", parameters);
        }
        public static async Task<bool> UpdateAsync(int? settingsId, string apiKey, string secretKey, string modelName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ApiKey", apiKey),
                new SqlParameter("@SettingsId", settingsId),
                new SqlParameter("@SecretKey", secretKey),
                new SqlParameter("@ModelName", modelName)
            };
            return await CRUD.UpdateAsync("Sp_Update", parameters);
        }

        public static async Task<bool> DeleteAsync(int? settingsId)
        {
            if (settingsId == null) return false;
            return await CRUD.DeleteAsync("SP_Delete", "SettingsId", settingsId);
        }

        public static async Task<Dictionary<string, object>?> GetByIdAsync(int settingsId) => await CRUD.GetByColumnValueAsync("Sp_GetSettingsById", "SettingsId", settingsId);

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await CRUD.GetAllAsDataTableAsync("Sp_GetAll");

        public static DataTable GetAll()
        {
            string Query = "Select * from AiSettings";
            SqlCommand cmd = new SqlCommand(Query);
            return CRUDs.GetAll(cmd);
        }


    }
}
