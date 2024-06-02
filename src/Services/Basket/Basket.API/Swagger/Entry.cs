namespace Basket.API.Swagger;

/// <summary>
/// Точка входа для сваггера
/// </summary>
public static class Entry
{
    /// <summary>
    /// Добавить сваггер в службы приложения
    /// </summary>
    /// <param name="services">Службы приложения</param>
    /// <returns>Службы приложения со сваггером</returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services.AddEndpointsApiExplorer()
            .AddSwaggerGen();
}