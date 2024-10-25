using System.Text.Json.Serialization;
using Backend.Models.DbContext;
using Backend.Repositories;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=Flashcards.db"));

        services.AddScoped<SessionManager>();

        services.AddScoped<SessionsRepository>();
        services.AddScoped<UsersRepository>();
        services.AddScoped<DecksRepository>();
        services.AddScoped<FlashcardsRepository>();
        services.AddScoped<CollaboratorRepository>();
        services.AddScoped<ImportRepository>();
        services.AddScoped<LearningSessionRepository>();
        
        services.AddControllers();

        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();

            if (!dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
                dbContext.Seed().GetAwaiter().GetResult();
            }
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                dbContext.Seed().GetAwaiter().GetResult();
            }
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowSpecificOrigin");
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flashcard API V1");
            c.RoutePrefix = "swagger";
        });
    }
}