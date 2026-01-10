using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Middleware;
using UserApi.Services;
using UserApi.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog
SerilogUtil.Configure(builder);

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<GlobalExceptionMiddleware>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// user middleware for global exception handling before MapControllers
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();

// Needed for WebApplicationFactory in integration tests
public partial class Program { }