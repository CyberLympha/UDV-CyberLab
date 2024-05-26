using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.ValueObjects;

namespace VirtualLab.Application
{
    public class LabProvider : ILabProvider
    {
        private readonly ILabRepository _labs;

        public LabProvider(ILabRepository labs)
        {
            _labs = labs;
        }

        public async Task<Result<IReadOnlyCollection<TeacherLabShortInfo>>> GetTeacherLabs(Guid teacherId)
        {
            var labsResult = await _labs.GetAllByCreatorId(teacherId);
            if (labsResult.IsFailed)
                return Result.Fail(labsResult.Errors);
            var answer = new List<TeacherLabShortInfo>();
            foreach (var lab in labsResult.Value)
            {
                answer.Add(TeacherLabShortInfo.From(lab));
            }
            return answer;
        }
    }
}
