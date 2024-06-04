using System.Threading.RateLimiting;
using Authorization;
using Microsoft.OpenApi.Models;
using ProxmoxApi;
using VirtualLab;
using VirtualLab.Application;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure;
using VirtualLab.Infrastructure.DataBase;
using VirtualLab.Infrastructure.Options;
using VirtualLab.Infrastructure.Repositories;
using VirtualLab.MiddleWare;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Logging.ClearProviders();
builder.AddLogVostokWithConfig();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Set the security scheme requirements
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
// все зависимости связанные с бд
builder.Services.AddDbContext<LabDbContext>();

builder.Services.AddScoped<ILabCreationService, LabCreationService>();
builder.Services.AddScoped<ILabRepository, LabRepository>();
builder.Services.AddScoped<IUserLabRepository, UserLabsRepository>();
builder.Services.AddScoped<IUserLabProvider, UserLabProviderService>();
builder.Services.AddScoped<ILabProvider, LabProvider>();
builder.Services.AddScoped<IUserHttpService, UserHttpService>();
//конец))
// самый важный класс
builder.Services.AddScoped<ILabConfigure, LabConfigure>();
builder.Services.AddScoped<ILabEntryPointRepository, LabEntryPointRepository>();
builder.Services.AddScoped<ILabManager, LabManager>();
builder.Services.AddScoped<IVirtualMachineDataHandler, VirtualMachineDataHandler>();
builder.Services.AddScoped<IVirtualMachineRepository, VirtualMachineRepository>();
builder.Services.AddScoped<ICredentialRepository, CredentialRepository>();

builder.Services.Configure<ConfMongoDb>(builder.Configuration.GetSection("MongoDbConf"));

builder.Services.AddConfigureAuthentication();
builder.Services.AddPveClient();


var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseUserLabStatuses();
app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();