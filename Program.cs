using Serilog;
using UDVTestCase.BD;
using UDVTestCase.Services;

var builder = WebApplication.CreateBuilder(args);

// Настройка логгера (сохраняет в файл и выводит в консоль)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(builder.Configuration["Logging:LogFilePath"], rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // <-- Подключаем Serilog

// Остальные сервисы
builder.Services.AddDbContext<BDContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<VKService>(provider => 
    new VKService(
        builder.Configuration["VK:AccessToken"],
        long.Parse(builder.Configuration["VK:UserId"])
    ));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

try
{
    Log.Information("Starting web host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}