using DATN.ADMIN.IServices;
using DATN.ADMIN.Services;
using DATN.Aplication.CustomProvider;
using DATN.Data.EF;
using DATN.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MudBlazor.Services;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped(_http => new HttpClient { BaseAddress = new Uri("https://localhost:7039/"), Timeout = TimeSpan.FromMinutes(30) });
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Key").Value))
                };
            });

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


builder.Services.AddAuthorization();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAddressService,AddressService>();
builder.Services.AddScoped<IGuestManagerClient,GuestManagerClient>();
builder.Services.AddScoped<IEmployeeScheduleSer,EmployeeScheduleSer>();
builder.Services.AddResponseCaching(); // Adds response caching, which also enables buffering

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
	;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


// ... (in the Configure method)
app.UseResponseCaching();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();