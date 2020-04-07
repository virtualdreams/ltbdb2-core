using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;
using System;
using ltbdb.Core.Services;
using ltbdb.Extensions;

namespace ltbdb
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// add options to DI
			services.AddOptions();
			services.Configure<Settings>(Configuration.GetSection("Settings"));

			// get settings for local usage
			var settings = new Settings();
			Configuration.GetSection("Settings").Bind(settings);

			// database context
			services.AddDbContext<MySqlContext>(options =>
			{
				options.UseMySql(settings.ConnectionString, mySqlOptions => { });
				//options.EnableSensitiveDataLogging(true);
			},
			ServiceLifetime.Scoped);

			// DI
			services.AddAutoMapper();
			services.AddTransient<BookService>();
			services.AddTransient<TagService>();
			services.AddTransient<CategoryService>();
			services.AddTransient<MaintenanceService>();
			services.AddTransient<ImageService>();
			services.AddTransient<JwtTokenGenerator>();
			services.AddScoped<CustomTokenEvents>();

			// key ring
			if (!String.IsNullOrEmpty(settings.KeyStore))
			{
				services.AddDataProtection(options =>
				{
					options.ApplicationDiscriminator = "ltbdb";
				}).PersistKeysToFileSystem(new DirectoryInfo(settings.KeyStore));
			}

			// IIS integration
			services.Configure<IISOptions>(options => { });

			// add custom model binders
			services.AddMvc(options =>
			{
				//options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());
			})
			.AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			});

			// add sessions
			// services.AddDistributedMemoryCache();
			// services.AddSession(options =>
			// {
			// 	options.Cookie.Name = "ltbdb_session";
			// 	options.IdleTimeout = TimeSpan.FromMinutes(30);
			// 	options.Cookie.HttpOnly = true;
			// });

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
					options.EventsType = typeof(CustomTokenEvents);
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

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

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

			app.UseRouting();

			// app.UseCors();

			app.UseAuthentication();
			app.UseAuthorization();

			app.AddEndpoints();
		}
	}
}
