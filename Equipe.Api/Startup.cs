using Equipe.Core;
using Equipe.Core.Interface;
using Equipe.Core.Interface.API;
using Equipe.Core.Services;
using Equipe.Core.Services.Background;
using Equipe.Infrastructure.Cache;
using Equipe.Infrastructure.Data;
using Equipe.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Refit;

namespace Equipe.Api
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // Aqui configuramos os serviços
        public void ConfigureServices(IServiceCollection services)
        {
            // Controllers
            services.AddControllers();

            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Equipe API",
                    Version = "v1",
                    Description = "API para gerenciamento de Equipes no sistema"
                });
            });

            services.AddDbContext<EquipeDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                    o => o.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IEquipeHandler, EquipeHandler>();
            services.AddScoped<IEquipeRepository, EquipeRepository>();
            services.AddScoped<IEquipeService, EquipeService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IMensageriaService, MensageriaService>();
            services.AddHostedService<WorkerAtualizaCacheHostedService>();

            services.AddRefitClient<IUsuarioService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5249"));

            services.AddRefitClient<IGinasioService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5183"));

            services.AddRefitClient<IAtletaService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5161"));
                
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "EquipeApp_";
            });
        
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Equipe API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}