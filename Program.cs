/*
 * This file is part of FirmTracker - Server.
 *
 * FirmTracker - Server is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * FirmTracker - Server is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with FirmTracker - Server. If not, see <https://www.gnu.org/licenses/>.
 */

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using FirmTracker_Server.Controllers;
using FirmTracker_Server.nHibernate.Products;
using FirmTracker_Server.nHibernate;
using FirmTracker_Server.Utilities.Converters;
using FirmTracker_Server.Utilities.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using FirmTracker_Server.Entities;
using FirmTracker_Server.Middleware;
using FirmTracker_Server.Services;
using System.Reflection;
using FirmTracker_Server.Mappings;
using NuGet.Packaging;



namespace FirmTracker_Server
{
    internal static class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string appDirectory = Directory.GetCurrentDirectory();        
            string configFilePath = Path.Combine(appDirectory, "appsettings.json");
            string connectionString = "";
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
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
            builder.Services.ConfigureAutoMapper();
            builder.Services.ConfigureServiceInjection();
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                });
                ;
            builder.ConfigureAuthentication();
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<SwaggerDateTimeSchemaFilter>();
            });



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
            app.UseHttpsRedirection();

            app.UseCors("AllowSpecificOrigin");


            app.UseAuthentication();
            app.UseAuthorization();

      
            app.MapControllers();

            var configuration = new Configuration();


            app.Run();
        }
        private static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            var authenticationSettings = new Authentication.AuthenticationSettings();
            builder.Configuration.GetSection("TokenConfig").Bind(authenticationSettings);
            builder.Services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtSecKey)),
                };
            });
            builder.Services.AddSingleton(authenticationSettings);
        }
        private static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc => {
                mc.AddProfile<LicenseMappingProfile>();
               // mc.AddProfile<PayLinkerMappingProfile>();
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
        private static void ConfigureServiceInjection(this IServiceCollection services)
        {         
            services.AddScoped<IUserService, UserService>();          
            services.AddScoped<ErrorHandling>();      
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
           // services.AddScoped<IWorkdayRepository, WorkdayRepository>();
            services.AddMvc();
        }

    }
}
