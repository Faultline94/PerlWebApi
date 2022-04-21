using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;

using DbCRUDRepos;
using PearlNecklace;

namespace PerlWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //Basically works similar to how we setup the connection to the database. We also connect up to swagger here.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add the DbContext to the services
            var connectionString = AppConfig.ConfigurationRoot.GetConnectionString("SQLServer_necklaceDB");
            AppLog.Instance.LogDBConnection(connectionString);

            services.AddDbContext<NecklaceDb>(options => options.UseSqlServer(connectionString));
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DbWebApi", Version = "v1" });
            });


            //Dependency Injection for the controller class constructors
            services.AddScoped<INecklaceRepository, NecklaceRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object> //Making sure it doesnt freeze when loading alot of data
                {
                    ["activated"] = false
                };
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DbWebApi v1"); //I am uncertain what I can change this to atm, we will need to get back to this part when we have assembled the project more.
                c.SupportedSubmitMethods(new[] {
                        SubmitMethod.Get, SubmitMethod.Put, SubmitMethod.Delete, SubmitMethod.Post});
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
