using Application.DTOs.Person;

namespace Application.Interfaces
{
    /// <summary>
    /// Define o contrato de serviço para o gerenciamento de pessoas.
    /// </summary>
    /// <remarks>
    /// Esta interface implementa o Princípio de Inversão de Dependência (DIP), permitindo que a camada de API 
    /// dependa desta abstração em vez de uma implementação concreta. 
    /// Facilita a Inversão de Controle (IoC) via Injeção de Dependência (DI) e possibilita a criação de Mocks 
    /// para testes unitários isolados com xUnit e Moq.
    /// </remarks>
    public interface IPersonService
    {
        /// <summary>
        /// Criação de uma nova pessoa, validando as regras de domínio.
        /// </summary>
        /// <param name="dto">Dados de entrada para o cadastro da pessoa.</param>
        /// <returns>Uma task que retorna o DTO de resposta com os dados persistidos.</returns>
        Task<PersonResponseDto> CreateAsync(CreatePersonDto dto);

        /// <summary>
        /// Atualiza os dados de uma pessoa existente após validar sua existência.
        /// </summary>
        /// <param name="dto">Objeto com os novos dados e o id da pessoa.</param>
        /// <returns>O DTO de resposta atualizado.</returns>
        /// <exception cref="KeyNotFoundException">Lançada se o ID não for localizado.</exception>
        Task<PersonResponseDto> UpdateAsync(UpdatePersonDto dto);

        /// <summary>
        /// Remove uma pessoa e faz a deleção em cascata de suas transações vinculadas.
        /// </summary>
        /// <param name="id">Identificador único da pessoa.</param>
        /// <returns>Uma task que representa a operação de remoção.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Recupera a listagem completa de pessoas de forma assíncrona.
        /// </summary>
        /// <returns>Uma coleção de DTOs de resposta.</returns>
        Task<IEnumerable<PersonResponseDto>> GetAllAsync();

        /// <summary>
        /// Busca uma pessoa específica pelo seu id.
        /// </summary>
        /// <param name="id">O GUID da pessoa.</param>
        /// <returns>O DTO da pessoa ou nulo caso não seja encontrada.</returns>
        Task<PersonResponseDto?> GetByIdAsync(Guid id);
    }
}