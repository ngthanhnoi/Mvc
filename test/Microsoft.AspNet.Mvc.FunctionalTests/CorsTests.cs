// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Cors.Core;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.TestHost;
using Microsoft.Framework.DependencyInjection;
using Xunit;

namespace Microsoft.AspNet.Mvc.FunctionalTests
{
    public class CorsTests
    {
        private readonly IServiceProvider _provider = TestHelper.CreateServices(nameof(CorsWebSite));
        private readonly Action<IApplicationBuilder> _app = new CustomRouteWebSite.Startup().Configure;

        [Fact]
        public async Task ResourceWithSimpleRequestPolicy_Allows_SimpleRequests()
        {
            System.Diagnostics.Debugger.Launch();
            // Arrange
            var server = TestServer.Create(_provider, _app);
            var client = server.CreateClient();

            var requestBuilder = server
                .CreateRequest("http://localhost/Cors/GetProductInfo")
                .AddHeader(CorsConstants.Origin, "http://localhost/Cors/GetProductInfo");

            // Act
            var response = await requestBuilder.GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
        }

        //[Fact]
        //public async Task ResourceWithSimpleRequestPolicy_RejectsNonSimpleRequests()
        //{
        //}

        [Fact]
        public async Task Middleware_With_AllowRestricted()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Cross origin response");
                });
            }))
            {
                // Preflight request.
                var response = await server.CreateRequest("/")
                    .AddHeader(CorsConstants.Origin, "http://localhost:5001/sub")
                    .AddHeader(CorsConstants.AccessControlRequestMethod, "PUT")
                    .SendAsync(CorsConstants.PreflightHttpMethod);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                response = await server.CreateRequest("/")
                    .AddHeader(CorsConstants.Origin, "http://localhost:5001/")
                    .AddHeader(CorsConstants.AccessControlRequestMethod, "PUT")
                    .SendAsync(CorsConstants.PreflightHttpMethod);

                response.EnsureSuccessStatusCode();
                Assert.Equal(2, response.Headers.Count());
                Assert.Equal("http://localhost:5001/", response.Headers.GetValues(CorsConstants.AccessControlAllowOrigin).FirstOrDefault());
                Assert.Equal("PUT", response.Headers.GetValues(CorsConstants.AccessControlAllowMethods).FirstOrDefault());

                // Actual request.
                response = await server.CreateRequest("/")
                    .AddHeader(CorsConstants.Origin, "http://localhost:5001/")
                    .SendAsync("PUT");

                response.EnsureSuccessStatusCode();
                Assert.Equal(2, response.Headers.Count());
                Assert.Equal("Cross origin response", await response.Content.ReadAsStringAsync());
                Assert.Equal("http://localhost:5001/", response.Headers.GetValues(CorsConstants.AccessControlAllowOrigin).FirstOrDefault());
                Assert.Equal("AllowedHeader", response.Headers.GetValues(CorsConstants.AccessControlExposeHeaders).FirstOrDefault());
            }
        }
    }
}