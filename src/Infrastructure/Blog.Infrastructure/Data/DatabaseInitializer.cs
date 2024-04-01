using AutoMapper;
using Blog.Application.Models.SiteSetting;
using Blog.Domain.Entites;
using Blog.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blog.Infrastructure.Data;

public class DatabaseInitializer(
	ApplicationContext context,
	IOptionsSnapshot<SiteSettings> siteSettings,
	UserManager<User> userManager,
	RoleManager<Role> roleManager,
	IMapper mapper)
{
    private static readonly string[] ValidateUsers =
    {
        RoleEnum.Administrator.ToString(),
        RoleEnum.Admin.ToString()
    };

    private readonly SiteSettings _siteSettings = siteSettings.Value;

    public void Initialize()
    {
        context.Database.Migrate();
    }

    public async Task SeedRoleDataAsync()
    {
        foreach (var role in Enum.GetValues(typeof(RoleEnum)))
        {
            try
            {
                var existRole = await roleManager.FindByNameAsync(role.ToString() ?? string.Empty);
                if (existRole != null) continue;
                await roleManager.CreateAsync(new Role()
                {
                    Name = role.ToString(),
                    NormalizedName = role.ToString()?.ToUpper()
                });
            }
            catch { return; }
        }
    }

    async Task SeedRoleDataAsync(string roleName)
    {
        try
        {
            var existRole = await roleManager.FindByNameAsync(roleName);
            if (existRole != null) return;
            await roleManager.CreateAsync(new Role()
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            });
        }
        catch { return; }
    }

    public async Task SeedUserDataAsync()
    {
        foreach (var user in _siteSettings.DefaultUsers)
        {
            try
            {
                var existUser = await userManager.FindByNameAsync(user.UserName);
                if (existUser != null) continue;
                var userModel = mapper.Map<User>(user);
                if (ValidateUsers.Contains(user.RoleName))
                {
                    userModel.EmailConfirmed = true;
                    userModel.PhoneNumberConfirmed = true;
                    userModel.IsConfirmed = true;
                }
                if (await userManager.CreateAsync(userModel, user.Password) != IdentityResult.Success) continue;
                await SeedRoleDataAsync(user.RoleName);
                await userManager.AddToRoleAsync(userModel, user.RoleName);
            }
            catch { return; }
        }
    }
}
