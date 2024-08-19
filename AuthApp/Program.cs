using AuthApp;
using AuthApp.Config;
using AuthApp.PageOperationAuthorization;
using AuthApp.Seed;
using AuthApp.Services;
using AuthAppBusiness.Constants;
using AuthAppBusiness.Implementations;
using AuthAppBusiness.Interfaces;
using AuthAppDataAccess.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options=>
options.UseSqlServer(configuration["ConnectionStrings:DefaultDb"]
,b=>b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
  );

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<CustomeCookieAuthEvents>();  
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);   
    options.EventsType = typeof(CustomeCookieAuthEvents);
});

//
builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomizeAuthorizationProviderPolicy>();
builder.Services.AddScoped<IAuthorizationHandler,PageOperationHandler>();


///
builder.Services.AddScoped<RoleMangerService>();   
///

builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IDoctor, DoctorService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services=scope.ServiceProvider;
    await SuperAdminSeeder.seedSuperAdimn(services);
}    

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{Controller=Auth}/{Action=Login}/{id?}/{returnUrl?}"
        );
    endpoints.MapAreaControllerRoute(
       name: "Doctor",
       areaName: "Doctor",
       pattern: "Doctor/{Controller=Auth}/{Action=Login}/{id?}/{returnUrl?}"
       );

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

});




app.Run();
