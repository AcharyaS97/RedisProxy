using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedisProxy.Properties;

namespace RedisProxy {

    public class Startup {

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            var globalExpiry = Configuration.GetValue<int>(StartupArgs.key_GlobalExpiry);
            var redisHostName = Configuration.GetValue<string>(StartupArgs.key_HostName);
            var maxCapacity = Configuration.GetValue<int>(StartupArgs.key_MaxSize);

            Console.WriteLine($"{StartupArgs.key_GlobalExpiry}: {globalExpiry}");
            Console.WriteLine($"{StartupArgs.key_HostName}: {redisHostName}");
            Console.WriteLine($"{StartupArgs.key_MaxSize}: {maxCapacity}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=RedisProxy}/{action=GetFromCache}/{CacheKey}");
            });
        }
    }
}