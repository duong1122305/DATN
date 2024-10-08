﻿using DATN.ADMIN.IServices;
using DATN.ADMIN.Services;
using DATN.Aplication.CustomProvider;
using DATN.Aplication.Services;
using DATN.Data.EF;
using DATN.Data.Entities;
using DATN.ViewModels.DTOs.Booking;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;
using MudBlazor.Services;
using Syncfusion.Blazor;
using System.Globalization;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXlccnRRRmNYV0Z+X0U=");
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("vi-VN");
builder.Services.Configure<CloundinarySettings>(builder.Configuration.GetSection("CloundinarySettings"));

builder.Services.AddSyncfusionBlazor();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSignalRCore();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped(_http => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetSection("BaseUrlAPI").Value), Timeout = TimeSpan.FromMinutes(30) });
builder.Services.AddScoped<IUserClientSev, UserClienSev>();
builder.Services.AddScoped<IVoucherServices, VoucherServices>();
builder.Services.AddScoped<HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMudServices();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
                    ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Key").Value!))
                };
            });

builder.Services.AddDbContext<DATNDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DATN"),
        b => b.MigrationsAssembly("DATN.API"));
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestLineSize = 32768; // Tăng kích thước tối đa cho dòng yêu cầu
    options.Limits.MaxRequestHeadersTotalSize = 32768*2; // Tăng kích thước tổng cho các header
});
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<DATNDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthorization();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IServiceManagermentService, ServiceManagermentService>();
builder.Services.AddScoped<IServiceDetailServices, ServiceDetailServices>();
builder.Services.AddScoped<IGuestManagerClient, GuestManagerClient>();
builder.Services.AddScoped<IEmployeeScheduleSer, EmployeeScheduleSer>();
builder.Services.AddScoped<IPetSpeciesServiceClient, PetSpeciesServiceClient>();
builder.Services.AddScoped<IPetServiceClient, PetServiceClient>();
builder.Services.AddScoped<IPetManagerServices, PetManagerServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IBrandServices, BrandServices>();
builder.Services.AddScoped<IUpLoadFileService, UploadFileService>();
builder.Services.AddResponseCaching(); // Adds response caching, which also enables buffering
builder.Services.AddScoped<IAttendanceServiceClient, AttendanceServiceClient>();
builder.Services.AddScoped<IUpLoadFileService, UploadFileService>();
builder.Services.AddScoped<IBookingViewServices, BookingViewServices>();
builder.Services.AddScoped<IStatiscalClient, StatiscalClient>();
builder.Services.AddScoped<IReportClient, ReportClient>();
builder.Services.AddSingleton<BookingService>();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 2000;
    config.SnackbarConfiguration.HideTransitionDuration = 100;
    config.SnackbarConfiguration.ShowTransitionDuration = 100;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Outlined;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
           builder =>
           {
               builder.WithOrigins("https://localhost:44305/", "https://localhost:7259/", "http://localhost:5173/", "https://mewshop.datlich.id.vn/", "https://datn-sd33.datlich.id.vn/") // Đổi thành domain của client
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .SetIsOriginAllowedToAllowWildcardSubdomains();
           });
});
builder.Services.AddServerSideBlazor()
        .AddCircuitOptions(options =>
        {
            options.DetailedErrors = true;
        });
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSession();

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("AllowSpecificOrigin"); // Áp dụng CORS
// ... (in the Configure method)
app.UseResponseCaching();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();