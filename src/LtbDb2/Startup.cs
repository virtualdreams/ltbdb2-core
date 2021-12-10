using FluentValidation.AspNetCore;
using LtbDb.Core;
using LtbDb.Events;
using LtbDb.Extensions;
using LtbDb.Options;
using LtbDb.Provider;
using LtbDb.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text;
using System;

namespace LtbDb
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
#if DEBUG
			IdentityModelEventSource.ShowPII = true;
#endif
			// add options to DI
			services.AddOptions<AppSettings>()
				.Bind(Configuration.GetSection(AppSettings.AppSettingsName));
			//.ValidateDataAnnotations();

			// get settings for local usage
			var _settings = Configuration.GetSection(AppSettings.AppSettingsName).Get<AppSettings>();
			var _provider = Configuration.GetSection("Database").GetValue<DatabaseProvider>("Provider");

			// database context
			services.AddDatabaseContext(Configuration.GetConnectionString("Default"), _provider);

			// DI
			services.AddAutoMapper();
			services.AddLtbdbServices(_provider);
			services.AddTransient<BearerTokenService>();
			services.AddScoped<CustomCookieAuthenticationEvents>();
			services.AddScoped<CustomJwtBearerEvents>();

			// key ring
			if (!String.IsNullOrEmpty(_settings.KeyStore))
			{
				services.AddDataProtection(options =>
				{
					options.ApplicationDiscriminator = "ltbdb";
				}).PersistKeysToFileSystem(new DirectoryInfo(_settings.KeyStore));
			}

			// IIS integration
			services.Configure<IISOptions>(options => { });

			// configure MVC
			services.AddMvc(options =>
			{
				options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "Bitte gib eine Zahl ein.");
			})
			.AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			})
			.AddFluentValidation(options =>
			{
				options.RegisterValidatorsFromAssemblyContaining<Startup>();
				options.DisableDataAnnotationsValidation = true;
			});

			// configure WebAPI 
			services.AddControllers()
			.ConfigureApiBehaviorOptions(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
				options.SuppressMapClientErrors = true;
			});

			// add distributed cache
			// services.AddDistributedMemoryCache();

			// add sessions
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
				options.EventsType = typeof(CustomCookieAuthenticationEvents);
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
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.AccessTokenKey)),
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
				options.EventsType = typeof(CustomJwtBearerEvents);
			});

			// authorization policies
			services.AddAuthorization(options =>
			{
				options.AddPolicy("AdministratorOnly", policy =>
				{
					policy.RequireRole("Administrator");
				});
			});

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey
				});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[] { }
					}
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

			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthentication();
			app.UseAuthorization();

			app.AddEndpoints();
		}
	}
}
