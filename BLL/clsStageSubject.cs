using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class clsStageSubject: absBaseEntity
    {
        public clsStage Stage { set; get; }
        public clsSubject Subject { set; get; }

        public byte StClass { set; get; }

        public clsStageSubject(clsStage stage, clsSubject subject, byte stClass)
        {
            Stage = stage;
            Subject = subject;
            StClass = stClass;
        }

        private clsStageSubject(int stageSubjectId, clsStage stage, clsSubject subject, byte stClass)
        {
            Id = stageSubjectId;
            Stage = stage;
            Subject = subject;
            StClass = stClass;
        }

        public async Task<bool> SaveAsync()
        {
            if (!Id.HasValue)
                return await __AddAsync();
            return await __UpdateAsync();
        }

        private async Task<bool> __AddAsync()
        {
            Id = await clsStageSubjectsData.AddAsync(StClass, Stage.Id, Subject.Id);
            return Id.HasValue;
        }

        public static async Task<clsStageSubject?> GetByIdAsync(int stageSubjectId)
        {
            var data = await  clsStageSubjectsData.GetByIdAsync(stageSubjectId);
            if (data == null) return null;
            return FetchUserData(ref data);
        }

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await clsExamTypeData.GetAllAsDataTableAsync();

        private async Task<bool> __UpdateAsync() => await clsStageSubjectsData.UpdateAsync(Id, StClass, Stage.Id, Subject.Id);

        public static async Task<bool> DeleteByIdAsync(int? stageSubjectId) => await clsExamTypeData.DeleteAsync(stageSubjectId);

        public async Task<bool> DeleteAsync() => await DeleteByIdAsync(Id);


        internal static clsStageSubject FetchUserData(ref Dictionary<string, object> dict) => 
            new clsStageSubject((int)dict["StageSubjectId"], clsStage.FetchUserData(ref dict), clsSubject.FetchUserData( ref dict), (byte)dict["Class"]);








    }
}
