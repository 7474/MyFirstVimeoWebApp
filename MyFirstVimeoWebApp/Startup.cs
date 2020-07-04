using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFirstVimeoWebApp.Models;
using VimeoOpenApi.Api;
using VimeoOpenApi.Client;

namespace MyFirstVimeoWebApp
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
            services.AddControllersWithViews();

            var vimeoAccessToken = Configuration.GetValue<string>("Vimeo:AccessToken");
            var vimeoAppUserId = Configuration.GetValue<string>("Vimeo:AppUserId");
            services.AddSingleton(s => new Configuration
            {
                AccessToken = vimeoAccessToken,
            });
            services.AddSingleton<IVimeoConfig>(s => new VimeoConfig
            {
                AppUserId = decimal.Parse(vimeoAppUserId),
            });
            // XXX これどのコンストラクタが選択されるんだろう。明示的に指定した方が良さそう。
            services.AddSingleton<IAPIInformationEssentialsApi, APIInformationEssentialsApi>();
            services.AddSingleton<IVideosEssentialsApi, VideosEssentialsApi>();
            services.AddSingleton<IVideosUploadsApi, VideosUploadsApi>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
