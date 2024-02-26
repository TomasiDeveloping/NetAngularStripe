using Microsoft.EntityFrameworkCore;

namespace NetAngularStripe.Data;

public class DataGenerator
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>());

        context.Database.EnsureCreated();
    }
}