namespace Discount.Grpc.Data
{
    public static class Extension
    {

        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<DiscountContext>();
            context.Database.MigrateAsync();

            return app;
        }
    }
}
