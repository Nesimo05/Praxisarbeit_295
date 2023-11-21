using Microsoft.EntityFrameworkCore;
using Ski_Service_Backend.Model;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Serilog.Events;
using Serilog;
using Ski_Service_Backend.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    { 
        // Logger-Konfiguration
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            // Host erstellen und Anwendung ausführen
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            // Logge eine Ausnahme, falls der Host unerwartet beendet wird
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            // Logger schließen und leeren
            Log.CloseAndFlush();
        }
    }

    // Host-Builder erstellen
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                // Anwendungskonfiguration mit der Startup-Klasse
                webBuilder.UseStartup<Startup>();
            });
}

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    // Dienstkonfiguration
    public void ConfigureServices(IServiceCollection services)
    {
        // Dienstkonfiguration, z. B., DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddSingleton<ITokenService, TokenService>();

        // Zusätzliche Dienste
        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            // Swagger-Dokumentkonfiguration
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });     
        });

        //
        // JWT
        //
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
    }
    // Anwendungs- und Middleware-Konfiguration
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            // Konfiguration für die Entwicklungsumgebung
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            // Konfiguration für die Produktionsumgebung
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Middleware für das Routing
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        // Middleware für Endpunkte
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        
        
        
    }
}
