using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BoxProtocol.Interfaces;
using Grpc.Core;
using MagicOnion;
using MagicOnion.Hosting;
using MagicOnion.HttpGateway.Swagger;
using MagicOnion.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BoxServerCore
{
    internal class Program
    {
        private static async Task Main(string[] args)

        {
            //un services
            var magicOnionHost = MagicOnionHost.CreateDefaultBuilder()
                .UseMagicOnion(
                    new MagicOnionOptions(true),
                    //new ServerPort("127.0.0.1", 12345, ServerCredentials.Insecure)
                    new ServerPort("0.0.0.0", 12345, ServerCredentials.Insecure)
                )
                .UseConsoleLifetime()
                .Build();

            // NuGet: Microsoft.AspNetCore.Server.Kestrel
            var webHost = new WebHostBuilder()
                .ConfigureServices(
                    collection =>
                    {
                        collection.AddSingleton(
                            magicOnionHost.Services.GetService<MagicOnionHostedServiceDefinition>().ServiceDefinition
                        );
                        // add the following codes
                        collection.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
                    }
                )
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:9998")
                .Build();
            //webHost.Run();
            // Run and wait both.
            await Task.WhenAll(webHost.RunAsync(), magicOnionHost.RunAsync());
        }
    }

    // WebAPI Startup configuration.
    public class Startup
    {
        // Inject MagicOnionServiceDefinition from DIl
        public void Configure(IApplicationBuilder app, MagicOnionServiceDefinition magicOnion)
        {
            // Optional:Add Summary to Swagger
            var xmlName = "Sandbox.NetCoreServer.xml";
            var xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), xmlName);
            app.UseStaticFiles();
            // HttpGateway requires two middlewares.
            // One is SwaggerView(MagicOnionSwaggerMiddleware)
            // One is Http1-JSON to gRPC-MagicOnion gateway(MagicOnionHttpGateway)
            app.UseMagicOnionSwagger(
                magicOnion.MethodHandlers,
                new SwaggerOptions("MagicOnion.Server", "Swagger Integration Test", "/")
                {
                    XmlDocumentPath = xmlPath
                }
            );
            app.UseMagicOnionHttpGateway(
                magicOnion.MethodHandlers,
                new Channel("0.0.0.0:12345", ChannelCredentials.Insecure)
            );
        }
    }
}