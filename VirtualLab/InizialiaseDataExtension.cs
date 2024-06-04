using Microsoft.EntityFrameworkCore;
using VirtualLab.Domain.Entities;
//using VirtualLab.Domain.Entities.Enums;
using VirtualLab.Infrastructure.DataBase;
using Vostok.Logging.Abstractions;
using StatusUserLab = VirtualLab.Domain.Entities.StatusUserLab;

namespace VirtualLab;

public static class InizialiaseDataExtension
{
    //public static void UseUserLabStatuses(this IHost builder)
    //{
    //    var labDbContext = builder.Services.CreateScope().ServiceProvider.GetService<LabDbContext>();
    //    var log = builder.Services.CreateScope().ServiceProvider.GetService<ILog>();

    //    foreach (var status in typeof(StatusUserLabEnum).GetEnumValues())
    //    {
    //        if (!labDbContext.UserLabStatus.Have(status.ToString(), log))
    //        {
    //            labDbContext.UserLabStatus
    //                .AddAsync(new StatusUserLab()
    //                    {
    //                        Name = Enum.Parse<StatusUserLabEnum>(status.ToString()),
    //                        Id = Guid.NewGuid()
    //                    }
    //                )
    //                .AsTask()
    //                .Wait();
    //            labDbContext
    //                .SaveChangesAsync()
    //                .Wait();
    //        }
    //    }
    //}


    public static bool Have(this DbSet<StatusUserLab> dbSet, string userLabEnum, ILog? log)
    {
        var ans = dbSet.FirstOrDefaultAsync(x => x.Name.ToString() == userLabEnum).Result != null;

        return ans;
    }
}