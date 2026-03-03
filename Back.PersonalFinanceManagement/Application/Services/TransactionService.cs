using Application.DTOs.Transaction;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    /// <summary>
    /// Serviço de aplicação responsável pela gestão do ciclo de vida de transações.
    /// </summary>
    /// <remarks>
    /// Esta classe atua como um orquestrador, mediando a comunicação entre a camada de API e o Domínio. 
    /// Ela garante que as dependências necessárias (Pessoa e Categoria) sejam validadas antes da 
    /// instanciação da entidade <see cref="Transaction"/>, onde as regras de negócio são efetivamente aplicadas.
    /// </remarks>
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;
        private readonly IPersonRepository _people;
        private readonly ICategoryRepository _categories;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="TransactionService"/> via injeção de dependência.
        /// </summary>
        /// <param name="transactionRepository">Interface de persistência para transações.</param>
        /// <param name="personRepository">Interface de consulta para dados de pessoas.</param>
        /// <param name="categoryRepository">Interface de consulta para categorias.</param>
        public TransactionService(
            ITransactionRepository transactionRepository,
            IPersonRepository personRepository,
            ICategoryRepository categoryRepository)
        {
            _repository = transactionRepository;
            _people = personRepository;
            _categories = categoryRepository;
        }

        /// <summary>
        /// Cria uma nova transação financeira após validar a existência de seus relacionamentos.
        /// </summary>
        /// <remarks>
        /// O fluxo de validação segue a hierarquia:
        /// 1. Existência física (Infraestrutura): Verifica se PersonId e CategoryId existem.
        /// 2. Integridade de Dados (Value Object): O <see cref="DescriptionVO"/> valida o texto da descrição.
        /// 3. Regras de Negócio (Domínio): A entidade <see cref="Transaction"/> valida compatibilidade de tipo e idade.
        /// </remarks>
        /// <param name="dto">Objeto de transferência com os dados para a nova transação.</param>
        /// <returns>Um <see cref="TransactionResponseDto"/> contendo os dados da transação persistida.</returns>
        /// <exception cref="KeyNotFoundException">Lançada caso a Pessoa ou Categoria não sejam encontradas.</exception>
        public async Task<TransactionResponseDto> CreateAsync(CreateTransactionDto dto)
        {
            var person = await _people.GetByIdAsync(dto.PersonId)
                ?? throw new KeyNotFoundException("Pessoa não encontrada.");

            var category = await _categories.GetByIdAsync(dto.CategoryId)
                ?? throw new KeyNotFoundException("Categoria não encontrada.");

            // A atribuição de 'dto.Description' à entidade dispara automaticamente 
            // a validação do Objeto de Valor (DescriptionVO) via propriedade pública.
            var transaction = new Transaction(
                dto.Description,
                dto.Amount,
                dto.Type,
                category.Id,
                category.Purpose,
                person.Id,
                person.Age);

            await _repository.AddAsync(transaction);

            return new TransactionResponseDto(
                transaction.Id,
                transaction.Description, // Retorna a string pura processada pelo domínio.
                transaction.Amount,
                transaction.Type,
                transaction.CategoryId,
                transaction.PersonId);
        }

        /// <summary>
        /// Recupera a listagem consolidada de todas as transações financeiras.
        /// </summary>
        /// <returns>Uma coleção de DTOs representando o estado atual das transações.</returns>
        public async Task<IEnumerable<TransactionResponseDto>> GetAllAsync()
        {
            var transactions = await _repository.GetAllAsync();

            // Mapeamento direto: a propriedade 'Description' da entidade já encapsula o valor do VO.
            return transactions.Select(t => new TransactionResponseDto(
                t.Id,
                t.Description,
                t.Amount,
                t.Type,
                t.CategoryId,
                t.PersonId));
        }
    }
}