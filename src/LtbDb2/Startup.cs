using FluentValidation.AspNetCore;
using FluentValidation;
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
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using System.Text;
using System;

namespace LtbDb
{
	public class Startup
	{
		private readonly IConfiguration Configuration;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
#if DEBUG
			IdentityModelEventSource.ShowPII = true;
#endif

			// features
			services.AddFeatureManagement(Configuration.GetSection("FeatureFlags"));

			// add options to DI
			services.AddOptions<AppSettings>()
				.Bind(Configuration.GetSection(AppSettings.SectionName));
			//.ValidateDataAnnotations();

			// get settings for local usage
			var _keyStore = Configuration.GetSection(AppSettings.SectionName).GetValue<string>("KeyStore", null);
			var _signingKey = Configuration.GetSection(AppSettings.SectionName).GetValue<string>("JwtSigningKey", null);
			var _provider = Configuration.GetSection("Database").GetValue<DatabaseProvider>("Provider", DatabaseProvider.PgSql);

			// database context
			services.AddDatabaseContext(Configuration.GetConnectionString("Default"), _provider);

			// dependency injection
			services.AddAutoMapper();

			services.AddTransient<BearerTokenService>();
			services.AddScoped<CustomCookieAuthenticationEvents>();
			services.AddScoped<CustomJwtBearerEvents>();

			services.AddLtbdbServices(_provider);

			// key ring
			if (!String.IsNullOrEmpty(_keyStore))
			{
				services.AddDataProtection(options =>
				{
					options.ApplicationDiscriminator = "ltbdb";
				}).PersistKeysToFileSystem(new DirectoryInfo(_keyStore));
			}

			// IIS integration
			services.Configure<IISOptions>(options => { });

			// lowercase urls - this is a problem for categories and tags
			// services.AddRouting(options =>
			// {
			// 	options.LowercaseUrls = true;
			// });

			// configure MVC
			services.AddMvc(options =>
			{
				options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "Bitte gib eine Zahl ein.");
			})
			.AddNewtonsoftJson(options =>
			{
				//options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			});

			// configure WebAPI 
			services.AddControllers()
			.ConfigureApiBehaviorOptions(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
				options.SuppressMapClientErrors = true;
			});

			// fluent validation
			services.AddFluentValidationAutoValidation(options =>
			{
				options.DisableDataAnnotationsValidation = true;
			})
			.AddFluentValidationClientsideAdapters();

			services.AddValidatorsFromAssemblyContaining<Startup>();

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
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingKey)),
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
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Lustiges Taschenbuch Datenbank API",
					Description = "An Web API for managing Lustiges Taschenbuch Datenbank items."
				});

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

				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});
			services.AddSwaggerGenNewtonsoftSupport();
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
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint($"/swagger/v1/swagger.json", "Lustiges Taschenbuch Datenbank API v1");
				});
			}

			app.UseAuthentication();
			app.UseAuthorization();

			app.AddEndpoints();
		}
	}
}
