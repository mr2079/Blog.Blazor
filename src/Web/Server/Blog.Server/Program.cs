
using Blog.Application;
using Blog.Infrastructure;
using Blog.Infrastructure.Data.Extensions;

namespace Blog.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            const string corsPolicyName = "Blazor";

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices();

            await builder.Services.InitializeDatabaseAsync();

            builder.Services.AddCors(config =>
            {
	            config.AddPolicy(corsPolicyName, options =>
	            {
		            var validOrigins = builder.Configuration
			            .GetSection("Cors:Valid").Get<List<string>>();

		            options.WithOrigins(validOrigins!.ToArray())
			            .AllowAnyMethod()
			            .AllowAnyHeader();
	            });
            });

			var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

			app.UseRouting();

			app.UseCors(corsPolicyName);

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

            await app.RunAsync();
        }
    }
}
