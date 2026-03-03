using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repositório de infraestrutura para persistência e manipulação de dados de pessoas utilizando Dapper.
    /// </summary>
    /// <remarks>
    /// Implementa o contrato <see cref="IPersonRepository"/>, garantindo que o acesso ao PostgreSQL 
    /// ocorra de forma otimizada e integrada ao contexto transacional via <see cref="IDbSession"/>.
    /// </remarks>
    public class PersonRepository : IPersonRepository
    {
        private readonly IDbSession _session;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="PersonRepository"/>.
        /// </summary>
        /// <param name="session">Sessão de banco de dados compartilhada da requisição.</param>
        public PersonRepository(IDbSession session)
        {
            _session = session;
        }

        /// <summary>
        /// Persiste uma nova pessoa no banco de dados.
        /// </summary>
        /// <param name="person">Entidade de domínio validada.</param>
        public async Task AddAsync(Person person)
        {
            const string sql = @"
            INSERT INTO Persons (Id, Name, Age) 
            VALUES (@Id, @Name, @Age)";

            await _session.Connection.ExecuteAsync(sql, person, _session.Transaction);
        }

        /// <summary>
        /// Atualiza os dados de uma pessoa existente.
        /// </summary>
        /// <param name="person">Entidade com os dados atualizados.</param>
        public async Task UpdateAsync(Person person)
        {
            const string sql = @"
            UPDATE Persons 
            SET Name = @Name, Age = @Age 
            WHERE Id = @Id";

            await _session.Connection.ExecuteAsync(sql, person, _session.Transaction);
        }

        /// <summary>
        /// Remove o registro de uma pessoa pelo seu id.
        /// </summary>
        /// <param name="id">GUID da pessoa.</param>
        public async Task DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM Persons WHERE Id = @Id";

            await _session.Connection.ExecuteAsync(sql, new { Id = id }, _session.Transaction);
        }

        /// <summary>
        /// Recupera uma pessoa específica pelo ID, incluindo sua idade para validações de negócio.
        /// </summary>
        /// <param name="id">GUID da pessoa.</param>
        /// <returns>A entidade <see cref="Person"/> ou nulo caso não exista.</returns>
        public async Task<Person?> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT Id, Name, Age FROM Persons WHERE Id = @Id";

            return await _session.Connection.QuerySingleOrDefaultAsync<Person>(
                sql, new { Id = id }, _session.Transaction);
        }

        /// <summary>
        /// Lista todas as pessoas cadastradas.
        /// </summary>
        /// <returns>Coleção de entidades.</returns>
        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            const string sql = "SELECT Id, Name, Age FROM Persons";

            return await _session.Connection.QueryAsync<Person>(
                sql, null, _session.Transaction);
        }
    }
}