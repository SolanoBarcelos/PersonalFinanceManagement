using Application.DTOs.Reports;
using Application.Interfaces;
using Dapper;
using Infrastructure.Data;

namespace Infrastructure.Reports
{
    /// <summary>
    /// Implementação de consultas de relatório utilizando SQL com Dapper para performance.
    /// </summary>
    /// <remarks>
    /// Esta classe segue o princípio de separação de responsabilidades de consulta (CQRS). 
    /// As agregações são realizadas diretamente no banco de dados para evitar tráfego desnecessário.
    /// Obs.:
    ///1 - As queries abaixo poderiam ser feitas usando LINQ, mas neste caso, utilizando Dapper, deveria buscar todas as Pessoas e depois todas as Transações para agrupar na memória em tempo de execução, causando uma perda de performance.  
    ///2 - Poderia-se criar Views (exibições) diretamente no PostgreSQL, mas como a API não compartilha o DB com outros sistemas, DB isolado, optou-se por colocar a query na aplicação, dessa forma se a regra do relatório mudar não será preciso fazer deploy da aplicação e do BD, reduzindo acoplamento estrutural, que é válido para esse cenário   
    ///3 - Neste caso, diferente da lógica aplicada em  "PersonService", como é uma operação de leitura, não existe validação de regra de negócio, as somas foram delegadas ao banco de dados.
    ///4 - A decisão dessa aplicação pode-se mostrar invalida para outras, pois levar muitos processamentos ao banco de dados pode-se criar um cenário em que se tenha que escalar o banco,
    /// e escalar uma aplicação única e mais fácil que escalar um banco de dados. Cada caso é um caso.
    ///5 - O enum Type : 1 = Receita(Income), 2 = Despesa(Expense)
    /// </remarks>
    public class ReportQuery : IReportQuery
    {
        private readonly IDbSession _session;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="ReportQuery"/>.
        /// </summary>
        /// <param name="session">Sessão de banco de dados compartilhada para execução da consulta.</param>
        public ReportQuery(IDbSession session)
        {
            _session = session;
        }

        /// <summary>
        /// Obtém o relatório consolidado por pessoa com totais gerais inclusos.
        /// </summary>
        /// <remarks>
        /// Realiza o agrupamento de transações por pessoa diretamente no PostgreSQL e 
        /// calcula o balanço individual e geral via SQL.
        /// </remarks>
        /// <returns>Um <see cref="ReportResultDto"/> contendo a lista de pessoas e os somatórios globais.</returns>
        public async Task<ReportResultDto> GetPersonTotalsAsync()
        {
            const string sql = @"
                -- 1. Agrega por Pessoa e calcula total individual
                WITH Aggregated AS (
                    SELECT 
                        p.Name AS NameOrDescription,
                        COALESCE(SUM(CASE WHEN t.TypeId = 1 THEN t.Amount ELSE 0 END), 0) AS TotalIncome,
                        COALESCE(SUM(CASE WHEN t.TypeId = 2 THEN t.Amount ELSE 0 END), 0) AS TotalExpense
                    FROM Persons p
                    LEFT JOIN Transactions t ON p.Id = t.PersonId
                    GROUP BY p.Id, p.Name
                )
                SELECT *, (TotalIncome - TotalExpense) AS Balance FROM Aggregated ORDER BY NameOrDescription;

                -- 2. Totais consolidados
                SELECT 
                    COALESCE(SUM(CASE WHEN TypeId = 1 THEN Amount ELSE 0 END), 0) AS GrandTotalIncome,
                    COALESCE(SUM(CASE WHEN TypeId = 2 THEN Amount ELSE 0 END), 0) AS GrandTotalExpense
                FROM Transactions;";

            return await ExecuteReportAsync(sql);
        }

        /// <summary>
        /// Obtém o relatório consolidado por categoria com totais.
        /// </summary>
        /// <remarks>
        /// Realiza o agrupamento por categoria e utiliza a mesma lógica de agregação em banco 
        /// para garantir consistência nos totais globais.
        /// </remarks>
        /// <returns>Um <see cref="ReportResultDto"/> contendo a lista de categorias e os somatórios globais.</returns>
        public async Task<ReportResultDto> GetCategoryTotalsAsync()
        {
            const string sql = @"
                -- 1. Agrega por Categoria e calcula total individual
                WITH Aggregated AS (
                    SELECT 
                        c.Description AS NameOrDescription,
                        COALESCE(SUM(CASE WHEN t.TypeId = 1 THEN t.Amount ELSE 0 END), 0) AS TotalIncome,
                        COALESCE(SUM(CASE WHEN t.TypeId = 2 THEN t.Amount ELSE 0 END), 0) AS TotalExpense
                    FROM Categories c
                    LEFT JOIN Transactions t ON c.Id = t.CategoryId
                    GROUP BY c.Id, c.Description
                )
                SELECT *, (TotalIncome - TotalExpense) AS Balance FROM Aggregated ORDER BY NameOrDescription;

                -- 2. Totais consolidados
                SELECT 
                    COALESCE(SUM(CASE WHEN TypeId = 1 THEN Amount ELSE 0 END), 0) AS GrandTotalIncome,
                    COALESCE(SUM(CASE WHEN TypeId = 2 THEN Amount ELSE 0 END), 0) AS GrandTotalExpense
                FROM Transactions;";

            return await ExecuteReportAsync(sql);
        }

        /// <summary>
        /// Metodo privado para execução de múltiplas queries e mapeamento.
        /// </summary>
        /// <param name="sql">Script SQL contendo os dois ResultSets (Items e Totals).</param>
        /// <returns>Objeto de transferência de relatório populado.</returns>
        private async Task<ReportResultDto> ExecuteReportAsync(string sql)
        {
            using var multi = await _session.Connection.QueryMultipleAsync(sql, null, _session.Transaction);

            var items = (await multi.ReadAsync<ReportItemDto>()).ToList();

            // Dicionário (chave - valor) para ignorar Case Sensitivity do banco
            var totalsRow = await multi.ReadFirstAsync<dynamic>() as IDictionary<string, object>;

            // Buscar valor ignorando maiúsculas/minúsculas
            decimal GetValue(string key) =>
                totalsRow.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value is decimal val
                ? val : Convert.ToDecimal(totalsRow.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value ?? 0);

            decimal income = GetValue("GrandTotalIncome");
            decimal expense = GetValue("GrandTotalExpense");

            return new ReportResultDto(
                items,
                income,
                expense,
                income - expense
            );
        }
    }
}