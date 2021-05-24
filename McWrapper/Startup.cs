using System.IO;
using System.Text.Json.Serialization;
using Dapper;
using McWrapper.Config.Mapping;
using McWrapper.Postgres;
using McWrapper.Repositories.Jars;
using McWrapper.Repositories.McWrapper;
using McWrapper.Repositories.Plugins;
using McWrapper.Repositories.PluginsServer;
using McWrapper.Repositories.Servers;
using McWrapper.Services.Jars;
using McWrapper.Services.McWrapper;
using McWrapper.Services.Plugins;
using McWrapper.Services.Servers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace McWrapper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();
            
            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.Configure<PostgresOptions>(Configuration.GetSection("Postgres"));
            
            services.AddTransient(p => PostgresConnectionFactory.CreatePostgresConnection(p.GetRequiredService<IOptions<PostgresOptions>>().Value));
            
            // Repositories
            services.AddTransient<IJarRepository, JarRepository>();
            services.AddTransient<IPluginRepository, PluginRepository>();
            services.AddTransient<IServerRepository, ServerRepository>();
            services.AddTransient<IPluginServerRepository, PluginServerRepository>();
            services.AddTransient<IMcWrapperRepository, McWrapperRepository>();
            
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            
            services.AddRouting(options =>
            {
                options.AppendTrailingSlash = true;
                options.LowercaseUrls = true;
            });

            // Services
            services.AddTransient<IJarService, JarService>();
            services.AddTransient<IPluginService, PluginService>();
            services.AddTransient<IServerService, ServerService>();
            services.AddTransient<IMcWrapperService, McWrapperService>();
            
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.SubstituteApiVersionInUrl = true;
                options.GroupNameFormat = "'v'VVV";
            });
            
            services.AddSwaggerGen(options => options.CustomSchemaIds(type => type.ToString()));
            services.AddAutoMapper();

            //TODO: check if this needs to be removed before release (since we use env.IsDevelopment() in the Configure method)
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder => { builder.WithOrigins("http://localhost:3000"); });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServerService serverService)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "McWrapper API V1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            if (env.IsDevelopment())
            {
                app.UseCors(MyAllowSpecificOrigins);
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            //serverService.Initialize();
            McWrapperLib.Program.CreateDirectories();
            CreateDirectories();
        }

        private void CreateDirectories()
        {
            Directory.CreateDirectory(JarService.JarPath);
        }
    }
}