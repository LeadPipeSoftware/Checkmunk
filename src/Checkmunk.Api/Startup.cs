using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using AutoMapper;
using Checkmunk.Application.Common.Behaviors;
using Checkmunk.Data.Contexts;
using Checkmunk.Data.SeedData;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Scrutor;
using Swashbuckle.AspNetCore.Swagger;

namespace Checkmunk.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

			Audit.Core.Configuration.Setup()
                 .UseDynamicProvider(config => config
                                     .OnInsert(ev => Console.WriteLine($"AUDIT: [{ev.StartDate}] {ev.Environment.UserName}->{ev.EventType}")));
        }

        public IConfiguration Configuration { get; }

		private static string XmlCommentsFilePath
		{
			get
			{
				var basePath = PlatformServices.Default.Application.ApplicationBasePath;
				var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
				return Path.Combine(basePath, fileName);
			}
		}

		/*
         * This method is called before the Configure() method. By convention,
         * configuration options are set in this method.
         */
		public void ConfigureServices(IServiceCollection services)
        {
			// Add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
			// Note: the specified format code will format the version as "'v'major[.minor][-status]"
			services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            services.AddMvc()
                .AddFluentValidation(config =>
                {
                    config.RegisterValidatorsFromAssemblyContaining<Application.Checklists.AssemblyMarker>();
                    config.RegisterValidatorsFromAssemblyContaining<Application.Users.AssemblyMarker>();
                });

			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());
			});

            services.AddApiVersioning(options => options.ReportApiVersions = true);

            services.AddMediatR(
                typeof(Application.Checklists.AssemblyMarker).GetTypeInfo().Assembly,
                typeof(Application.Users.AssemblyMarker).GetTypeInfo().Assembly);

            //services.Scan(scan =>
                          //scan.FromAssemblyOf<Application.AssemblyMarker>()
                              //.AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehavior<,>)))
                              //.AsImplementedInterfaces()
                              //.WithTransientLifetime());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TimerBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidateBehavior<,>));

			services.AddDbContext<CheckmunkContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

			services.AddSwaggerGen(
				options =>
				{
					// Resolve the IApiVersionDescriptionProvider service
					// Note: that we have to build a temporary service provider here because one has not been created yet
					var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

					// Add a swagger document for each discovered API version
					foreach (var description in provider.ApiVersionDescriptions)
					{
						options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
					}

					// Add a custom operation filter which sets default values
					options.OperationFilter<SwaggerDefaultValues>();

					// Integrate XML comments
					options.IncludeXmlComments(XmlCommentsFilePath);

				    options.CustomSchemaIds(x => x.FullName);
                });

            services.AddMetrics()
                    .AddJsonMetricsSerialization()
                    .AddJsonMetricsTextSerialization()
                    .AddJsonHealthSerialization()
                    .AddJsonEnvironmentInfoSerialization()
                    .AddHealthChecks()
                    .AddMetricsMiddleware();

            services.AddAutoMapper(
                typeof(Application.Checklists.AssemblyMarker),
                typeof(Application.Users.AssemblyMarker));
        }

        /*
         * This method is called after the ConfigureServices() method and is
         * where we determine how ASP.NET will respond to HTTP requests.
         */
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CheckmunkContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			app.UseMetrics();

            app.UseCors("CorsPolicy"); // This MUST be called before UseMvc

			app.UseMvc();

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkmunk API v1");
			});

			using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
                serviceScope.ServiceProvider.GetService<CheckmunkContext>().Database.EnsureDeleted();
                serviceScope.ServiceProvider.GetService<CheckmunkContext>().Database.Migrate();
			}

            try
            {
				TestDataSeeder.SeedDatabase(context);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

		private static Info CreateInfoForApiVersion(ApiVersionDescription description)
		{
			var info = new Info()
			{
				Title = $"Checkmunk API {description.ApiVersion}",
				Version = description.ApiVersion.ToString(),
				Description = "The programmer's interface to Checkmunk.",
				Contact = new Contact() { Name = "Greg Major", Email = "greg@checkmunk.com" },
				TermsOfService = "Open Source",
				License = new License() { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
			};

			if (description.IsDeprecated)
			{
				info.Description += " This API version has been deprecated.";
			}

			return info;
		}
    }
}
