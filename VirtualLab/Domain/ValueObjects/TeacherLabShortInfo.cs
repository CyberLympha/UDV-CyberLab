using VirtualLab.Domain.Entities;

namespace VirtualLab.Domain.ValueObjects
{
    public class TeacherLabShortInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static TeacherLabShortInfo From(Lab lab)
        {
            return new TeacherLabShortInfo
            {
                Id = lab.Id,
                Name = lab.Name
            };
        }
    }
}
