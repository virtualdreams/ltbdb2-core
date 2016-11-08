using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Authentication;

//using Microsoft.Extensions.Logging;

using SimpleInjector;
using SimpleInjector.Integration.AspNetCore;
using SimpleInjector.Integration.AspNetCore.Mvc;

using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using ltbdb.ModelBinders;
using AutoMapper;
using ltbdb.Core.Models;
using ltbdb.Models;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace ltbdb
{
	public class Startup
	{
		private Container Container = new Container();

		public IConfigurationRoot Configuration { get; }

		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(options => {
				//options.ModelBinderProviders.Add(typeof(ObjectId), new Object());
				options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());
			}).AddJsonOptions(options => {
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
			});
			
			services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(Container));
			services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(Container));
		}
		
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			// initialize AutoMapper
			InitializeAutoMapper();

			// initialize simple injector
			app.UseSimpleInjectorAspNetRequestScoping(Container);
			Container.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();
			InitializeSimpleInjector(app);

			/*if(env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/error/index");
			}*/

			app.UseDeveloperExceptionPage();
			app.Use(async (context, next) => {
				await next();
				if(context.Response.StatusCode == 404)
				{
					context.Request.Path = "/error/http404";
					await next();
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

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "areaRoute",
    				template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
				);

				routes.MapRoute(
					name: "Search",
					template: "search",
					defaults: new { controller = "Home", action = "search"}
				);

				routes.MapRoute(
					name: "Book",
					template: "book/{id?}/{slug?}",
					defaults: new { controller = "Book", action = "View"},
					constraints: new { id = @"^[a-f0-9]{24}$" }
				);

				routes.MapRoute(
					name: "Tags",
					template: "tags",
					defaults: new { controller = "tag", action = "index" }
				);

				routes.MapRoute(
					name: "Tag",
					template: "tag/{id?}",
					defaults: new { controller = "tag", action = "view" }
				);

				routes.MapRoute(
					name: "Categories",
					template: "categories",
					defaults: new { controller = "category", action = "index" }
				);

				routes.MapRoute(
					name: "Category",
					template: "category/{id?}",
					defaults: new { controller = "category", action = "view" }
				);

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}"
				);
			});
		}
		
		private void InitializeSimpleInjector(IApplicationBuilder app)
		{
			Container.RegisterMvcControllers(app);
			Container.RegisterMvcViewComponents(app);

			Container.Register<IMongoClient>(() => new MongoClient("mongodb://127.0.0.1/"), Lifestyle.Scoped);
			//Container.Register<DemoService>(Lifestyle.Scoped);

			Container.Verify();
		}

		private void InitializeAutoMapper()
		{
			Mapper.Initialize(cfg => {
				cfg.CreateMap<Book, BookModel>();
				cfg.CreateMap<Book, BookWriteModel>()
					.ForMember(d => d.Tags, map => map.MapFrom(s => String.Join("; ", s.Tags)))
					.ForMember(d => d.Image, map => map.Ignore())
					.ForMember(d => d.Remove, map => map.Ignore());

				cfg.CreateMap<BookWriteModel, Book>()
					.ForMember(d => d.Title, map => map.MapFrom(s => s.Title.Trim()))
					.ForMember(d => d.Category, map => map.MapFrom(s => s.Category.Trim()))
					.ForMember(s => s.Created, map => map.Ignore())
					.ForMember(d => d.Filename, map => map.Ignore())
					.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories.Select(x => x.Trim()).Where(w => !String.IsNullOrEmpty(w))))
					.ForMember(d => d.Tags, map => map.MapFrom(s => s.Tags.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(w => !String.IsNullOrEmpty(w))))
					.ForSourceMember(s => s.Image, map => map.Ignore())
					.ForSourceMember(s => s.Remove, map => map.Ignore());
			});
		}
	}
}
