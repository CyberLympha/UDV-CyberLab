using Authorization;
using VirtualLab.Domain.Entities;
namespace VirtualLab.Domain.ValueObjects
{
    public class AttemptShortInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime LastSentAt { get; set; }
        public int LastRate { get; set; }

        public static AttemptShortInfo From(UserInfo user, UserLab userLab)
        {
            return new AttemptShortInfo
            {
                Id = userLab.Id,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastSentAt = DateTime.Parse("2024-05-22T20:30"),//TODO: userLab.Report.LastSentAt,
                LastRate = userLab.Rate
            };
        }
    }
}
