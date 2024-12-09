using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using System.Net;

namespace MiddlewareTests;

public class CustomMiddlewareTests
{
    [Fact]
    public async Task Should_Call_Home_Index_When_Route_Is_Empty()
    {
        //arrange
        var factory = new AspWebApplicationFactory<Middleware.Program>();

        using var client = factory.CreateClient();

        //act
        var result = await client.GetAsync("/");

        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await result.Content.ReadAsStringAsync();
        content.ShouldBe("Received request to Home/Index method"); 
    }

    [Fact]
    public async Task Should_Return_Response_By_Minimal_Api_Registration()
    {
        // arrange

        using var host = await new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
            webBuilder
                .UseTestServer()
                .ConfigureServices(services => services.AddRouting())
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(ep => ep.MapGet("/test", () => "Hello world !!!"));
                });
        })
        .StartAsync();

        // act
        var response = await host.GetTestClient().GetAsync("/test");

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseText = await response.Content.ReadAsStringAsync();
        responseText.ShouldBe("Hello world !!!"); 
    }

    [Fact]
    public async Task Should_Return_Custom_Header_When_Set_In_Middleware()
    {
        // arrange

        using var host = await new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
            webBuilder
                .UseTestServer()
                .ConfigureServices(services => services.AddRouting())
                .Configure(app =>
                {
                    app.UseRouting(); 
                    app.Use(async (context, next) => 
                    {
                        context.Response.Headers["custom_header"] = "custom content";
                        await next(context);
                    });
                    app.UseEndpoints(ep => ep.MapGet("/", () => "Hello world !!!"));
                });
        })
        .StartAsync();

        // act
        var response = await host.GetTestClient().GetAsync("/");

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var header = response.Headers.TryGetValues("custom_header", out var values);
        header.ShouldBeTrue(); 
        values.Count().ShouldBe(1);
        values.Single().ShouldBe("custom content"); 
    }

    [Fact]
    public async Task Should_Return_Static_Files_When_Set()
    {
        using var host = await new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
            webBuilder
                .UseTestServer()
                .UseWebRoot("/root")
                .ConfigureServices(services => services.AddRouting())
                .Configure(app =>
                {
                    app.UseRouting();
                    app.Use(async (context, next) =>
                    {
                        context.Response.Headers["custom_header"] = "custom content";
                        await next(context);
                    });
                    
                });
        })
        .StartAsync();

        // act
        var response = await host.GetTestClient().GetAsync("/test.min.css");

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBe("body{background-color: olive;}");
    }
}
