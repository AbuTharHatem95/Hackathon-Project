using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class clsQuestionsType: absBaseEntity
    {
        public string Type { set; get; }
        public clsQuestionsType(string type)
        {
            Type = type;
        }

        public clsQuestionsType(int questionTypeId, string type)
        {
            Id = questionTypeId;
            Type = type;
        }
        public async Task<bool> SaveAsync()
        {
            if (!Id.HasValue)
                return await __AddAsync();
            return await __UpdateAsync();
        }

        private async Task<bool> __AddAsync()
        {
            Id = await clsSchoolData.AddAsync(Type);
            return Id.HasValue;
        }

        public static async Task<clsQuestionsType?> GetByIdAsync(int schoolId)
        {
            var data = await clsSchoolData.GetByIdAsync(schoolId);
            if (data == null) return null;
            return FetchUserData(ref data); 
        }

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await clsSchoolData.GetAllAsDataTableAsync();

        private async Task<bool> __UpdateAsync() => await clsQuestionsTypeData.UpdateAsync(Id, Type);

        public static async Task<bool> DeleteByIdAsync(int? questionTypeId) => await clsQuestionsTypeData.DeleteAsync(questionTypeId);

        public async Task<bool> DeleteAsync() => await DeleteByIdAsync(Id);

        internal static clsQuestionsType FetchUserData(ref Dictionary<string, object> dict) => new clsQuestionsType((int)dict["QuestionTypeId"], (string)dict["Type"] );


    }
}
