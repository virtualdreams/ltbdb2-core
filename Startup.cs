using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using ltbdb.Core.Helpers;
using ltbdb.Core.Services;
using ltbdb.ModelBinders;
using Newtonsoft.Json;
using System.Text;
using ltbdb.Extensions;

namespace ltbdb
{
    public class Startup
	{
		public IConfigurationRoot Configuration { get; }

		static public ILoggerFactory Logger { get; private set; }

		public Startup(IHostingEnvironment env, ILoggerFactory logger)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("Config/appsettings.json", optional: true, reloadOnChange: false)
				.AddEnvironmentVariables();

			Configuration = builder.Build();

			Logger = logger;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddLogging();

			services.AddMvc(options => {
				options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());
			}).AddJsonOptions(options => {
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
			});

			services.AddAuthorization(options => {
				options.AddPolicy("AdministratorOnly", policy => {
					policy.RequireRole("Administrator");
				});
			});
			
			// dependency injection
			services.AddAutoMapper();
			services.AddScoped<IMongoClient>(options => new MongoClient(GlobalConfig.Get().MongoDB));
			services.AddTransient<BookService>();
			services.AddTransient<TagService>();
			services.AddTransient<CategoryService>();
			services.AddTransient<UserService>();
		}
		
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
		{
			// add logger
			/*
			logger.WithFilter(new FilterLoggerSettings{
				{ "Microsoft", LogLevel.Warning},
				{ "System", LogLevel.Warning },
				{ "ltbdb", LogLevel.Debug }
			}).AddConsole(LogLevel.Debug);
			*/
			logger.AddConsole(Configuration.GetSection("Logging"));

			if(env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/error/index");
			}
			
			app.Use(async (context, next) => {
				await next();
				if(context.Response.StatusCode == 404)
				{
					if(context.Request.Path.StartsWithSegments("/api"))
					{
						context.Response.ContentType = "application/json";
						await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = "Not found." }, Formatting.Indented), Encoding.UTF8);
					}
					else
					{
						context.Request.Path = "/error/http404";
						await next();
					}
				}
			});

			app.UseStaticFiles();

			app.UseCookieAuthentication(new CookieAuthenticationOptions(){
				AuthenticationScheme = "ltbdb",
				CookieName = "ltbdb",
				LoginPath = new PathString("/account/login"),
				AccessDeniedPath = new PathString("/account/login"),
				AutomaticAuthenticate = true,
				AutomaticChallenge = true
			});

			app.AddRoutes();
		}
	}
}
