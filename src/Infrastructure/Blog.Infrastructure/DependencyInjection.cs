using System.Text;
using Blog.Application.Contracts.Persistence.Common;
using Blog.Application.Contracts.Services;
using Blog.Application.Models.SiteSetting;
using Blog.Domain.Entites;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Implementation.Repositories.Common;
using Blog.Infrastructure.Implementation.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureServices(
		this IServiceCollection services)
	{
		var siteSettings = services.BuildServiceProvider()
			.GetRequiredService<IOptionsSnapshot<SiteSettings>>().Value;

		services.AddDbContext<ApplicationContext>(options =>
		{
			options.UseSqlServer(siteSettings.ConnectionStrings.SqlServer);
		});

		services.AddIdentity<User, Role>()
			.AddEntityFrameworkStores<ApplicationContext>()
			.AddDefaultTokenProviders();

		services.Configure<IdentityOptions>(options =>
		{
			options.Password.RequireDigit = true;
			options.Password.RequireLowercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequiredLength = 6;
			options.Password.RequiredUniqueChars = 0;
		});

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			options.SaveToken = true;
			options.RequireHttpsMetadata = false;
			options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ClockSkew = TimeSpan.Zero,
				ValidAudience = siteSettings.Jwt.ValidAudience,
				ValidIssuer = siteSettings.Jwt.ValidIssuer,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(siteSettings.Jwt.SecurityKey))
			};
		});

		services.AddScoped(typeof(IQueryRepository<,>), typeof(QueryRepository<,>));
		services.AddScoped(typeof(ICommandRepository<,>), typeof(Repository<,>));

		services.AddScoped<DatabaseInitializer>();

		services.AddScoped<ITokenService, TokenService>();

		return services;
	}
}