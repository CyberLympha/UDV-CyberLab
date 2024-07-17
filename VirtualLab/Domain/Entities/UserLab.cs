namespace VirtualLab.Domain.Entities;

//todo дописать сущность
public class UserLab : IEntity<Guid>
{
    public Guid UserId { get; set; }
    public Guid LabId { get; set; }
    public Guid StatusId { get; set; } // нужна таблица с statusId
    public int Rate { get; set; } // 

    public Guid Id { get; set; }
    // и другие данные, которые отображаются для пользователя.

    public static UserLab From(Guid userId, Lab lab, StatusUserLab status)
    {
        return new UserLab
        {
            Id = Guid.NewGuid(),
            Rate = 0,
            LabId = lab.Id,
            StatusId = status.Id,
            UserId = userId
        };
    }

    public override string ToString()
    {
        return $"UserLab {{UserId={UserId}, LabId={LabId}, StatusId={StatusId}, Rate={Rate}, id={Id}}}";
    }
}