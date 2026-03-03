using Domain.Enums;

namespace Application.DTOs.Transaction
{
    /// <summary>
    /// Objeto de transferência de dados para a criação de uma nova transação.
    /// </summary>
    /// <param name="Description">Descrição detalhada da transação (ex: "Aluguel", "Salário").</param>
    /// <param name="Amount">Valor da transação. Deve ser um valor positivo.</param>
    /// <param name="Type">Define se a transação é uma Receita (1) ou Despesa (2).</param>
    /// <param name="CategoryId">Id (GUID) da categoria associada.</param>
    /// <param name="PersonId"> Id(GUID) da pessoa responsável pela transação.</param>
    /// <remarks>
    /// Records são utilizados aqui por serem objetos de dados imutáveis, facilitando a concorrência 
    /// e garantindo que os dados não sejam alterados após a recepção pela Controller. 
    /// A comparação e feita pelo valor e nao pela referência, o que é ideal para DTOs.
    /// </remarks>
    public record CreateTransactionDto(
        string Description,
        decimal Amount,
        TransactionType Type,
        Guid CategoryId,
        Guid PersonId);
}