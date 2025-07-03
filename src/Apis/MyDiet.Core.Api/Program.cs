using MyDiet.Auth.Business.Middlewares;
using MyDiet.Core.Business.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddStartupServices(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<AuthMiddleware>();
app.UseAuthorization();
app.UseMiddleware<CoreUserMiddleware>();
app.MapControllers();

app.Run();
