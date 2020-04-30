using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStack.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace FoodStack
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var builder = ConfigureDb();

            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.ConnectionString));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }


        private NpgsqlConnectionStringBuilder ConfigureDb()
        {
            var userName = Environment.GetEnvironmentVariable("FOODSTACK_DB_USERNAME");
            var dbName = Environment.GetEnvironmentVariable("FOODSTACK_DB_NAME");
            var host = Environment.GetEnvironmentVariable("FOODSTACK_DB_HOST");
            var password = Environment.GetEnvironmentVariable("FOODSTACK_DB_PASSWORD");

            return new NpgsqlConnectionStringBuilder()
            {
                Username = userName,
                Database = dbName,
                Host = host,
                Password = password,
                Port = 5432,
                Pooling = true,
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            };
        }
    }
}
