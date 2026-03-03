using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repositório de infraestrutura para persistência de transações financeiras via Dapper.
    /// </summary>
    /// <remarks>
    /// As tabelas auxiliares para os enums <see cref="Domain.Enums.TransactionType"/> e 
    /// <see cref="Domain.Enums.CategoryPurpose"/> estão no banco de dados para garantir integridade referencial.
    /// </remarks>
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDbSession _session;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="TransactionRepository"/>.
        /// </summary>
        /// <param name="session">Sessão de banco de dados compartilhada da requisição.</param>
        public TransactionRepository(IDbSession session)
        {
            _session = session;
        }

        /// <summary>
        /// Registra uma nova transação financeira vinculada a uma pessoa e categoria.
        /// </summary>
        /// <param name="transaction">Entidade de transação validada pelo domínio.</param>
        public async Task AddAsync(Transaction transaction)
        {
            const string sql = @"
            INSERT INTO Transactions (Id, Description, Amount, TypeId, CategoryId, PersonId) 
            VALUES (@Id, @Description, @Amount, @Type, @CategoryId, @PersonId)";

            // O Dapper mapeia automaticamente a propriedade Type da entidade para o parâmetro @Type (TypeId).
            await _session.Connection.ExecuteAsync(sql, transaction, _session.Transaction);
        }

        /// <summary>
        /// Recupera o histórico completo de transações registradas.
        /// </summary>
        /// <returns>Uma coleção de entidades <see cref="Transaction"/>.</returns>
        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            const string sql = @"
            SELECT Id, Description, Amount, TypeId AS Type, CategoryId, PersonId 
            FROM Transactions";

            return await _session.Connection.QueryAsync<Transaction>(
                sql, null, _session.Transaction);
        }

        /// <summary>
        /// Remove todas as transações associadas a uma pessoa específica.
        /// </summary>
        /// <remarks>
        /// Este método é utilizado para suportar a deleção em cascata controlada pela camada de Application.
        /// </remarks>
        /// <param name="personId">Identificador único da pessoa.</param>
        public async Task DeleteByPersonIdAsync(Guid personId)
        {
            const string sql = "DELETE FROM Transactions WHERE PersonId = @PersonId";

            await _session.Connection.ExecuteAsync(sql, new { PersonId = personId }, _session.Transaction);
        }
    }
}