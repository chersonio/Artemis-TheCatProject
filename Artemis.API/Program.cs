using Artemis.API.AutoMapper.Profiles;
using Artemis.API.Helpers;
using Artemis.API.Services;
using Artemis.API.Services.Interfaces;
using Artemis.Data;
using Artemis.Data.Repositories;
using Artemis.Data.Repositories.Interfaces;
using Artemis.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Artemis.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });

            // Database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddTransient<DatabaseMigrationService>();

            // Repository
            builder.Services.AddScoped<ICatRepository, CatRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // STAVROS = see which works
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Services
            builder.Services.AddHttpClient<ICatService, CatService>();
            builder.Services.AddScoped<ICatService, CatService>();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var dbMigrationService = builder.Services.BuildServiceProvider().GetService<DatabaseMigrationService>();
            Task.Run(() => dbMigrationService.MigrateAsync());

            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Artemis API V1");
                    c.RoutePrefix = "swagger"; // This will serve Swagger UI at /swagger
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
