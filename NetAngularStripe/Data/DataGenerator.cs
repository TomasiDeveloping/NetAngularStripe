using Microsoft.EntityFrameworkCore;

namespace NetAngularStripe.Data;

/// <summary>
///     Class responsible for initializing database data.
/// </summary>
public class DataGenerator
{
    /// <summary>
    ///     Initializes the database with required data.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public static void Initialize(IServiceProvider serviceProvider)
    {
        // Create a new instance of the application context
        using var context = new ApplicationContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>());

        // Ensure that the database is created
        context.Database.EnsureCreated();
    }
}