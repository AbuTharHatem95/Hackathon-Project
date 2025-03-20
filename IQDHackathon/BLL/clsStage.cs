using DAL;
using System.Data;

namespace BLL
{
    public class clsStage : absBaseEntity
    {
        public string StageName { set; get; }
        public clsStage(string stageName)
        {
            StageName = stageName;
        }

        private clsStage(int stageId, string stageName)
        {
            Id = stageId;
            StageName += stageName;
        }

        public async Task<bool> SaveAsync()
        {
            if (!Id.HasValue)
                return await __AddAsync();
            return await __UpdateAsync();
        }

        private async Task<bool> __AddAsync()
        {
            Id = await clsStageData.AddAsync(StageName);
            return Id.HasValue;
        }

        public static async Task<clsStage?> GetByIdAsync(int stageId)
        {
            var data = await clsStageData.GetByIdAsync(stageId);
            if (data == null) return null;
            return FetchUserData(ref data);
        }

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await clsStageData.GetAllAsDataTableAsync();

        private async Task<bool> __UpdateAsync() => await clsSchoolData.UpdateAsync(Id, StageName);

        public static async Task<bool> DeleteByIdAsync(int? schoolId) => await clsSchoolData.DeleteAsync(schoolId);

        public async Task<bool> DeleteAsync() => await DeleteByIdAsync(Id);



        public static DataTable GetAll() => clsStageData.GetAll();

        internal static clsStage FetchUserData(ref Dictionary<string, object> dict) => new clsStage((int)dict["StageId"], (string)dict["StageName"] );



    }
}
