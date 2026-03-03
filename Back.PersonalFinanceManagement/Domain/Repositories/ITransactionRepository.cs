using Domain.Entities;

namespace Domain.Repositories
{
    /// <summary>
    /// Interface para o repositório de persistência da entidade <see cref="Transaction"/>.
    /// </summary>
    /// <remarks>
    /// Define as operações necessárias para transações. 
    /// Esta abstração isola as regras de negócios das implementações de acesso a dados.
    /// Seguindo o Princípio de Inversão de Dependência (DIP), esta interface define as operações 
    /// necessárias para a camada de domínio, enquanto a implementação real está na camada de 
    /// Infrastructure. A camada de Domínio não sabe como os dados são salvos (PostgreSQL, Dapper), apenas o que precisa ser feito
    /// </remarks>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Registra uma nova transação financeira de forma assíncrona.
        /// </summary>
        /// <param name="transaction">A entidade de transação validada.</param>
        /// <returns>Uma task que representa a operação assíncrona.</returns>
        Task AddAsync(Transaction transaction);

        /// <summary>
        /// Recupera a listagem completa de todas as transações cadastradas no sistema.
        /// </summary>
        /// <returns>Uma coleção de entidades <see cref="Transaction"/>.</returns>
        Task<IEnumerable<Transaction>> GetAllAsync();

        /// <summary>
        /// Remove todas as transações vinculadas a uma pessoa específica.
        /// </summary>
        /// <remarks>
        /// A camada de Aplication deve orquestrar a deleção em cascata.
        /// </remarks>
        /// <param name="personId">O identificador único da pessoa.</param>
        /// <returns>Uma tarefa que representa a operação de remoção em lote.</returns>
        Task DeleteByPersonIdAsync(Guid personId);
    }
}