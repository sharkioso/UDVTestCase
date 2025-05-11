using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BDContext>
(options=>options.UseNpgsql(builder.Configuration
                 .GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<VKService>();

builder.Logging.AddFile(builder.Configuration["Logging:LogPath"]);

app.Run();
