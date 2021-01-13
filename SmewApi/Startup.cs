using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Concurrency;
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
using Serilog;
using Smew.Handlers;
using Smew.Infrastructure;
using SmewApi.Infrastructure;
using StackExchange.Redis;

namespace SmewApi {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            Console.WriteLine (Configuration.ToString ());

            services.AddControllers ();
            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new OpenApiInfo { Title = "SmewApi", Version = "v1" });
            });

            services.Configure<AppInfo> (Configuration.GetSection (AppInfo.Kind));
            services.Configure<RedisConfig> (Configuration.GetSection (RedisConfig.Kind));

            services.AddSingleton<IReactiveEventBroker, ReactiveEventBroker> ();
            services.AddSingleton<IRedisProvider, RedisProvider> ();
            services.AddSingleton<IMessageBus, RedisMessageBus> ();

            services.AddHostedService<GetMessageResponseHandler> ();
            services.AddHostedService<PostMessageHandler> ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }
            app.UseSwagger ();
            app.UseSwaggerUI (c => c.SwaggerEndpoint ("/swagger/v1/swagger.json", "SmewApi v1"));

            app.UseHttpsRedirection ();

            app.UseSerilogRequestLogging (options => options.EnrichDiagnosticContext = RequestLogEnricher.EnrichFromRequest);
            app.UseRouting ();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}