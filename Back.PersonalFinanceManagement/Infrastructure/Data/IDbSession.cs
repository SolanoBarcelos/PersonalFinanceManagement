using System.Data;

namespace Infrastructure.Data
{
    /// <summary>
    /// Define o contrato para uma sessão de banco de dados, centralizando a conexão e a transação atual.
    /// </summary>
    /// <remarks>
    /// Permite o desacoplamento entre a lógica de persistência e a gestão de transações, 
    /// garantindo que múltiplos repositórios operem sob o mesmo contexto.
    /// </remarks>
    public interface IDbSession : IDisposable
    {
        /// <summary>
        /// Obtém a conexão ativa com o provedor de banco de dados,NpgsqlConnection.
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Obtém ou define a transação.
        /// </summary>
        IDbTransaction? Transaction { get; set; }
    }
}