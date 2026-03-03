using Application.DTOs.Reports;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller para geração de relatórios e consolidação de dados financeiros.
    /// </summary>
    /// <remarks>
    /// Esta controller utiliza consultas otimizadas para retornar o balanço entre receitas e despesas.
    /// Erros de processamento são capturados pelo Middleware de exceção global.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportQuery _reportQuery;

        /// <summary>
        /// Inicializa uma nova instância da <see cref="ReportController"/>.
        /// </summary>
        /// <param name="reportQuery">Interface de consulta para relatórios.</param>
        public ReportController(IReportQuery reportQuery)
        {
            _reportQuery = reportQuery;
        }

        /// <summary>
        /// Obtém o total de transações agrupado por pessoa.
        /// </summary>
        /// <remarks>
        /// O relatório retorna a lista de pessoas com totais de receitas, 
        /// despesas e o saldo final (balance).
        /// </remarks>
        /// <returns>Um objeto <see cref="ReportResultDto"/> contendo os totais por pessoa.</returns>
        /// <response code="200">Relatório gerado com sucesso.</response>
        [HttpGet("persons")]
        [ProducesResponseType(typeof(ReportResultDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonTotals()
        {
            var result = await _reportQuery.GetPersonTotalsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtém o total de transações agrupado por categoria.
        /// </summary>
        /// <remarks>
        /// O cálculo respeita as finalidades (Income/Expense/Both) definidas no domínio.
        /// </remarks>
        /// <returns>Um objeto <see cref="ReportResultDto"/> contendo os totais por categoria.</returns>
        /// <response code="200">Relatório gerado com sucesso.</response>
        [HttpGet("categories")]
        [ProducesResponseType(typeof(ReportResultDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoryTotals()
        {
            var result = await _reportQuery.GetCategoryTotalsAsync();
            return Ok(result);
        }
    }
}