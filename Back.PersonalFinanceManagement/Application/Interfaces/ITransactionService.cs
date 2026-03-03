using Application.DTOs.Transaction;

namespace Application.Interfaces
{
    /// <summary>
    /// Define o contrato para o serviço de transações.
    /// <remarks>
    /// Esta interface é fundamental para a aplicação do Princípio de Inversão de Dependência (DIP), 
    /// fazendo que os Controllers e Presenters da clean architecture dependam apenas de abstrações. 
    /// Através da Inversão de Controle (IoC), permite que as regras complexas de validação,
    /// como idade do usuário e validação categoria, sejam injetadase em 
    /// quem consome o serviço atravéz da injeção de dependencia (DI).
    /// </remarks>
    public interface ITransactionService
    {
        /// <summary>
        /// Fluxo de criação de uma transação, validando regras de domínio.
        /// </summary>
        /// <param name="dto">Dados de entrada para a nova transação.</param>
        /// <returns>Uma tarefa que retorna o DTO de resposta com os dados da transação validada.</returns>
        /// <exception cref="KeyNotFoundException">Lançada se os relacionamentos de Pessoa ou Categoria não existirem.</exception>
        /// <exception cref="InvalidOperationException">Lançada se houver violação de regra de negócio (ex: restrição de idade).</exception>
        Task<TransactionResponseDto> CreateAsync(CreateTransactionDto dto);

        /// <summary>
        /// Recupera o histórico completo de transações de forma assíncrona.
        /// </summary>
        /// <returns>Uma coleção de DTOs representando todas as transações.</returns>
        Task<IEnumerable<TransactionResponseDto>> GetAllAsync();
    }
}