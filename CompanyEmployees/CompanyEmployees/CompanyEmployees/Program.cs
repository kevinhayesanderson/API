using CompanyEmployees.Extensions;
using NLog;

namespace CompanyEmployees
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            LogManager.LoadConfiguration(Path.Combine(Directory.GetCurrentDirectory(),"nlog.config"));

            //to give or restrict access rights to applications from different domains
            builder.Services.ConfigureCors();

            //IIS integration which will help with deployment to IIS
            builder.Services.ConfigureIISIntegration();

            builder.Services.ConfigureLoggerService();

            builder.Services.AddControllers();//registers only the controllers in IServiceCollection

            var app = builder.Build();//type WebApplication, it implements
            //IHost -> use to start and stop the host
            //IApplicationBuilder -> use to build the middleware pipeline
            //IEndpointRouteBuilder -> use to add endpoints to our app
            
            if(app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();// adds the Strict-Transport-Security header.

            //for the redirection from HTTP to HTTPS
            app.UseHttpsRedirection();

            //enables using static files for the request. if not set. wwwroot folder is default
            app.UseStaticFiles();

            //will forward proxy headers to the current request.
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
            });

            //adding CORS configuration to the application pipeline
            app.UseCors("CorsPolicy");

            //adds authorization middleware to IApplicationBuilder
            app.UseAuthorization();

            //adds the endpoints from controller actions to the IEndpointRouteBuilder
            app.MapControllers();

            //runs the application and block the calling thread until the host shutdown
            app.Run();
        }
    }
}