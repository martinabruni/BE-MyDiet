using MyDiet.Core.Business.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddStartupServices(builder.Configuration);
<<<<<<<< HEAD:src/Apis/MyDiet.Shared.Api/Program.cs
========

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
>>>>>>>> origin/dev:src/Apis/MyDiet.Identity.Api/Program.cs

var app = builder.Build();

await app.Services.InitializeAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<CoreUserMiddleware>();
app.MapControllers();

app.Run();
