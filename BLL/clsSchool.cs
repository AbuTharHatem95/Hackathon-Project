using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class clsSchool : absBaseEntity
    {
        public string SchoolName { set; get; }
        public clsSchool(string schoolName)
        {
            SchoolName = schoolName;
        }

        private clsSchool(int schoolId, string schoolName)
        {
            Id = schoolId;
            SchoolName += schoolName;
        }

        public async Task<bool> SaveAsync()
        {
            if (!Id.HasValue)
                return await __AddAsync();
            return await __UpdateAsync();
        }

        private async Task<bool> __AddAsync()
        {
            Id = await clsSchoolData.AddAsync(SchoolName);
            return Id.HasValue;
        }

        public static async Task<clsSchool?> GetByIdAsync(int schoolId)
        {
            var data = await clsSchoolData.GetByIdAsync(schoolId);
            if (data == null) return null;
            return new clsSchool(schoolId, (string)data["SchoolName"]);
        }

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await clsSchoolData.GetAllAsDataTableAsync();

        private async Task<bool> __UpdateAsync() => await clsSchoolData.UpdateAsync(Id, SchoolName);

        public static async Task<bool> DeleteByIdAsync(int? schoolId) => await clsSchoolData.DeleteAsync(schoolId);

        public async Task<bool> DeleteAsync() => await DeleteByIdAsync(Id);



    }
}
