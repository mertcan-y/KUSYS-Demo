using Autofac;
using Autofac.Extensions.DependencyInjection;
using KUSYS.Business.DependencyResolvers.Autofac;

namespace KUSYS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new AutofacBusinessModule());
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // responselardaki sunucu bilgisi kaldilir
                    webBuilder.ConfigureKestrel(
                        options => options.AddServerHeader = false);
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
