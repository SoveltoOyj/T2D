using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace InventoryApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var hostBuilder = new WebHostBuilder()
					.UseKestrel()
					.UseContentRoot(Directory.GetCurrentDirectory())
					.UseIISIntegration()
					.UseStartup<Startup>()
					;

			//if running in AppServices (Azure)
			//asp.net core 1.1.2 is not compatible with Microsoft.AspNetCore.AzureAppServicesIntegration 1.0.2
			var regionName = Environment.GetEnvironmentVariable("REGION_NAME");
			//if (regionName != null)
			//{
			//	hostBuilder.UseAzureAppServices();
			//}

			var host = hostBuilder.Build();


			host.Run();
		}
	}
}
