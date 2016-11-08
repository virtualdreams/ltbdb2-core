using System.IO;
using ltbdb.Core.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace ltbdb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // read the config
            GlobalConfig.Get();

            // configure the server
            var host = new WebHostBuilder()
				.UseKestrel()
				.UseUrls(GlobalConfig.Get().Kestrel)
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseStartup<Startup>()
				.Build();
            
            // start
			host.Run();
        }
    }
}
