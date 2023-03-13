using Api.Core;
using Application;
using Application.Commands;
using Application.DataTransfer;
using Application.Queries;
using DataAccess;
using Implementation.Commands;
using Implementation.Queries;
using Implementation.Repository;
using Implementation.Validataion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<PowerPlatformTaskContext>();
builder.Services.AddTransient<ICreateCampaignCustomerCommand, CreateCampaignCustomerCommand>();
builder.Services.AddTransient<CreateCampaignCustomerValidator>();
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.AddTransient<IGetCampaignsQuery, GetCampaignsQuery>();
builder.Services.AddTransient<JwtManager>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IApplicationActor>(x =>
{
    var accessor = x.GetService<IHttpContextAccessor>();
    var user = accessor.HttpContext.User;

    if(user.FindFirst("ActorData") == null)
    {
        throw new InvalidOperationException("Actor data doesn't exist in token.");
    }

    var actorString = user.FindFirst("ActorData").Value;

    var actor = JsonConvert.DeserializeObject<JwtActor>(actorString);

    return actor;
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = "asp_api",
        ValidateIssuer = true,
        ValidAudience = "Any",
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsMyVerySecretKey")),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddControllers();
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

app.UseCors(x =>
{
    x.AllowAnyOrigin();
    x.AllowAnyMethod();
    x.AllowAnyHeader();
});

app.UseMiddleware<GlobalExceptionHandler>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
