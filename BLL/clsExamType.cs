using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class clsExamType :absBaseEntity
    {

        public string TypeName { set; get; }
        public clsExamType(string typeName)
        {
            this.TypeName = typeName;
        }

        private clsExamType(int typeId, string typeName)
        {
            Id = typeId;
            TypeName = typeName;
        }

        public async Task<bool> SaveAsync()
        {
            if (!Id.HasValue)
                return await __AddAsync();
            return await __UpdateAsync();
        }

        private async Task<bool> __AddAsync()
        {
            Id = await clsTeacherData.AddAsync(TypeName);
            return Id.HasValue;
        }

        public static async Task<clsExamType?> GetByIdAsync(int typeId)
        {
            var data = await clsTeacherData.GetByIdAsync(typeId);
            if (data == null) return null;
            return FetchData(ref data);
        }

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await clsExamTypeData.GetAllAsDataTableAsync();

        private async Task<bool> __UpdateAsync() => await clsExamTypeData.UpdateAsync(Id, TypeName);

        public static async Task<bool> DeleteByIdAsync(int? typeId) => await clsExamTypeData .DeleteAsync(typeId);

        public async Task<bool> DeleteAsync() => await DeleteByIdAsync(Id);
        internal static clsExamType FetchData(ref Dictionary<string, object> dict) => new clsExamType((int)dict["ExamTypeId"], (string)dict["TypeName"]);



    }
}
