using Application.DTOs.Person;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller responsável por pessoas.
    /// </summary>
    /// <remarks>
    /// Seguindo os princípios de DDD, esta controller delega a validação de regras de negócio 
    /// (nome e idade) para a entidade de domínio <see cref="Domain.Entities.Person"/>. 
    /// Exceções de domínio são tratadas globalmente via Middleware.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        /// <summary>
        /// Inicializa uma nova instância da <see cref="PersonController"/>.
        /// </summary>
        /// <param name="personService">Serviço de aplicação para pessoas.</param>
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        /// <summary>
        /// Cadastra uma nova pessoa no sistema.
        /// </summary>
        /// <param name="dto">Dados para criação da pessoa.</param>
        /// <returns>Os dados da pessoa criada com seu id.</returns>
        /// <response code="201">Pessoa criada com sucesso.</response>
        /// <response code="400">Dados inválidos ou violação de regra de domínio.</response>
        [HttpPost]
        [ProducesResponseType(typeof(PersonResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePersonDto dto)
        {
            var result = await _personService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Atualiza os dados de uma pessoa existente.
        /// </summary>
        /// <param name="id">Identificador único (GUID) da pessoa na rota.</param>
        /// <param name="dto">Dados atualizados da pessoa.</param>
        /// <returns>Os dados atualizados da pessoa.</returns>
        /// <response code="200">Atualização realizada com sucesso.</response>
        /// <response code="400">Divergência de IDs ou dados inválidos.</response>
        /// <response code="404">Pessoa não encontrada.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(PersonResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "O ID da rota difere do ID do corpo da requisição." });

            var result = await _personService.UpdateAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Remove uma pessoa do sistema.
        /// </summary>
        /// <param name="id">Identificador único da pessoa.</param>
        /// <response code="204">Remoção concluída com sucesso (sem conteúdo de retorno).</response>
        /// <response code="404">Pessoa não encontrada.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _personService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Obtém a listagem de todas as pessoas cadastradas.
        /// </summary>
        /// <returns>Uma lista de pessoas.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _personService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtém pessoa específica pelo seu id.
        /// </summary>
        /// <param name="id">Identificador único da pessoa.</param>
        /// <returns>Os dados da pessoa solicitada.</returns>
        /// <response code="200">Pessoa encontrada e retornada.</response>
        /// <response code="404">Pessoa não encontrada.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PersonResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _personService.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}