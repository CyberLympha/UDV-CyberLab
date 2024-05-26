using VirtualLab.Domain.ValueObjects;

namespace VirtualLab.Application.Interfaces
{
    public interface IUserHttpService
    {
        public Task<UserInfo> GetUserInfo(string userId);
    }
}
