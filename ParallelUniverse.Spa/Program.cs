using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Routing;
using Blazored.SessionStorage;

namespace ParallelUniverse.Spa
{
    public class Program
    {
        public static string DebugString;
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<UniverseAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<UniverseAuthStateProvider>());
            builder.RootComponents.Add<App>("app");
            var webApiUrl = builder.Configuration.GetValue<string>("puWebApi");

            //DebugString += webApiUrl;
            //DebugString += builder.HostEnvironment.BaseAddress;
            builder.Services.AddSingleton(sp => new HttpClient
            {
                BaseAddress = new Uri(webApiUrl)
                //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });

            await builder.Build().RunAsync();
        }
    }
}
