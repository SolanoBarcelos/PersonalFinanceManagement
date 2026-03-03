using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Infrastructure.Data
{
    /// <summary>
    /// Gerencia a sessão de conexão com o banco de dados PostgreSQL e o estado das transações.
    /// </summary>
    /// <remarks>
    /// Esta classe é registrada como Scoped, garantindo que a mesma conexão seja 
    /// compartilhada entre diferentes repositórios durante o ciclo de vida de uma única requisição. 
    /// </remarks>
    public sealed class DbSession : IDbSession
    {
        /// <summary>Instância da conexão ativa com o banco de dados.</summary>
        public IDbConnection Connection { get; }

        /// <summary>Transação ativa.</summary>
        public IDbTransaction? Transaction { get; set; }

        /// <summary>
        /// Inicializa uma nova conexão aberta com o banco de dados via Connection String.
        /// </summary>
        /// <param name="configuration">Interface de configuração para acesso ao appsettings.json.</param>
        public DbSession(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Connection = new NpgsqlConnection(connectionString);

            // A conexão é aberta imediatamente para garantir prontidão para os repositórios.
            Connection.Open();
        }

        /// <summary>
        /// Libera os recursos de transação e conexão de forma segura.
        /// </summary>
        /// <remarks>
        /// Chamado automaticamente pelo contêiner de DI ao final da requisição.
        /// </remarks>
        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
        }
    }
}