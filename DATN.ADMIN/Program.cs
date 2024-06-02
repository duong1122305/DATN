using DATN.ADMIN.IServices;
using DATN.ADMIN.Services;
using DATN.ViewModels.DTOs.Authenticate;
using MudBlazor.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped(_http => new HttpClient { BaseAddress = new Uri("https://localhost:7039/") });
builder.Services.AddScoped<IUserClientSev, UserClienSev>();
builder.Services.AddScoped<UserLoginView>();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
