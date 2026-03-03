using Domain.Entities;

namespace Domain.Repositories
{
    /// <summary>
    /// Interface de repositório para a entidade <see cref="Person"/>.
    /// </summary>
    /// <remarks>
    /// Define o contrato das operações de persistência necessárias para o ciclo de vida de uma pessoa.
    /// A implementação concreta deve garantir a implemetação da Interface.
    /// Seguindo o Princípio de Inversão de Dependência (DIP), esta interface define as operações 
    /// necessárias para a camada de domínio, enquanto a implementação real reside na camada de 
    /// Infrastructure. A camada de Domínio não sabe comonão sabe como os dados são salvos (PostgreSQL, Dapper), apenas o que precisa ser feito
    /// </remarks>
    public interface IPersonRepository
    {
        /// <summary>
        /// Registra uma nova pessoa no repositório.
        /// </summary>
        /// <param name="person">A entidade person validada pelo domínio.</param>
        /// <returns>Uma task que representa a operação assíncrona.</returns>
        Task AddAsync(Person person);

        /// <summary>
        /// Atualiza os dados de uma pessoa já existente.
        /// </summary>
        /// <param name="person">A entidade com os dados atualizados.</param>
        /// <returns>Uma task que representa a operação assíncrona.</returns>
        Task UpdateAsync(Person person);

        /// <summary>
        /// Remove uma pessoa do repositório de forma definitiva.
        /// </summary>
        /// <remarks>
        /// Nota: A deleção de transações vinculadas deve ser orquestrada pela camada de Application 
        /// antes de invocar este método para manter a integridade.
        /// </remarks>
        /// <param name="id">Identificador único (GUID) da pessoa.</param>
        /// <returns>Uma task que representa a operação assíncrona.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Busca uma pessoa específica pelo seu identificador.
        /// </summary>
        /// <param name="id">GUID da pessoa.</param>
        /// <returns>A entidade encontrada ou nulo caso não exista.</returns>
        Task<Person?> GetByIdAsync(Guid id);

        /// <summary>
        /// Recupera a listagem completa de pessoas cadastradas.
        /// </summary>
        /// <returns>Uma coleção de entidades <see cref="Person"/>.</returns>
        Task<IEnumerable<Person>> GetAllAsync();
    }
}