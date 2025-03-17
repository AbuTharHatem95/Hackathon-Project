using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class clsSubject : absBaseEntity
    {


        public string SubjectName { set; get; }
        public clsSubject(string subjectName)
        {
            SubjectName = subjectName;
        }

        private clsSubject(int subjectId, string subjectName)
        {
            Id = subjectId;
            SubjectName += subjectName;
        }

        public async Task<bool> SaveAsync()
        {
            if (!Id.HasValue)
                return await __AddAsync();
            return await __UpdateAsync();
        }

        private async Task<bool> __AddAsync()
        {
            Id = await clsTeacherData.AddAsync(SubjectName);
            return Id.HasValue;
        }

        public static async Task<clsSubject?> GetByIdAsync(int subjectId)
        {
            var data = await clsTeacherData.GetByIdAsync(subjectId);
            if (data == null) return null;
            return FetchUserData(ref data);
        }

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await clsSubjectData.GetAllAsDataTableAsync();

        private async Task<bool> __UpdateAsync() => await clsSubjectData.UpdateAsync(Id, SubjectName);

        public static async Task<bool> DeleteByIdAsync(int? subjectId) => await clsSubjectData.DeleteAsync(subjectId);

        public async Task<bool> DeleteAsync() => await DeleteByIdAsync(Id);

        internal static clsSubject FetchUserData(ref Dictionary<string, object> dict) => new clsSubject((int)dict["SubjectId"], (string)dict["SubjectName"]);
        



    }
}
