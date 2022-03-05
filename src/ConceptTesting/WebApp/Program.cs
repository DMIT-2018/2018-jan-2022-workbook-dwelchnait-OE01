
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

#region Additional Namespaces
using ChinookSystem;
using System.Configuration;
#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//supplied because at creation I requested Individual Accounts
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//coded for Chinook database
var connectionStringChinook = builder.Configuration.GetConnectionString("ChinookDB");

//given
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//coded to call the Backend Startup Extension to register services
builder.Services.AddBackendDependencies(options =>
    options.UseSqlServer(connectionStringChinook));

//given
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
