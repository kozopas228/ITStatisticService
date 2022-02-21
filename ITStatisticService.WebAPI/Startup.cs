using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITStatisticService.Data;
using ITStatisticService.Logic.Implementation.DjinniCO;
using ITStatisticService.Logic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

namespace ITStatisticService.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ITStatisticService.WebAPI", Version = "v1"});
            });
            services.AddHttpClient();
            services.AddDbContext<ApplicationContext>();
            
            services.AddTransient<IDjinniCoParserSettings>(x => new DjinniCoParserSettings
            {
                PagesAmount = Int32.Parse(Configuration.GetSection("PagesAmount").Value),
                BaseUrl = Configuration.GetSection("DjinniCO").GetSection("baseUrl").Value,
                CertainUrl = Configuration.GetSection("DjinniCO").GetSection("certainUrl").Value,
            });
            services.AddTransient<IDjinniCoParser, DjinniCoParser>();
            
            services.AddTransient<StatisticService>();
            services.AddTransient<ILoggingService, LoggingService>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ITStatisticService.WebAPI v1"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}