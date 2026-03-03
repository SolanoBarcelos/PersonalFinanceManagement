using Domain.Enums;

namespace Application.DTOs.Transaction
{
    /// <summary>
    /// Objeto de transferência de dados que representa o detalhamento de uma transação financeira realizada.
    /// </summary>
    /// <param name="Id">Identificador único (GUID) da transação no banco de dados.</param>
    /// <param name="Description">Descrição ou histórico da movimentação financeira.</param>
    /// <param name="Amount">Valor monetário da transação.</param>
    /// <param name="Type">Tipo da transação: Receita (1) ou Despesa (2).</param>
    /// <param name="CategoryId">Identificador da categoria à qual esta transação pertence.</param>
    /// <param name="PersonId">Identificador da pessoa que realizou ou é dona da transação.</param>
    /// <remarks>
    /// Este DTO é utilizado para padronizar o retorno das consultas de transações, 
    /// isolando a entidade de domínio e expondo apenas os dados necessários para o cliente.
    /// Uso de Record para garantir imutabilidade e comparação por valor, ideal para objetos de transferência de dados.
    /// </remarks>
    public record TransactionResponseDto(
        Guid Id,
        string Description,
        decimal Amount,
        TransactionType Type,
        Guid CategoryId,
        Guid PersonId);
}