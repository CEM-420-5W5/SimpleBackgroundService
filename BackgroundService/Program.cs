using Microsoft.EntityFrameworkCore;
using BackgroundService.Data;
using BackgroundService.Hubs;
using BackgroundService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BackgroundServiceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BackgroundServiceContext") ?? throw new InvalidOperationException("Connection string 'BackgroundServiceContext' not found.")));

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<Spammer>();
builder.Services.AddHostedService<Spammer>(p => p.GetService<Spammer>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200", "https://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<SpammerHub>("/spammer");

app.Run();
