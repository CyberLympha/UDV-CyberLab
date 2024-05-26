using FluentResults;
using VirtualLab.Domain.ValueObjects;

namespace VirtualLab.Application.Interfaces
{
    public interface ILabProvider
    {
        public Task<Result<IReadOnlyCollection<TeacherLabShortInfo>>> GetTeacherLabs(Guid teacherId);
    }
}
