using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.Data;
using System.Windows;
//using System.Windows;
using VibrationDetectors.Interfaces;
using VibrationDetectors.Services;

using Microsoft.AspNetCore.Http;


namespace VibrationDetectors
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _grpcHost;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _grpcHost = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        options.ListenLocalhost(5000);   // HTTP/1.1 + gRPC-Web
                        options.ListenLocalhost(5001, o =>
                        {
                            o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
                        });
                    });

                    webBuilder.ConfigureServices(services =>
                    {
                        services.AddGrpc();
                    });

                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();

                        app.UseEndpoints(endpoints =>
                        {
                            //Lite osäker på denna rad TODO
                            endpoints.MapGrpcService<VDStatusHandlerService>();
                            


                            endpoints.MapGet("/", async context =>
                            {
                                await context.Response.WriteAsync(
                                    "This WPF app is hosting a gRPC server.");
                            });
                        });
                    });
                })
                .Build();

            _grpcHost.Start();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_grpcHost != null)
            {
                await _grpcHost.StopAsync();
                _grpcHost.Dispose();
            }

            base.OnExit(e);
        }

    }

}
