using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestWithAspNet.Data;
using RestWithAspNet.Business;
using RestWithAspNet.Business.Implementations;
using RestWithAspNet.Repository;
using Serilog;
using System.Collections.Generic;
using RestWithAspNet.Repository.Generic;

namespace RestWithAspNet
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public string _connection => Configuration.GetConnectionString("MSSQLLocalDB");

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(_connection));

            services.AddApiVersioning(options => { options.ReportApiVersions = true; });

            services.AddControllers();              
            
            services.AddScoped<IPersonBusiness, PersonBusiness>();
            services.AddScoped<IBookBusiness, BookBusiness>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //MigrateDatabase(app);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void MigrateDatabase(IApplicationBuilder app)
        {
            try
            {
                using var db = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<MyDbContext>();

                var evolveConnection = db.Database.GetDbConnection();
                var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "db/migrations", "db/dataset" },
                    IsEraseDisabled = true,
                };
                evolve.Migrate();

            }
            catch (System.Exception ex)
            {
                Log.Error("Database migrations failed", ex);
                throw;
            }
        }
    }
}
