using Microsoft.EntityFrameworkCore; 
using MediRecords.Domain.Entities;   
using MediRecords.Services.AuthServices;
using MediRecords.Services.UserServices;
using MediRecords.Repository;
using MediRecords.Repositories;
using MediRecords.Repository.UserRepo;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<MediRecordsDbContext>(options => 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<IAuthRepository,AuthRepository>(); 
builder.Services.AddScoped<IUserRepository,UserRepository>(); 
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
        
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "MediRecords API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapControllers(); 

app.Run();