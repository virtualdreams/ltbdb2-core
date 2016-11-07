using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;

//using Microsoft.Extensions.Logging;

using SimpleInjector;
using SimpleInjector.Integration.AspNetCore;
using SimpleInjector.Integration.AspNetCore.Mvc;

using MongoDB.Driver;

namespace ltbdb
{
	public class Startup
	{
		private Container Container = new Container();

		public IConfigurationRoot Configuration { get; }

		public Startup(IHostingEnvironment env)
		{
			Console.WriteLine("startup...");
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
			
			services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(Container));
			services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(Container));
		}
		
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseSimpleInjectorAspNetRequestScoping(Container);
			Container.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();

			// initialize simple injector
			InitializeContainer(app);

			app.UseDeveloperExceptionPage();
			app.UseStaticFiles();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
		
		private void InitializeContainer(IApplicationBuilder app)
		{
			Container.RegisterMvcControllers(app);
			Container.RegisterMvcViewComponents(app);

			Container.Register<IMongoClient>(() => new MongoClient("mongodb://127.0.0.1/"), Lifestyle.Scoped);
			Container.Register<DemoService>(Lifestyle.Scoped);

			Container.Verify();
		}
	}
}
