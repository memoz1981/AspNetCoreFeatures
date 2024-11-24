namespace _1_Middleware; 

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddLogging(opt => opt.AddConsole()); 

        var app = builder.Build();

        // 1 - Exception handling
        // 1a - On development will use built in DeveloperExceptionPageMiddlewareImpl 
        // 1b - On non-dev environment, will use ExceptionHandlerMiddlewareImpl with custom implementation
        if (!app.Environment.IsDevelopment())
            app.UseExceptionHandler("/home/error");
        else
            app.UseDeveloperExceptionPage();

        // 1c - will return error page when status code is 4xx or 5xx
        // (for example if a resource is not found)
        app.UseStatusCodePagesWithReExecute("home/error"); 

        // 2 - welcome page pipeline - will short circuit any middleware after it
        // for the path specified - if path is not specified -> will short circuit all requests
        // invokes WelcomePageMiddleware when the path is matching with specified path
        app.UseWelcomePage("/welcomepage");

        // 3 - inline middleware to log the request path and response status code
        var logger = app.Logger;
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.HasValue)
                logger.Log(LogLevel.Information, $"Request path: {context.Request.Path.Value}");

            await next(context);

            logger.Log(LogLevel.Information,
                    $"Response started, status code: {context.Response.StatusCode}");
        });

        // 4 - middleware to map static files - built in StaticFileMiddleware (provider access to wwwroot folder for 
        // certain file types (css, jss, img etc.)
        app.UseStaticFiles();

        // 5 - middleware to map to endpoints
        // this will ensure that endpoints are mapped. uses EndpointRoutingMiddleware. 
        // will populate endpoint of HttpContext
        app.UseRouting();

        // 6 - Custom middleware to log all mapped routes - will run on each request
        app.Use(async (context, next) =>
        {
            LogAllRoutes(app, logger);
            await next(context);
        });

        // 7a -> Minimal endpoint mapping - map specific endpoint
        app.MapGroup("test")
            .MapGet("get", () => Results.Ok("request accepted..."))
            .WithName("Get"); 

        // 7b - middleware to provide re-direction with route. Re-direction is done by browser not the app...
        app.MapGroup("test")
            .MapGet("redirect", () => Results.Redirect(url: "/test/get"));

        // 7c - middleware to provide re-direction with name. Re-direction is done by browser not the app...
        // the only difference from above is this is by name, above is by route... 
        app.MapGroup("test")
            .MapGet("redirect2", () => Results.RedirectToRoute(routeName: "get"));

        // 7d - custom middleware to throw an exception for /exception path to test exception handling middleware
        app.MapGet("/exception", () => ThrowException()); 

        // 7e - middleware to map controllers based on attribute routing - this provider flexibility for custom routes
        app.MapControllers();

        // 7f - this will enable conventional routing -> actions will be mapped by controller/action name - limited functionality
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // 7g - this is not recommended, but an alternative to combine all routes. 
        // rather an old setup
#pragma warning disable ASP0014 // Suggest using top level route registrations
        app.UseEndpoints(routes =>
        {
            routes.MapControllers();
            routes.MapControllerRoute(name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
            routes.MapGet("test2", () => Results.Ok("request accepted...")); 
        });
#pragma warning restore ASP0014 // Suggest using top level route registrations

        // 8 - This would run the host
        app.Run();

        // 9 - This middleware will never be entered -> just for demonstration
        // if used above, will short circuit the pipeline. 
        app.Run(async (context) => await context.Response.WriteAsync("Hello world"));
    }

    /// <summary>
    /// Private method to throw an exception
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private static void ThrowException()
    {
        throw new NotImplementedException("This is a test exception!");
    }

    /// <summary>
    /// Method to log all routes
    /// </summary>
    /// <param name="app"></param>
    /// <param name="logger"></param>
    static void LogAllRoutes(WebApplication app, ILogger logger)
    {
        // Access the EndpointDataSource from the DI container
        var endpointDataSources = app.Services.GetRequiredService<IEnumerable<EndpointDataSource>>();

        logger.Log(LogLevel.Information, "Registered Routes:");
        foreach (var source in endpointDataSources)
        {
            foreach (var endpoint in source.Endpoints)
            {
                if (endpoint is RouteEndpoint routeEndpoint)
                {
                    logger.Log(LogLevel.Information, routeEndpoint.DisplayName);
                }
            }
        }
    }
}
