using DATN.Aplication.Extentions;
using DATN.Aplication.System;
using DATN.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DATN.Data.EF;
using DATN.Aplication.Services;
using DATN.Aplication;
using DATN.Aplication.Services.IServices;
using System.Net;
using DATN.Aplication.Mapping;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DATNDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DATN"),
        b => b.MigrationsAssembly("DATN.API"));
});

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<DATNDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Key").Value))

    };
});

builder.Services.AddScoped<MailExtention>();
builder.Services.AddScoped<RandomCodeExtention>();
builder.Services.AddScoped<IAuthenticate, Authenticate>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IGuestManagerService, GuestManagerService>();
builder.Services.AddScoped<IWorkShiftManagementService, WorkShiftManagementService>();
builder.Services.AddScoped<IEmployeeScheduleManagementService, EmployeeScheduleManagementService>();
builder.Services.AddScoped<IShiftManagementService, ShiftManagementService>();
builder.Services.AddScoped<IVoucherManagementService, VoucherManagementService>();
builder.Services.AddScoped<IPetSpeciesManagerService, PetSpeciesManagerService>();
builder.Services.AddScoped<IPetManagerService, PetManagerService>();
builder.Services.AddScoped<IServiceManagementService, ServiceManagementService>();
builder.Services.AddScoped<IServiceDetailManagementService, ServiceDetailManagementService>();
builder.Services.AddScoped<ICategoryManagementService, CategoryManagementService>();
builder.Services.AddScoped<ICategoryProductManagementService, CategoryProductManagementService>();
builder.Services.AddScoped<IBrandManagementService, BrandManagementService>();
builder.Services.AddScoped<IProductManagementService, ProductManagementService>();
builder.Services.AddScoped<IProductDetaiManagementService, ProductDetaiManagementService>();


// Add auto mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
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
