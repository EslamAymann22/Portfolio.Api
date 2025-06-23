using Microsoft.EntityFrameworkCore;
using Portfolio.Core;
using Portfolio.Data.Data;

namespace Portfolio.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<PortfolioDbContext>(options =>
                //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
                options.UseSqlServer(builder.Configuration.GetConnectionString("PublicConnection")));

            builder.Services.AddCoreDependencies();
            builder.Services.AddHttpContextAccessor();


            var app = builder.Build();

            #region AutoDatabaseUpdate 
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                var DbContext = Services.GetRequiredService<PortfolioDbContext>();
                await DbContext.Database.MigrateAsync();

            }
            catch (Exception ex)
            {

                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "Error During Update database in Program\n");

            }
            #endregion

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
