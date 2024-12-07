using Microsoft.AspNetCore.Hosting;
using Shouldly;
using System.Net;

namespace MiddlewareTests;

public sealed class CustomMiddlewareTests : IClassFixture<AspWebApplicationFactory>
{
    [Fact]
    public async Task Should_Call_Home_Index_When_Route_Is_Empty()
    {
        //arrange
        var factory = new AspWebApplicationFactory();

        using var client = factory.CreateClient();

        //act
        var result = await client.GetAsync("/");

        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await result.Content.ReadAsStringAsync();
        content.ShouldBe("Received request to Home/Index method"); 
    }

    [Fact]
    public async Task Should_Return_Custom_Header_When_Set_In_Middleware()
    {
        //arrange
        var factory = new AspWebApplicationFactory().WithWebHostBuilder(builder => 
        {
            builder.Configure(app =>
            {
                app.Use(async (context, next) =>
                {
                    await next; 
                }); 
            });
        }); 

        //act

        //assert
    }
}
