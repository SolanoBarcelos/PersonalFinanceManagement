using Domain.Entities;

namespace Domain.Repositories
{
    /// <summary>
    /// Contrato para o repositório de persistência da entidade <see cref="Category"/>.
    /// </summary>
    /// <remarks>
    /// Seguindo o Princípio de Inversão de Dependência (DIP), esta interface define as operações 
    /// necessárias para a camada de domínio, enquanto a implementação real está na camada de 
    /// Infrastructure. A camada de Domínio não sabe como sabe como os dados são salvos (PostgreSQL, Dapper), apenas o que precisa ser feito.
    /// </remarks>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Persiste uma nova categoria no banco de dados de forma assíncrona.
        /// </summary>
        /// <param name="category">A entidade de categoria a ser adicionada.</param>
        /// <returns>Uma task que representa a operação assíncrona.</returns>
        Task AddAsync(Category category);

        /// <summary>
        /// Recupera a listagem completa de categorias.
        /// </summary>
        /// <returns>Uma coleção de entidades <see cref="Category"/>.</returns>
        Task<IEnumerable<Category>> GetAllAsync();

        /// <summary>
        /// Busca uma categoria específica pelo seu id (GUID).
        /// </summary>
        /// <param name="id">O identificador da categoria.</param>
        /// <returns>A entidade encontrada ou nulo caso não exista.</returns>
        Task<Category?> GetByIdAsync(Guid id);
    }
}