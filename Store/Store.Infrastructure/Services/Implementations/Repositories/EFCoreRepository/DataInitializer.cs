using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Infrastructure.Settings;

namespace Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;

public class DataInitializer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AdminSettings _adminSettings;
    
    public DataInitializer(IServiceProvider serviceProvider, 
        IOptions<AdminSettings> adminSettings)
    {
        _serviceProvider = serviceProvider;
        _adminSettings = adminSettings.Value;
    }

    public async Task InitializeAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<Role>>();
        await RoleInit(roleManager);
        await AdminInit(userManager);
    }

    private async Task RoleInit(RoleManager<Role> roleManager)
    {
        foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        {
            var roleName = role.ToString();
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new Role { Name = roleName });
        }
    }

    private async Task AdminInit(UserManager<User> userManager)
    {
        var adminUser = await userManager.FindByNameAsync(_adminSettings.UserName);
        if (adminUser == null)
        {
            adminUser = new User { UserName = _adminSettings.UserName };
            await userManager.CreateAsync(adminUser, _adminSettings.Password);
            await userManager.AddToRoleAsync(adminUser, UserRole.Admin.ToString());
        }
    }
}