using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

using CustomerEquipmentApi.Modules;
using Microsoft.Extensions.PlatformAbstractions;

namespace CustomerEquipmentApi
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; private set; }
        
        public static IServiceProvider ServiceProvider { get; set; }

        public Startup(IConfiguration configuration)
        {
            var environment = "dev";
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{environment}.json", false, true);
            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSingleton(Configuration)
                    .AddMvc(setupAction =>
                    {
                        var jsonOutputFormatter = setupAction.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();

                        setupAction.OutputFormatters.Clear();
                        jsonOutputFormatter?.SupportedMediaTypes.Clear();
                        jsonOutputFormatter?.SupportedMediaTypes.Add("application/json");
                        setupAction.OutputFormatters.Add(jsonOutputFormatter);
                    })
                    .AddControllersAsServices()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });

            services.AddAutofac();

            //since we need get the config data right now we need to instantiate the config wrapper
            var tempServices = new ServiceCollection()
                                    .AddSingleton(Configuration)
                                    .AddMemoryCache();

            var serviceProvider = tempServices.BuildServiceProvider();
            var configWrapper = serviceProvider.GetService<IConfiguration>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Get Available Appointmets API",
                    Version = "v1"
                });


                options.OrderActionsBy((apiDesc) => $"{apiDesc.HttpMethod}");

                options.DescribeAllEnumsAsStrings();
                options.DescribeStringEnumsInCamelCase();
                options.IgnoreObsoleteActions();

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "CustomerEquipmentApi.xml"); //Change this when you generate XML documentation file
                options.IncludeXmlComments(filePath);
            });

            var builder = new ContainerBuilder();

            //// register modules
            builder.RegisterModule<HandlerModule>();
            builder.RegisterModule<RepositoryModule>();

            builder.Populate(services);
            var container = builder.Build();
            services.AddSingleton(container);

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VIRTUAL_PATH")))
            {
                app.Map(Environment.GetEnvironmentVariable("VIRTUAL_PATH"), (myAppPath) => Configure2(myAppPath, env));
            }
            else
            {
                Configure2(app, env);
            }
        }


        /// <summary>
        /// This method gets called by the runtime. This method will configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">Application</param>
        /// <param name="env">Enviroment</param>
        /// <param name="loggerFactory">Logger</param>
        public void Configure2(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            var pathPrefix = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VIRTUAL_PATH")) ? string.Format("{0}/", Environment.GetEnvironmentVariable("VIRTUAL_PATH")) : "/";

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{pathPrefix}swagger/v1/swagger.json", "Get Available Appointments API");
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
