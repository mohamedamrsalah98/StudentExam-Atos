
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentSystemRepository.Interfaces;
using StudentSystemRepository.Repository;
using StudentSystemService.Helper;
using StudentSystemService.Interfaces;
using StudentSystemService.Service;
using StudentSytemData.Data;
using StudentSytemData.Dto;
using StudentSytemData.Models;
using System.Text;
using StudentSystemService.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IAuthRepository, AuthRepository>().AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISubjectRepository,SubjectRepository>().AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>().AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAddChoiceRepository, AddChoiceRepository>().AddScoped<IAddChoiceService, AddChoiceService>();
builder.Services.AddScoped<IAddQuestionRepository, AddQuestionRepository>().AddScoped<IAddQuestionService, AddQuestionService>();
builder.Services.AddScoped<IAddExamRepository, AddExamRepository>().AddScoped<IAddExamService, AddExamService>();
builder.Services.AddScoped<IGetExamRepository, GetExamRepository>().AddScoped<IGetExamService, GetExamService>();
builder.Services.AddScoped<IAddSubjectToStudentService, AddSubjectToStudentService>().AddScoped<IAddSubjectToStudentRepository, AddSubjectToStudentRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();


//DatabaseConfig
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Identity-Config
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DataContext>();

//JwtConfig
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddAuthentication(configureOptions: options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(o =>
    {

        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });

//coneect back with front

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

builder.Services.AddCors(options =>
{
    var frontendURL = configuration.GetValue<string>("frontend_url");

    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name V1");
    //    c.RoutePrefix = string.Empty; // Serve the Swagger UI at the root (http://localhost:5000/)
    //});
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
