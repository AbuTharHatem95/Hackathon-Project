using DAL;
using System.Data;

namespace BLL
{
    public class clsTeacher: absBaseEntity
    {
        public string TeacherName { set; get; }
        public clsTeacher(string teacherName)
        {
            TeacherName = teacherName;
        }

        private clsTeacher(int teacherId, string teacherName)
        {
            Id = teacherId;
            TeacherName = teacherName;
        }

        public async Task<bool> SaveAsync()
        {
            if (!Id.HasValue)
                return await __AddAsync();
            return await __UpdateAsync();
        }

        private async Task<bool> __AddAsync()
        {
            Id = await clsTeacherData.AddAsync(TeacherName);
            return Id.HasValue;
        }

        public static async Task<clsTeacher?> GetByIdAsync(int teacherId)
        {
            var data = await clsTeacherData.GetByIdAsync(teacherId);
            if (data == null) return null;
            return new clsTeacher(teacherId, (string)data["TeacherName"]);
        }

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await clsTeacherData.GetAllAsDataTableAsync();

        private async Task<bool> __UpdateAsync() => await clsTeacherData.UpdateAsync(Id, TeacherName);

        public static async Task<bool> DeleteByIdAsync(int? teacherId) => await clsTeacherData.DeleteAsync(teacherId);

        public async Task<bool> DeleteAsync() => await DeleteByIdAsync(Id);

    }
}
