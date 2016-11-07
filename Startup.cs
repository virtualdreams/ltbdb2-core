using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace aspnetcoreapp
{
	public class Startup
	{
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
			Console.WriteLine("services...");
			services.AddMvc();
		}
		
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			Console.WriteLine("configure...");
			//if (env.IsDevelopment())
			//{
				Console.WriteLine("devel...");
				app.UseDeveloperExceptionPage();
				//app.UseBrowserLink();
			//}
			//else
			//{	
			//	Console.WriteLine("prod...");
			//	app.UseExceptionHandler("/Home/Error");
			//}

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
