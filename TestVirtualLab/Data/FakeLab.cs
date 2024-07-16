using VirtualLab.Domain.Entities;

namespace TestVirtualLab.Data;

public class FakeLab
{
    public static Lab CreateSample(Guid id)
    {
        return new Lab()
        {
            Description = "очень важная штука",
            Id = id,
            Manual = "просто открой и делай",
            Name = "hack",
            CreatedAt = new DateTime(20024, 1, 11),
            CreatorId = id,
            DeadLine = new DateTime(20024, 1, 11),
            IsOpened = false,
            UserLabs = []
        };
    }
}