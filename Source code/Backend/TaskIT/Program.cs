global using Microsoft.EntityFrameworkCore;
global using TaskIT.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskIT.Communication.GroupManager;
using TaskIT.Communication.NotificationServices;
using TaskIT.Communication.NotificationServicesImpl;
using TaskIT.Filters;
using TaskIT.Hubs;
using TaskIT.Repository.FinishedJobRepositoryF;
using TaskIT.Repository.JobAdvertisementRepositoryF;
using TaskIT.Repository.JobApplicationRepositoryF;
using TaskIT.Repository.NotificationRepositoryF;
using TaskIT.Repository.UnityOfWork;
using TaskIT.Repository.UserFollowingRepositoryF;
using TaskIT.Repository.UserRepositoryF;
using TaskIT.Repository.WorkerJobTypeFollowingF;
using TaskIT.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddScoped<NotificationService, NotificationServiceImpl>();
builder.Services.AddScoped<SignalRGroupManager>();

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
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<TaskITContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/taskItHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };

});

builder.Services.AddScoped<TokenService>();
builder.Services.AddAuthorization();

builder.Services.AddScoped<UserRepository, UserRepositoryImpl>();
builder.Services.AddScoped<FinishedJobRepository, FinishedJobRepositoryImpl>();
builder.Services.AddScoped<JobAdvertisementRepository, JobAdvertisementRepositoryImpl>();
builder.Services.AddScoped<UserFollowingRepository, UserFollowingRepositoryImpl>();
builder.Services.AddScoped<WorkerJobTypeFollowingRepository, WorkerJobTypeFollowingRepositoryImpl>();
builder.Services.AddScoped<JobApplicationRepository, JobApplicationRepositoryImpl>();
builder.Services.AddScoped<NotificationRepository, NotificationRepositoryImpl>();
builder.Services.AddScoped<UnitOfWork, UnitOfWorkImpl>();

builder.Services.AddSingleton<JobFilterStrategyFactory>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskIT API", Version = "v1" });

    // JWT Authorization
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token."
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
            new string[] {}
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RolesGenerator.SeedRoles(roleManager);
}

app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<TaskItHub>("/taskItHub");

app.Run();