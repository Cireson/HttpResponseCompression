using System;
using System.IO;
using Cireson.HttpResponseCompression.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;

namespace Owin.WebSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseResponseCompressingMiddleware();

            app.UseWelcomePage("/");

            var root = Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName, "StaticFiles");
            var options = new StaticFileOptions
            {
                FileSystem = new PhysicalFileSystem(root)
            };
            app.UseStaticFiles(options);
        }
    }
}
