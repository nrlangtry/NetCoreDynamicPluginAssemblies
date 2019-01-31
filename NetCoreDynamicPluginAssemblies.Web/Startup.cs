using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreDynamicPluginAssemblies.Web
{
    public class Startup
    {
        private readonly string localPluginsPath;
        private readonly string pluginWatchPath;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;

            // Setup local plugin cache directory
            localPluginsPath = Path.Combine(hostingEnvironment.ContentRootPath, "Plugins");
            if (!Directory.Exists(localPluginsPath.ToString()))
                Directory.CreateDirectory(localPluginsPath.ToString());

            // Setup directory to watch for Plugin changes
            pluginWatchPath = hostingEnvironment.ContentRootPath + configuration["Extensions:Path"];
            pluginWatchPath = System.IO.Path.GetFullPath(pluginWatchPath);
            if (!Directory.Exists(this.pluginWatchPath.ToString()))
                Directory.CreateDirectory(this.pluginWatchPath.ToString());

            // Remove "cached" plugins
            var localPlugins = Directory.GetFiles(localPluginsPath);
            foreach (var plugin in localPlugins)
                File.Delete(plugin);

            // Copy Plugins from watch directory
            var plugins = Directory.GetFiles(this.pluginWatchPath);
            foreach (var plugin in plugins)
                File.Copy(plugin, Path.Combine(localPluginsPath, Path.GetFileName(plugin)), true);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Load Plugins
            var localPlugins = Directory.GetFiles(localPluginsPath);
            foreach (var plugin in localPlugins)
                AssemblyLoadContext.Default.LoadFromAssemblyPath(plugin);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Watch for new files & shutdown when found.
            //  Restart occurs on new call http call & new plugins will be copied
            var extensionsWatcher = new Web.PluginWatcher(appLifetime);
            extensionsWatcher.Start(pluginWatchPath);
        }
    }
}
