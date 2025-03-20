using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class clsSubjectQuestionTypesStages : absBaseEntity
    {
        public clsStageSubject StageSubject { get; set; }
        public clsQuestionsType QuestionsType { get; set; }

        public clsSubjectQuestionTypesStages(clsStageSubject stageSubject, clsQuestionsType questionsType)
        {
            StageSubject = stageSubject;
            QuestionsType = questionsType;
        }
        private clsSubjectQuestionTypesStages(int subjectQuestionTypeStageId, clsStageSubject stageSubject, clsQuestionsType questionsType)
        {
            Id = subjectQuestionTypeStageId;
            StageSubject = stageSubject;
            QuestionsType = questionsType;
        }

        public async Task<bool> SaveAsync()
        {
            if (!Id.HasValue)
                return await __AddAsync();
            return await __UpdateAsync();
        }

        private async Task<bool> __AddAsync()
        {
            Id = await clsSubjectQuestionTypesStagesData.AddAsync(StageSubject.Id, QuestionsType.Id);
            return Id.HasValue;
        }

        public static async Task<clsSubjectQuestionTypesStages?> GetByIdAsync(int subjectQuestionTypeStageId)
        {
            var data = await clsSubjectQuestionTypesStagesData.GetByIdAsync(subjectQuestionTypeStageId);
            if (data == null) return null;
            return FetchData(ref data);
        }

        public static async Task<DataTable?> GetAllAsDataTableAsync() => await clsSubjectQuestionTypesStagesData.GetAllAsDataTableAsync();

        private async Task<bool> __UpdateAsync() => await clsSubjectQuestionTypesStagesData.UpdateAsync(Id, StageSubject.Id, QuestionsType.Id);

        public static async Task<bool> DeleteByIdAsync(int? subjectQuestionTypeStageId) => await clsSubjectQuestionTypesStagesData.DeleteAsync(subjectQuestionTypeStageId);

        public async Task<bool> DeleteAsync() => await DeleteByIdAsync(Id);

        internal static clsSubjectQuestionTypesStages FetchData(ref Dictionary<string, object> dict) =>
            new clsSubjectQuestionTypesStages((int)dict["QuestionTypeSubjectStageId"], clsStageSubject.FetchUserData(ref dict), clsQuestionsType.FetchUserData(ref dict));






    }
}
