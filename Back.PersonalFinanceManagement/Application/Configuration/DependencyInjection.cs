using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITransactionService, TransactionService>();

            return services;
        }
    }
}
