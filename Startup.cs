using ltbdb.Core.Services;
using ltbdb.Extensions;
using ltbdb.ModelBinders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;
using System.Text;

namespace ltbdb
{
	public class Startup
	{
		private readonly ILogger<Startup> Log;

		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration, IHostingEnvironment env, ILogger<Startup> log)
		{
			Log = log;
			Configuration = configuration;

			Log.LogInformation($"Application ltbdb2 v{System.Reflection.Assembly.GetEntryAssembly().GetName().Version} started.");
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// add options to DI
			services.AddOptions();
			services.Configure<Settings>(Configuration.GetSection("Settings"));

			// DI
			services.AddAutoMapper();
			services.AddScoped(config => config.GetService<IOptionsSnapshot<Settings>>().Value);
			services.AddScoped<MongoContext>();
			services.AddTransient<BookService>();
			services.AddTransient<TagService>();
			services.AddTransient<CategoryService>();
			services.AddTransient<ImageService>();
			services.AddTransient<MaintenanceService>();

			// get settings
			var settings = services.BuildServiceProvider().GetRequiredService<Settings>();

			// key ring
			if (!String.IsNullOrEmpty(settings.KeyStore))
			{
				services.AddDataProtection(options =>
				{
					options.ApplicationDiscriminator = "ltbdb";
				}).PersistKeysToFileSystem(new DirectoryInfo(settings.KeyStore));
			}

			// IIS integration
			services.Configure<IISOptions>(options =>
			{

			});

			// add custom model binders
			services.AddMvc(options =>
			{
				options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());
			}).AddJsonOptions(options =>
			{
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
			});

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Cookie.Name = "ltbdb";
					options.LoginPath = new PathString("/login");
					options.AccessDeniedPath = new PathString("/");
				})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateAudience = true,
						ValidAudience = "ltbdb",
						ValidateIssuer = true,
						ValidIssuer = "ltbdb",
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecurityKey)),
						ValidateLifetime = true,
						ClockSkew = TimeSpan.FromMinutes(5)
					};
				});

			// authorization policies
			services.AddAuthorization(options =>
			{
				options.AddPolicy("AdministratorOnly", policy =>
				{
					policy.RequireRole("Administrator");
				});
			});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
		{
			app.UseWhen(context => !context.Request.Path.StartsWithSegments(new PathString("/api")), branch =>
			{
				branch.UseStatusCodePagesWithReExecute("/error/{0}");

				if (env.IsDevelopment())
				{
					branch.UseDeveloperExceptionPage();
				}
				else
				{
					branch.UseExceptionHandler("/error/500");
				}
			});

			app.UseStaticFiles();

			app.UseAuthentication();

			app.AddRoutes();
		}
	}
}
