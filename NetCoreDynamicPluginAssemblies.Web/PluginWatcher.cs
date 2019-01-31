using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreDynamicPluginAssemblies.Web
{
    public class PluginWatcher
    {
        private IApplicationLifetime ApplicationLifetime;

        public PluginWatcher(IApplicationLifetime appLifetime)
        {
            ApplicationLifetime = appLifetime;
        }

        public void Start(string extensionsWatchPath)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher()
            {
                Path = extensionsWatchPath,
                Filter = "*.dll" // only watch dll files
            };

            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Add event handlers.
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnChanged;

            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            ApplicationLifetime.StopApplication();
        }
    }
}
