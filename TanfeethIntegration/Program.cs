using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using TanfeethIntegration.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.EntityFrameworkCore;
using TanfeethIntegration.Data;
using TanfeethIntegration.Models;
using Microsoft.AspNetCore.Identity;
using NLog.Web;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Error);
builder.Host.UseNLog();

// Configure services
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LogTanfeethIntegration"))
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<LogDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // Set session timeout to 20 minutes
    options.SlidingExpiration = true; // Reset the session timeout on each request
});

#if DEBUG
// In Debug configuration, add Razor Runtime Compilation to enable on-the-fly view updates during development
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
#else
// In Release configuration, don't add Razor Runtime Compilation for performance reasons
builder.Services.AddControllersWithViews();
#endif

// HttpClient uses the operating system certificate trust store.
// Production certificates must be trusted by the hosting server; TLS validation is not bypassed.
builder.Services.AddHttpClient<ILookupService, LookupService>();
builder.Services.AddHttpClient<IGovAgencyRequestService, GovAgencyRequestService>();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Set session timeout to 20 minutes
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add scoped services
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<IGovAgencyRequestService, GovAgencyRequestService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Apply any pending migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LogDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline
var env = app.Services.GetRequiredService<IWebHostEnvironment>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/CustomErrors/500.html");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/CustomErrors/{0}.html");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;
    if (path.Contains("."))
    {
        var filePath = Path.Combine(env.ContentRootPath, path.TrimStart('/'));
        if (File.Exists(filePath))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access denied.");
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Request.Path = "/CustomErrors/404.html";
            await next();
        }
    }
    else
    {
        await next();
    }
});

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status404NotFound && !context.Response.HasStarted)
    {
        context.Request.Path = "/CustomErrors/404.html";
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await next();
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
