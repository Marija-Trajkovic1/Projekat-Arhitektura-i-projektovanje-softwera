global using Microsoft.EntityFrameworkCore;
global using TaskIT.Model;
using TaskIT.Repository;



//global using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TaskITContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskItCS"));
});

builder.Services.AddTransient(typeof(Repository<>), typeof(RepositoryImpl<>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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