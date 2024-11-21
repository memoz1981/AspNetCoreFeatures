namespace _1_Middleware.Extensions; 

public static class MiddlewareExtensions
{
    public static IApplicationBuilder AddDefaultPipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days.
            // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        return app; 
    }

    public static IApplicationBuilder AddWelcomePageMiddleware(this IApplicationBuilder app, string path = null)
        => app.UseWelcomePage(path); 



}
