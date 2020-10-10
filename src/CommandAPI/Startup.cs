using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CommandAPI.Data;
using Npgsql;
using AutoMapper;
using Newtonsoft.Json.Serialization;

namespace CommandAPI
{
    public class Startup
    {
        public IConfiguration Configuration{get;}
        public Startup(IConfiguration configuration){
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //The Secret Manager Tool
            //dotnet user-secrets set "UserID" "sddsf"
            //dotnet user-secrets set "Password" "sddsf"
            //Windows: %APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json
            //Linux/OSX: ~/.microsoft/usersecrets/<user_secrets_id>/secrets.json

            var builder = new NpgsqlConnectionStringBuilder();
            builder.ConnectionString = Configuration.GetConnectionString("PostgreSqlConnection");
            builder.Username = Configuration["UserID"];
            builder.Password = Configuration["Password"];

            services.AddDbContext<CommandContext>(opt => 
                opt.UseNpgsql(builder.ConnectionString));

            services.AddControllers().AddNewtonsoftJson(s=>{
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICommandAPIRepo, SqlCommandAPIRepo>();
            //AddTransient: a service is created each time it is requested from the service container
            //AddScoped: a service is created once per client request(connectoin).
            //AddSingleton: a service is created once and reused.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapGet("/", async context =>
                // {
                //     await context.Response.WriteAsync("Hello World!");
                // });
            });
        }
    }
}
