using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using ParallelUniverse.RazorPages.Data;

namespace ParallelUniverse.RazorPages
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Logout";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.Cookie.HttpOnly = true;
                });
            services.AddDbContext<ParallelUniverseContext>(ops => 
                ops.UseMySQL(Configuration.GetConnectionString("ParallelUniverse")));
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ParallelUniverseContext>();
                context.Database.Migrate();
            }

            app.Use(async (ctx, next) =>
            {
                if(ctx.Request.Headers.TryGetValue(HeaderNames.UserAgent, out var userAgent))
                {
                    if(userAgent.ToString().Contains("MicroMessenger", StringComparison.OrdinalIgnoreCase))
                    {
                        var content = File.ReadAllBytes(Path.Combine(env.WebRootPath, "notsupport.html"));
                        await ctx.Response.BodyWriter.WriteAsync(content);
                        return;
                    }
                }

                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages()
                    .RequireAuthorization();
            });
        }
    }
}
