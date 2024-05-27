using VirtualLab.Domain.Entities;

namespace VirtualLab.Domain.ValueObjects
{
    public class AttemptFullInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime LastSentAt { get; set; }
        public int LastRate { get; set; }
        public string Report { get; set; }

        public static AttemptFullInfo From(UserInfo user, UserLab userLab)
        {
            return new AttemptFullInfo
            {
                Id = userLab.Id,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastSentAt = DateTime.Parse("2024-05-22T20:30"),//TODO: userLab.Report.LastSentAt,
                LastRate = userLab.Rate,
                Report = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec ac posuere nibh. In hac habitasse platea dictumst. Sed ex metus, fringilla et ornare a, finibus eget eros. Quisque pharetra rutrum elit, vel malesuada ante imperdiet vel. Phasellus ornare laoreet libero. Vivamus ac lacinia sem. Sed egestas tempor neque, sit amet fringilla quam blandit eu. Aliquam eu porta neque, sed rutrum massa. Phasellus dictum dui erat, ut rhoncus turpis molestie at. Suspendisse ultricies nisl sed ex feugiat ultricies. Phasellus commodo nisl quis sem lobortis lacinia. Cras laoreet massa vel justo tempor fringilla a eget nulla."
            };
        }
    }
}
