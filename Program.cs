using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using FirmTracker_Server.Controllers;
using FirmTracker_Server.nHibernate.Products;
using FirmTracker_Server.nHibernate;

namespace FirmTracker_Server
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string appDirectory = Directory.GetCurrentDirectory();
            // Combine the application directory with the relative path to the config file
            string configFilePath = Path.Combine(appDirectory, "appsettings.json");
            string connectionString = "";
            // Check if the config file exists
            if (File.Exists(configFilePath))
            {
                var config = new ConfigurationBuilder()
                  .AddJsonFile(configFilePath)
                  .Build();


                var connectionstringsection = config.GetSection("AppSettings:ConnectionString");

                connectionString = connectionstringsection.Value;

                SessionFactory.Init(connectionString);
            }
            else
            {
                Console.WriteLine($"The configuration file '{configFilePath}' was not found.");
            }

            TestClass test = new TestClass();
            test.AddTestProduct();
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            var configSwagger = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

       
            var port = configSwagger.GetValue<int>("Port", 5075); 
            var port2 = configSwagger.GetValue<int>("Port", 7039);
            app.Urls.Add($"http://*:{port}");  
            app.Urls.Add($"https://*:{port2}");
         
            try
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "FirmTracker - TEST"); 
                    c.RoutePrefix = "swagger";
                });
                Console.WriteLine("uruchomiono swaggera");
                app.UseHttpsRedirection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nie uda³o siê uruchomiæ swaggera");
            }

            Console.WriteLine("raz dwa trzy");

            app.UseAuthorization();


            app.MapControllers();

            var configuration = new Configuration();


            app.Run();
        }
    }
}
