using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repositório de infraestrutura para persistência de categorias utilizando Dapper.
    /// </summary>
    /// <remarks>
    /// Esta classe implementa o padrão Repository para isolar o acesso ao banco de dados PostgreSQL.
    /// Utiliza o <see cref="IDbSession"/> para garantir que todas as operações ocorram dentro do mesmo contexto de conexão e transação.
    /// As Tabelas dos enums TransactionType e CategoryPurpose estão no DB.
    /// </remarks>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbSession _session;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="CategoryRepository"/>.
        /// </summary>
        /// <param name="session">Sessão de banco de dados compartilhada.</param>
        public CategoryRepository(IDbSession session)
        {
            _session = session;
        }

        /// <summary>
        /// Insere uma nova categoria na tabela de Categories.
        /// </summary>
        /// <param name="category">Entidade de categoria a ser persistida.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task AddAsync(Category category)
        {
            const string sql = @"
            INSERT INTO Categories (Id, Description, PurposeId) 
            VALUES (@Id, @Description, @Purpose)";

            // O Dapper mapeia automaticamente as propriedades da classe para os parâmetros @ no SQL.
            await _session.Connection.ExecuteAsync(sql, category, _session.Transaction);
        }

        /// <summary>
        /// Recupera todas as categorias cadastradas.
        /// </summary>
        /// <remarks>
        /// Realiza o mapeamento de 'PurposeId' no banco para a propriedade 'Purpose' (Enum) na entidade.
        /// </remarks>
        /// <returns>Uma coleção de entidades <see cref="Category"/>.</returns>
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            const string sql = "SELECT Id, Description, PurposeId AS Purpose FROM Categories";

            return await _session.Connection.QueryAsync<Category>(
                sql, null, _session.Transaction);
        }

        /// <summary>
        /// Busca uma categoria específica pelo seu id.
        /// </summary>
        /// <param name="id">GUID da categoria.</param>
        /// <returns>A entidade encontrada ou nulo caso não exista.</returns>
        public async Task<Category?> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT Id, Description, PurposeId AS Purpose FROM Categories WHERE Id = @Id";

            return await _session.Connection.QuerySingleOrDefaultAsync<Category>(
                sql, new { Id = id }, _session.Transaction);
        }
    }
}