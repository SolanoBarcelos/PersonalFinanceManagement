using Application.Interfaces;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Reports;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IDbSession, DbSession>();

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddScoped<IReportQuery, ReportQuery>();

            return services;
        }
    }
}