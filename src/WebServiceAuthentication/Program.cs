using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebServiceAuthentication.Context;
using WebServiceAuthentication.Entities;
using WebServiceAuthentication.Models.Auth;
using WebServiceAuthentication.Services;

var builder = WebApplication.CreateBuilder(args);

// For Linux reading appsettings
builder.Host.ConfigureAppConfiguration((_, config) =>
{
    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json", true);
});

// Add services to the container.
builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.Issuer,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
        };
    });

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

builder.Services.AddTransient<WebConfiguratorContext>();

#region Init Default User in Db

var dbContext = new WebConfiguratorContext(builder.Configuration);

dbContext.Database.EnsureCreated();

if (!dbContext.Users.Any())
{
    dbContext.Users.Add(new User
        { Username = "Admin", Password = "Admin", Role = "Administrator" });

    dbContext.SaveChanges();
}

dbContext.Dispose();

#endregion

var app = builder.Build();

// Url: https://localhost:7290/swagger/index.html
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();