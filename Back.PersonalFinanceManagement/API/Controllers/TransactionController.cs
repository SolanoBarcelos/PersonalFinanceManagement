using Application.DTOs.Transaction;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Controller responsável para transações (Receitas e Despesas).
    /// </summary>
    /// <remarks>
    /// A intenção e seguir o padrão Controller Anêmica, delegando as validações de 
    /// regras de negócio para a entidade de domínio <see cref="Domain.Entities.Transaction"/>.
    /// As violações de regra de negócio geram exceções capturadas pelo Middleware global.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        /// <summary>
        /// Inicializa uma nova instância da <see cref="TransactionController"/>.
        /// </summary>
        /// <param name="transactionService">Serviço de aplicação para transações.</param>
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Registra uma nova transação financeira no sistema.
        /// </summary>
        /// <remarks>
        /// Regras de Negócio:
        /// <para>1. Idade: Menores de 18 anos só podem registrar transações do tipo Despesa.</para>
        /// <para>2. Compatibilidade: O tipo da transação (Receita - 1/ Despesa - 2) deve ser compatível com a finalidade da categoria escolhida.</para>
        /// <para>3. Exceção: Categorias com finalidade 'Ambas' são aceitas para qualquer tipo de transação.</para>
        /// </remarks>
        /// <param name="dto">Objeto contendo os dados da transação (descrição, valor, tipo, categoria e pessoa).</param>
        /// <returns>Retorna os dados da transação criada, incluindo o seu id.</returns>
        /// <response code="201">Transação criada com sucesso.</response>
        /// <response code="400">Dados inválidos ou violação das regras de negócio (ex: menor de idade tentando criar receita).</response>
        [HttpPost]
        [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
        {
            var result = await _transactionService.CreateAsync(dto);
            return Created("", result);
        }

        /// <summary>
        /// Recupera a listagem completa de transações financeiras.
        /// </summary>
        /// <returns>Uma coleção de transações registradas.</returns>
        /// <response code="200">Lista de transações retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _transactionService.GetAllAsync();
            return Ok(result);
        }
    }
}