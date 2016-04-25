using Cireson.HttpResponseCompression.Owin.Middleware;
using Owin;

namespace Cireson.HttpResponseCompression.Owin
{
    public static class AppBuilderExtensions
    {
        public static void UseResponseCompressingMiddleware(this IAppBuilder app)
        {
            app.Use<ResponseCompressingMiddleware>();
        }
    }
}
