using Application.DTOs.Person;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    /// <summary>
    /// Serviço de aplicação responsável pelo gerenciamento de pessoas.
    /// </summary>
    /// <remarks>
    /// Esta camada orquestra a comunicação entre os repositórios e o domínio, 
    /// garantindo que as regras de negócio de <see cref="Person"/> sejam respeitadas.
    /// </remarks>
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ITransactionRepository _transactionRepository;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="PersonService"/>.
        /// </summary>
        /// <param name="personRepository">Repositório para persistência de pessoas.</param>
        /// <param name="transactionRepository">Repositório para gerenciamento de transações vinculadas.</param>
        public PersonService(IPersonRepository personRepository, ITransactionRepository transactionRepository)
        {
            _personRepository = personRepository;
            _transactionRepository = transactionRepository;
        }

        /// <summary>
        /// Cria uma nova pessoa no sistema após validação de domínio.
        /// </summary>
        /// <param name="dto">Objeto contendo nome e idade da pessoa.</param>
        /// <returns>Dados da pessoa criada, incluindo o id.</returns>
        /// <exception cref="ArgumentException">Lançada se os dados violarem as regras de <see cref="Person"/>.</exception>
        public async Task<PersonResponseDto> CreateAsync(CreatePersonDto dto)
        {
            var person = new Person(dto.Name, dto.Age);
            await _personRepository.AddAsync(person);

            return new PersonResponseDto(person.Id, person.Name, person.Age);
        }

        /// <summary>
        /// Atualiza os dados de uma pessoa existente.
        /// </summary>
        /// <param name="dto">Objeto contendo o ID e os novos dados da pessoa.</param>
        /// <returns>Dados da pessoa após a atualização.</returns>
        /// <exception cref="KeyNotFoundException">Lançada caso o ID informado não exista no banco.</exception>
        public async Task<PersonResponseDto> UpdateAsync(UpdatePersonDto dto)
        {
            var person = await _personRepository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException("Pessoa não encontrada.");

            person.Update(dto.Name, dto.Age);
            await _personRepository.UpdateAsync(person);

            return new PersonResponseDto(person.Id, person.Name, person.Age);
        }

        /// <summary>
        /// Remove uma pessoa e todas as suas transações vinculadas (Cascade Delete manual).
        /// </summary>
        /// <remarks>
        /// Opção por Cascade manual na aplicação para manter a regra de negócio visível 
        /// e independente de configurações específicas do SGBD. Seguindo o princípio da Clean Architecture de que a aplicação deve controlar as suas regras,
        /// </remarks>
        /// <param name="id">Identificador único da pessoa.</param>
        /// <exception cref="KeyNotFoundException">Lançada caso a pessoa não seja encontrada.</exception>
        public async Task DeleteAsync(Guid id)
        {
            var person = await _personRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Pessoa não encontrada.");

            await _transactionRepository.DeleteByPersonIdAsync(id);
            await _personRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Obtém a listagem completa de todas as pessoas cadastradas.
        /// </summary>
        /// <returns>Uma coleção de DTOs representando as pessoas.</returns>
        public async Task<IEnumerable<PersonResponseDto>> GetAllAsync()
        {
            var persons = await _personRepository.GetAllAsync();
            return persons.Select(p => new PersonResponseDto(p.Id, p.Name, p.Age));
        }

        /// <summary>
        /// Recupera uma pessoa específica pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador único da pessoa.</param>
        /// <returns>Dados da pessoa ou nulo se não encontrada.</returns>
        public async Task<PersonResponseDto?> GetByIdAsync(Guid id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null) return null;

            return new PersonResponseDto(person.Id, person.Name, person.Age);
        }
    }
}