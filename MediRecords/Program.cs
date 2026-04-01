using Microsoft.EntityFrameworkCore; 
using MediRecords.Domain.Entities;   
using MediRecords.Services.AuthServices;
using MediRecords.Services.UserServices;
using MediRecords.Repository;
using MediRecords.Repositories;
using MediRecords.Repository.UserRepo;
using Microsoft.OpenApi;
using MediRecords.Repository.UserRoleRepository;

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
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository,AuthRepository>();
builder.Services.AddScoped<IUserRoleRepository,UserRoleRepository>();
 
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