using System.Reflection;

namespace AutoGestion.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoriesAuto(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Buscar todas las clases concretas que terminen en "Repository"
            var repositoryTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
                .ToList();

            foreach (var implementationType in repositoryTypes)
            {
                // Buscar la interfaz coincidente (ej. ClientRepository -> IClientRepository)
                var interfaceType = implementationType.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{implementationType.Name}");

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, implementationType);
                }
                else
                {
                    // Si no tiene interfaz con 'I', se registra a sí misma
                    services.AddScoped(implementationType);
                }
            }

            return services;
        }
    }
}
