using DATN.API.Services;
using DATN.Aplication;
using DATN.Aplication.Extentions;
using DATN.Aplication.Mapping;
using DATN.Aplication.Services;
using DATN.Aplication.Services.IServices;
using DATN.Aplication.System;
using DATN.Data.EF;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Payment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSignalRCore();
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
           builder =>
           {
               builder.WithOrigins("https://localhost:44305/", "https://localhost:7259/", "http://localhost:5173/", "https://mewshop.datlich.id.vn", "https://datn-sd33.datlich.id.vn/") // Đổi thành domain của client
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .SetIsOriginAllowedToAllowWildcardSubdomains(); // Cho phép gửi thông tin xác thực
           });
});
builder.Services.AddServerSideBlazor()
        .AddCircuitOptions(options =>
        {
            options.DetailedErrors = true;
        });
builder.Services.AddScoped<MailExtention>();
builder.Services.AddScoped<RandomCodeExtention>();
builder.Services.AddScoped<IAuthenticate, Authenticate>();
builder.Services.AddScoped<IAuthenticateGuest, AuthenticateGuest>();
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
builder.Services.Configure<CloundinarySettings>(builder.Configuration.GetSection("CloundinarySettings"));
builder.Services.AddScoped<IAttendanteMangarService, AttendanteMangarService>();
builder.Services.AddScoped<IStatisticalService, StatisticalService>();
builder.Services.AddScoped<IUploadFileServices, UploadFileServices>();

// Add auto mapper
builder.Services.AddScoped<IBookingManagement, BookingManagement>();
builder.Services.AddScoped<IProductManagement, ProductManagement>();
builder.Services.AddScoped<NotificationHub>();
builder.Services.AddScoped<Utils>();
builder.Services.AddAutoMapper(typeof(MappingProfile));



builder.Services.AddMvcCore().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = (errorContext) =>
    {
        var errors = errorContext.ModelState.Values.SelectMany(e => e.Errors.Select(m => new
        {
            ErrorMessage = m.ErrorMessage
        })).ToList();
        var result = new ResponseData<string>()
        {
            IsSuccess = false,
            Error = errors.Select(e => e.ErrorMessage).First()
        };
        return new BadRequestObjectResult(result);
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<BookingHub>("/bookingHub");
app.Run();
