using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.Model.BaseEntity;
using Microsoft.Extensions.Options;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddRazorPages();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  // read Serilog section from appsettings.json
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddScoped<RateLimitFilter>();
builder.Services.AddScoped<SessionValidationFilter>();
builder.Services.AddScoped<RoleRestrictionFilter>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<RateLimitFilter>();
    options.Filters.Add<SessionValidationFilter>();
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<RoleRestrictionFilter>();
});


// Name your service for resource data.
var serviceName = "UrbanProject";
builder.Services.AddOpenTelemetry()
    .ConfigureResource(rb => rb.AddService(serviceName))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()           // trace incoming HTTP requests:contentReference[oaicite:7]{index=7}
        .AddConsoleExporter() //badan hazf beshe va package OpenTelemetry.Exporter.Console ham pak she
    );


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// Make it available via DI if you need it elsewhere too
builder.Services.AddSingleton(new DbConfig(""));
builder.Services.AddDbContext<FromUrbanDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpContextAccessor();



builder.Services.AddIdentity<CostumIdentityUser, CostumIdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 1;
        options.Password.RequiredUniqueChars = 0;
    })
    .AddEntityFrameworkStores<FromUrbanDbContext>()
    .AddDefaultTokenProviders();


// Register the custom validator
builder.Services.AddScoped<ISecurityStampValidator, OptimizedSecurityStampValidator>();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(1); // Increase if needed
});

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "CSRF-TOKEN";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.SuppressXFrameOptionsHeader = false;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<CostumIdentityUser>, CustomClaimsPrincipalFactory>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "__Host-Auth";   // ✅ Must start with "__Host-"
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    // هرگاه بیش از نیمی از این زمان گذشت، کوکی تمدید شود
    options.SlidingExpiration = true;
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    // عدم تنظیم Expiration به معنای موقتی بودن کوکی است
    options.Cookie.Expiration = null; // کوکی با بسته شدن مرورگر منقضی می‌شود

    options.LogoutPath = "/Login/Logout";

    options.LoginPath = "/Login/Index";  // تعیین مسیر ورود سفارشی
    options.AccessDeniedPath = "/Error/Error403"; // مسیر عدم دسترسی

});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#region Model Services
builder.Services.ConfigureApplicationServices(builder.Configuration);
#endregion


// Add services
builder.Services.AddMemoryCache();
builder.Services.Configure<BotDetectionSettings>(builder.Configuration.GetSection("BotDetection"));
builder.Services.AddTransient<BotDetectionMiddleware>(); // Register as transient (per-request)
builder.Services.AddTransient<IPRestrictionMiddleware>(); //Check if ip address is blocked

builder.Services.Configure<VaultOptions>(builder.Configuration.GetSection("Vault"));

builder.Services.AddHttpClient("VaultClient", (sp, client) =>
{
    var opts = sp.GetRequiredService<IOptions<VaultOptions>>().Value;
    client.BaseAddress = new Uri(opts.Address);
});

builder.Services.AddSingleton<VaultTokenProvider>();
builder.Services.AddSingleton<IEncryptionService, VaultEncryptionService>();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

var app = builder.Build();

app.UseMiddleware<SecurityLoggingMiddleware>();

app.UseMiddleware<IPRestrictionMiddleware>();


app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

#region CSP Middleware
//string cspPolicy = "default-src 'self'; connect-src 'self' http://localhost:* ws://localhost:* wss://localhost:*; img-src 'self' data:; style-src 'self' 'unsafe-inline'; script-src 'self';";
//app.UseMiddleware<CspMiddleware>(cspPolicy);

string cspPolicy = string.Join(" ",
    "default-src 'self';",
    "connect-src 'self' http://localhost:* ws://localhost:* wss://localhost:*;",
    "img-src 'self' data:;",
    "style-src 'self';",                 // حذف unsafe-inline
    "script-src 'self';",
    "form-action 'self';",              // اضافه شده طبق گزارش ZAP
    "frame-ancestors 'none';"           // اضافه شده برای جلوگیری از Clickjacking
);
app.UseMiddleware<CspMiddleware>(cspPolicy);


#endregion


app.UseMiddleware<BotDetectionMiddleware>(); // Add factory middleware
app.UseWebSockets();

app.UseStatusCodePagesWithReExecute("/Error/Error{0}");


// Only log detailed info in development
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}
//else
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHsts();
//}
app.UseExceptionHandler("/Error");
app.UseHsts();

app.UseHttpsRedirection();

// Add Serilog request logging
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
        diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString());

        // Security-relevant enrichment
        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            diagnosticContext.Set("UserId", httpContext.User.Identity.Name);
        }
    };
});

app.UseRouting();

// استفاده از کانفیگ environment
var env = builder.Environment;
// فقط در محیط Production مایگریت کن
if (env.IsProduction())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<FromUrbanDbContext>();

        try
        {
            dbContext.Database.Migrate(); // مایگریت خودکار
        }
        catch (Exception ex)
        {
            // لاگ گرفتن اگر خطا رخ داد
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "خطا هنگام اجرای مایگریشن دیتابیس در محیط Production");
            throw; // اختیاری: اگر بخوای اپ متوقف شه
        }
    }
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=User}/{action=Index}/{id?}")
        .RequireRateLimiting("fixed"); // Apply the "fixed" policy

    _ = endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Login}/{action=Index}/{id?}")
        .RequireRateLimiting("fixed"); // Apply the "fixed" policy
});

app.MapRazorPages().WithStaticAssets();
try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}