namespace Application.DTOs.Reports
{
    /// <summary>
    /// Representa uma linha no relatório por Pessoa ou Categoria
    /// </summary>
    /// <param name="NameOrDescription">Nome da Pessoa ou Descrição da Categoria agrupada.</param>
    /// <param name="TotalIncome">Soma total de todas as receitas (Income).</param>
    /// <param name="TotalExpense">Soma total de todas as despesas (Expense).</param>
    /// <remarks>
    /// Este Record utiliza uma propriedade calculada para o Balanço, garantindo consistência 
    /// na regra de negócio (Receita - Despesa) em toda a aplicação.
    /// </remarks>
    public record ReportItemDto(
        string NameOrDescription,
        decimal TotalIncome,
        decimal TotalExpense,
        decimal Balance
    );
}