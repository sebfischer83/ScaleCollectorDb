using System.Security.Claims;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScaleCollectorDbServer.Data;
using ScaleCollectorDbServer.Services.UserResolver;
using Serilog;
using StackExchange.Profiling.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("scale");

// Add services to the container.
builder.Services.AddCors(options =>
 {
     options.AddPolicy(name: "CorsPolicy",
                     builder =>
                     {
                         builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                     });
 });

string connectionString = builder.Configuration["Scale:ConnectionString"];

builder.Services.AddDbContext<ScaleDbContext>(options =>
                options.UseSqlServer(connectionString));

string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = domain;
            options.Audience = builder.Configuration["Auth0:Audience"];
            // If the access token does not have a `sub` claim, `User.Identity.Name` will be `null`. Map it to a different claim by setting the NameClaimType below.
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = ClaimTypes.NameIdentifier
            };
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:weather", policy => policy.Requirements.Add(new HasScopeRequirement("read:weather", domain)));
});

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

builder.Services.AddControllers()
     .AddNewtonsoftJson(options =>
     {
         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
     });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks().AddSqlServer(connectionString, "SELECT 1");

builder.Services.AddHealthChecksUI().AddInMemoryStorage();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/mini-profiler-resources";
    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

    options.ResultsAuthorize = request => true;
    options.ResultsListAuthorize = request => true;
    
    options.TrackConnectionOpenClose = true; options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;
}).AddEntityFramework();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<UserResolverService>();
builder.Services.AddTransient<ExtendedScaleDbContext>();

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseMiniProfiler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();

app
    .UseRouting()
    .UseAuthentication()
        .UseAuthorization()
    .UseEndpoints(config =>
    {
        config.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        config.MapHealthChecksUI(a => a.UIPath = "/healthzui");
        config.MapDefaultControllerRoute();
    });


app.MapControllers();

//app.MapHealthChecks("/healthz");
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
    db.Database.Migrate();
}

app.Run();
