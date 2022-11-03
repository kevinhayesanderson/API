namespace MiddlewareExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Use(async (context, next) => 
            {
                Console.WriteLine($"Logic before executing the next delegate in the use method");
                await next.Invoke();
                Console.WriteLine($"Logic after executing the next delegate in the use method");
            });

            app.Map("/usingmapbranch", builder =>
            {
                builder.Use(async (context, next) => {
                    Console.WriteLine("Map branch logic in the Use method before the next delegate");
                    await next.Invoke();
                    Console.WriteLine("Map branch logic in the Use method after the next delegate");
                });
                builder.Run(async context =>
                {
                    Console.WriteLine($"Map branch response to the client in the Run method");
                    await context.Response.WriteAsync("Hello from then map branch");
                });
            });

            app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), builder =>
            {
                builder.Run(async context =>
                {
                    await context.Response.WriteAsync("Hello from the MapWhen branch.");
                });
            });

            app.Run(async context => {
                Console.WriteLine($"Writing the response to the client in the Run method");
                await context.Response.WriteAsync("Hello from the middleware component");
            });

            app.MapControllers();

            app.Run();
        }
    }
}