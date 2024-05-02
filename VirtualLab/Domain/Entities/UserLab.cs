using ProxmoxApi.Domen;
using ProxmoxApi.Domen.Entities;

namespace VirtualLab.Domain.Entities;

//todo
public class UserLab : IEntity<Guid>
{
    public System.Guid Id { get; set; }
    public System.Guid UserId { get; set; }
    public System.Guid LabId { get; set; }
    public System.Guid StatusId { get; set; } // нужна таблица с statusId
    public int Rate { get; set; } // 
    // и другие данные, которые отображаются для пользователя.

    public static UserLab From(User user, Lab guid)
    {
        return new UserLab()
        {
            Id = Guid.NewGuid(),
            Rate = 0,
            LabId = guid.Id,
            StatusId = new Guid("52059edd-dcb0-49f8-9f37-86fd36649228"), // пока это часть ваще не реализованна.
            UserId = user.Id
        };
    }
}