using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class clsSettings : absBaseEntity
    {
        public string ApiKey { set; get; }
        public string SecretKey { set; get; }
        public string ModelName { set; get; }
        public clsSettings(string apiKey, string secretKey, string modelName)
        {
            ApiKey = apiKey;
            SecretKey = secretKey;
            ModelName = modelName;
        }

        private clsSettings(int AiInfoId, string apiKey, string secretKey, string modelName)
        {
            Id = AiInfoId;
            ApiKey = apiKey;
            SecretKey = secretKey;
            ModelName = modelName;
        }

        public async Task<bool> SaveAsync()
        {
            if (!Id.HasValue)
                return await __AddAsync();
            return await __UpdateAsync();
        }

        private async Task<bool> __AddAsync()
        {
            Id = await clsSettingAiData.AddAsync(ApiKey, SecretKey, ModelName);
            return Id.HasValue;
        }

        public static async Task<clsSettings?> GetByIdAsync(int aiInfoId)
        {
            var data = await clsSettingAiData.GetByIdAsync(aiInfoId);
            if (data == null) return null;
            return new clsSettings(aiInfoId, (string)data["ApiKey"], (string)data["SecretKey"], (string)data["ModelName"]);
        }

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await clsSettingAiData.GetAllAsDataTableAsync();

        private async Task<bool> __UpdateAsync() => await clsSettingAiData.UpdateAsync(Id, ApiKey, SecretKey, ModelName);

        public static async Task<bool> DeleteByIdAsync(int? AiInfoId) => await clsSettingAiData.DeleteAsync(AiInfoId);

        public async Task<bool> DeleteAsync() => await DeleteByIdAsync(Id);




    }
}
