using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ParallelUniverse.Spa
{
    public class Program
    {
        public static string DebugString;
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            var webApiUrl = builder.Configuration.GetValue<string>("puWebApi");

            DebugString += webApiUrl;
            DebugString += builder.HostEnvironment.BaseAddress;
            builder.Services.AddScoped(sp => new HttpClient 
            { 
                BaseAddress = new Uri(webApiUrl)
                //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });

            await builder.Build().RunAsync();
        }
    }
}
