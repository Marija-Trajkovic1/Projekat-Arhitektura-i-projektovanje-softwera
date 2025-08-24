global using Microsoft.EntityFrameworkCore;
global using TaskIT.Model;
using TaskIT.Repository;
using TaskIT.Hubs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskIT.Repository.UserRepositoryF;
using TaskIT.Repository.FinishedJobRepositoryF;
using TaskIT.Repository.JobAdvertisementRepositoryF;



//global using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();

builder.Services.AddControllers();
builder.Services.AddDbContext<TaskITContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskItCS"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<TaskITContext>();
builder.Services.AddTransient(typeof(Repository<>), typeof(RepositoryImpl<>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UserRepository, UserRepositoryImpl>();
builder.Services.AddScoped<FinishedJobRepository, FinishedJobRepositoryImpl>();
builder.Services.AddScoped<JobAdvertisementRepository, JobAdvertisementRepositoryImpl>();

var app = builder.Build();

app.UseCors("AllowReactApp");

app.MapIdentityApi<User>();

//prebaciti u zaseban fajl

app.MapPost("/logout", async (SignInManager<User> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok("User logged out successfully.");
}).RequireAuthorization();

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

app.MapControllers();

app.MapHub<TaskItHub>("/taskItHub");


app.Run();