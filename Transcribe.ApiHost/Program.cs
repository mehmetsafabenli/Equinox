using Eqn.AspNetCore.Asp.Builder;
using Eqn.AspNetCore.Extensions.DependencyInjection;
using Eqn.Autofac.Extensions.Hosting;
using Equinox.Shared;
using Equinox.Shared.Gateway;
using Serilog;

namespace Transcribe.ApiHost;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var assemblyName = typeof(Program).Assembly.GetName().Name;

        SerilogConfigurationHelper.Configure(assemblyName);

        try
        {
            Log.Information($"Starting {assemblyName}.");

            var builder = WebApplication.CreateBuilder(args);
            builder.Host
                .UseAutofac()
                .UseSerilog();
            await builder.AddApplicationAsync<TranscribeApiHostModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{assemblyName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}