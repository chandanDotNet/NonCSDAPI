using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using POS.API;
using POS.API.Helpers;
using POS.Domain;
using System;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<JobService>();

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

builder.Logging.ClearProviders();
builder.Host.UseNLog();
var app = builder.Build();

try
{
    using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<POSDbContext>();
        context.Database.Migrate();
    }
}
catch (System.Exception)
{
    throw;
}

ILoggerFactory loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
startup.Configure(app, app.Environment, loggerFactory);

app.UseHangfireDashboard();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHangfireDashboard();
});

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "skd-non-csd-firebase-adminsdk-8pk2m-2286c6be47.json")),
});

JobService jobService = app.Services.GetRequiredService<JobService>();
jobService.StartScheduler();
app.Run();
