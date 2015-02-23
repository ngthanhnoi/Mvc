// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;

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
                    options.AddPolicy(
                        "AllowAnySimpleRequest",
                        (builder) =>
                        {
                            builder.AddOrigins("*")
                                   .AddMethods("GET")
                                   .AddMethods("POST")
                                   .AddMethods("HEAD");
                        });

                    options.AddPolicy(
                        "PreFlight",
                        (builder) =>
                        {
                            builder.AddOrigins("*");
                        });

                    options.AddPolicy("WithCredentials", (builder) => { builder.AddOrigins("http://foo.com/"); });
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