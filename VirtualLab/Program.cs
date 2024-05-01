using ProxmoxApi;
using VirtualLab.Application;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure;
using VirtualLab.Infrastructure.DataBase;
using VirtualLab.Infrastructure.Repositories;
using VirtualLab.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.AddLogVostokWithConfig();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// все зависимости связанные с бд
builder.Services.AddDbContext<FakeDbContext>();

builder.Services.AddScoped<ILabCreationService, LabCreationService>();
builder.Services.AddScoped<ILabRepository, LabRepository>();
builder.Services.AddScoped<IUserLabRepository, UserLabsRepository>();
builder.Services.AddScoped<IUserLabProvider, UserLabProviderService>();
//конец))



builder.Services.AddPveClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();