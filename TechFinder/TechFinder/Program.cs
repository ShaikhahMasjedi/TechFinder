using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TechFinder.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


var cultureInfo = new CultureInfo("en-US");
cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;




// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TechFinder_storeContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("Tech")));

builder.Services.AddScoped<UserService>();

var app = builder.Build();

//var lifetime = app.Lifetime;
//var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

/////////
using (var scope = app.Services.CreateScope())
{
    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
    userService.ResetAllUsersLoginStatus();
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Interface2}/{action=Index}/{id?}");

app.Run();
