namespace Application.DTOs.Reports
{
    /// <summary>
    /// Objeto de transferência de dados que representa uma linha do relatório.
    /// </summary>
    /// <param name="Items">Coleção de itens detalhados do relatório por Pessoa ou Categoria.</param>
    /// <remarks>
    /// Este Record expõe propriedades calculadas para totais gerais, facilitando a exibição no frontend.
    /// </remarks>
    public record ReportResultDto(
         IEnumerable<ReportItemDto> Items,
         decimal GrandTotalIncome,
         decimal GrandTotalExpense,
         decimal GrandTotalBalance
     );
}