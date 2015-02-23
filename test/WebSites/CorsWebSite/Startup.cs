// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Cors.Core;

namespace CorsWebSite
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            var configuration = app.GetTestConfiguration();

            app.UseServices(services =>
            {
                services.AddMvc(configuration);
                services.Configure<MvcOptions>(options =>
                {
                    options.AddXmlDataContractSerializerFormatter();
                });

                services.ConfigureCors(options =>
                {
                    // Simple read.
                    options.AddPolicy(
                        "AllowAnySimpleRequest",
                        (builder) =>
                        {
                            builder.AllowAnyOrigin()
                                   .AddMethods("GET")
                                   .AddMethods("POST")
                                   .AddMethods("HEAD");
                        });

                    // A post from a well known origin.
                    options.AddPolicy(
                        "WithCredentials",
                        (builder) =>
                        {
                            builder.AddOrigins("http://mobile.Shopping.com/")
                                   .AddMethods("POST")
                                   .AllowCredentials();
                        });

                    options.AddPolicy(
                        "SimplePreFlight",
                        (builder) =>
                        {
                            builder.AddOrigins("http://foo.com/")
                                   .AddHeaders("UserAgent");
                        });
                });
            });

            app.UseErrorReporter();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}