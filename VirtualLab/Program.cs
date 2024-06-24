using Authorization;
using ProxmoxApi;
using VirtualLab;
using VirtualLab.Application;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.MongoRepository;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;
using VirtualLab.Infrastructure.Options;
using VirtualLab.Infrastructure.Repositories;
using VirtualLab.Infrastructure.UnitOfWork;
using VirtualLab.Lib;
using VirtualLab.MiddleWare;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.AddLogVostokWithConfig();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// все зависимости связанные с бд
builder.Services.AddDbContext<LabDbContext>();

builder.Services.AddScoped<ILabCreationService, LabCreationService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<ILabRepository, LabRepository>();
builder.Services.AddScoped<IUserLabRepository, UserLabsRepository>();
builder.Services.AddScoped<IUserLabProvider, UserLabProviderService>();


//конец))
// самый важный класс
builder.Services.AddScoped<ILabConfigure, LabConfigure>();
builder.Services.AddScoped<ILabEntryPointRepository, LabEntryPointRepository>();
builder.Services.AddScoped<ILabManager, LabManager>();
builder.Services.AddScoped<IVirtualMachineDataHandler, VirtualMachineDataHandler>();
builder.Services.AddScoped<IVirtualMachineRepository, VirtualMachineRepository>();
builder.Services.AddScoped<ICredentialRepository, CredentialRepository>();
builder.Services.AddScoped<ITemplateVmRepository, TemplateVmRepository>();



builder.Services.AddScoped(_ => MongoDbConf.FromEnv());
builder.Services.AddScoped<IMongoContext, MongoDb>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkMongo>();
builder.Services.AddScoped<IConfigStandRepository, ConfigStandRepository>();


builder.Services.AddConfigureAuthentication();
builder.Services.AddPveClient();


var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseUserLabStatuses();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();