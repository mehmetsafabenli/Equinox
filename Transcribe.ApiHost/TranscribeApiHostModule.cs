using Amazon.S3;
using Amazon.TranscribeService;
using Eqn.AspNetCore.Eqn;
using Eqn.AspNetCore.SeriLog.AspNetCore.Serilog;
using Eqn.AspNetCore.SeriLog.Builder;
using Eqn.Autofac;
using Eqn.Core;
using Eqn.Core.Microsoft.Extensions.DependencyInjection;
using Eqn.Core.Modularity;
using Eqn.Core.System;
using Eqn.Swashbuckle.Eqn.Swashbuckle;
using Eqn.Swashbuckle.Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Rewrite;
using Transcribe.Application;

namespace Transcribe.ApiHost;

[DependsOn(
    typeof(EqnSwashbuckleModule),
    typeof(TranscribeApplicationModule),
    typeof(EqnAutofacModule),
    typeof(EqnAspNetCoreSerilogModule)
)]
public class TranscribeApiHostModule : EqnModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClient();
        var config = context.Services.GetConfiguration();
        //add RequestTimeMiddleware middleware
        context.Services.AddControllersWithViews();
        
        AddAwsServices(context, config);
        ConfigureSwagger(context);
        ConfigureCors(context, config);
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.Trim().RemovePostFix("/"))
                            .ToArray()
                    )
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private void ConfigureSwagger(ServiceConfigurationContext context)
    {
        context.Services.AddEqnSwaggerGen(
            options => { options.HideEqnEndpoints(); }
        );
    }

    private void AddAwsServices(ServiceConfigurationContext context, IConfiguration config)
    {
        context.Services.AddDefaultAWSOptions(config.GetAWSOptions());
        context.Services.AddAWSService<IAmazonS3>();
        context.Services.AddAWSService<IAmazonTranscribeService>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection();
        app.UseEqnSerilogEnrichers();
        app.UseCors();
        app.UseMiddleware<RequestTimeMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API2 V1"); });
        app.UseRewriter(new RewriteOptions()
            .AddRedirect("^(|\\|\\s+)$", "/swagger"));
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}