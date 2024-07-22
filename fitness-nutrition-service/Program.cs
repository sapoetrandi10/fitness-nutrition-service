using fitness_db.Data;
using Microsoft.EntityFrameworkCore;

namespace fitness_nutrition_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<FitnessContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            bool useCors = builder.Configuration.GetValue<bool>("CorsSettings:UseCors");
            if (useCors)
            {
                string[] allowedOrigins = useCors
                    ? builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>()
                    : Array.Empty<string>();

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowConfiguredOrigins", policy =>
                    {
                        policy.WithOrigins(allowedOrigins)
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
                });
            }

            var app = builder.Build();

            if (useCors)
            {
                app.UseCors("AllowConfiguredOrigins");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
