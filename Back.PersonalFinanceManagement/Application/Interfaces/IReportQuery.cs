using Application.DTOs.Reports;

namespace Application.Interfaces
{
    /// <summary>
    /// Interface de leitura otimizada para relatórios.
    /// </summary>
    /// <remarks>
    /// Esta interface implementa o padrão CQRS (Command Query Responsibility Segregation), 
    /// separando as responsabilidades de consulta (Read) das operações de escrita (Write).
    /// Como uma "Query", ela é projetada para performance, utilizando consultas SQL diretas 
    /// via Dapper para consolidar saldos de pessoas e categorias.
    /// </remarks>
    public interface IReportQuery
    {
        /// <summary>
        /// Executa uma consulta consolidada para obter os totais financeiros agrupados por pessoa.
        /// </summary>
        /// <returns>Um DTO contendo a lista de pessoas, seus totais de entrada, saída e saldo.</returns>
        Task<ReportResultDto> GetPersonTotalsAsync();

        /// <summary>
        /// Executa uma consulta consolidada para obter os totais financeiros agrupados por categoria.
        /// </summary>
        /// <remarks>
        /// Esta consulta agrupa as transações com base na finalidade da categoria (Income/Expense/Both).
        /// </remarks>
        /// <returns>Um DTO contendo o total por categoria.</returns>
        Task<ReportResultDto> GetCategoryTotalsAsync();
    }
}