using Microsoft.EntityFrameworkCore; 
using MediRecords.Domain.Entities;   
using MediRecords.Services.AuthServices;
using MediRecords.Services.UserServices;
using MediRecords.Repository;
using MediRecords.Repositories;
using MediRecords.Repository.UserRepo;
using MediRecords.Repository.MedicationRepository;
using MediRecords.Services.MedicationServices;
using MediRecords.Repository.PrescriptionItemRepository;
using MediRecords.Services.PrescriptionItemServices;
using Microsoft.OpenApi;
using MediRecords.Repository.UserRoleRepository;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using MediRecords.Repository.PatientRepo;
using MediRecords.Services.PatientServices;
using MediRecords.Repository.EncounterRepo;
using MediRecords.Services.EncounterServices;
using MediRecords.MappingProfiles;
using MediRecords.Services.AppointmentsServices;
using MediRecords.Repository.VitalSignRepository;
using MediRecords.Services.VitalSignServices;
using MediRecords.Repository.NursingNoteRepository;
using MediRecords.Services.NursingNoteServices;
using MediRecords.Services.ProblemListServices;
using MediRecords.Repository.ProblemListRepository;
using MediRecords.Services.AllergyServices;
using MediRecords.Repository.AllergyRepository;
using MediRecords.Services.MedicalHistoryServices;
using MediRecords.Repository.MedicalHistoryRepository;
using MediRecords.Services.PrescriptionService;
using MediRecords.Repository.PrescriptionRepository;

var builder = WebApplication.CreateBuilder(args);


// Add DbContext
builder.Services.AddDbContext<MediRecordsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
 
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name ="Authorization",
        Type=SecuritySchemeType.Http,
        Scheme="Bearer",
        BearerFormat="JWT",
        In=ParameterLocation.Header,
        Description="JWT Authentication using Bearer scheme"
    });
    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {new OpenApiSecuritySchemeReference("Bearer",doc),new List<string>()}
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository,AuthRepository>();
builder.Services.AddScoped<IUserRoleRepository,UserRoleRepository>();
builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();
builder.Services.AddScoped<IMedicationService, MedicationService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IEncounterRepository, EncounterRepository>();
builder.Services.AddScoped<IEncounterService, EncounterService>();
builder.Services.AddScoped<IVitalSignRepository, VitalSignRepository>();
builder.Services.AddScoped<IVitalSignService, VitalSignService>();
builder.Services.AddScoped<INursingNoteRepository, NursingNoteRepository>();
builder.Services.AddScoped<INursingNoteService, NursingNoteService>();
builder.Services.AddAutoMapper(typeof(EncounterMappingProfile));
builder.Services.AddScoped<IAppointmentsService, AppointmentsService>();
builder.Services.AddScoped<IAppointmentsRepository, AppointmentsRepository>();
builder.Services.AddScoped<IProblemListService, ProblemListService>();
builder.Services.AddScoped<IProblemListRepository, ProblemListRepository>();
builder.Services.AddScoped<IAllergyService, AllergyService>();
builder.Services.AddScoped<IAllergyRepository, AllergyRepository>();
builder.Services.AddScoped<IMedicalHistoryService, MedicalHistoryService>();
builder.Services.AddScoped<IMedicalHistoryRepository, MedicalHistoryRepository>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IPrescriptionItemRepository, PrescriptionItemRepository>();
builder.Services.AddScoped<IPrescriptionItemService, PrescriptionItemService>();
var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();
}

app.UseHttpsRedirection();
 
// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();
 
app.Run();