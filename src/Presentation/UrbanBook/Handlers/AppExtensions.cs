namespace UrbanBook.Handlers
{
    public static class AppExtensions
    {
        public static void useHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ValidationHandlerMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();                 
        }
    }
}
