global using Microsoft.EntityFrameworkCore;
global using TaskIT.Model;
using TaskIT.Repository;
using TaskIT.Hubs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;



//global using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TaskITContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskItCS"));
});

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<TaskITContext>();
builder.Services.AddTransient(typeof(Repository<>), typeof(RepositoryImpl<>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

app.MapIdentityApi<User>();

app.MapPost("/logout", async (SignInManager<User> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok("User logged out successfully.");
});

app.MapGet("/pingauth", (ClaimsPrincipal user) =>
{
    var email = user.FindFirstValue(ClaimTypes.Email);
    return Results.Json(new { Email = email }); 

}).RequireAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<NoviOglasHub>("/noviOglasHub");

app.MapControllers();

app.Run();