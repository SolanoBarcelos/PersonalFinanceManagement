using Application.DTOs.Category;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller responsável pelas categorias.
    /// </summary>
    /// <remarks>
    /// Esta controller é projetada para ser anêmica, delegando as validações de regras de negócio 
    /// para as entidades de domínio (DDD). 
    /// Erros de domínio são capturados por um Middleware de exceção global.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Inicializa uma nova instância da <see cref="CategoryController"/>.
        /// </summary>
        /// <param name="categoryService">Serviço de aplicação para categorias.</param>
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Registra uma nova categoria no sistema.
        /// </summary>
        /// <remarks>
        /// A categoria pode ter finalidades distintas (Receita - 1/ Despesa - 2/ Ambos - 3).
        /// </remarks>
        /// <param name="dto">Dados para criação da categoria.</param>
        /// <returns>A categoria criada com seu id.</returns>
        /// <response code="201">Categoria criada com sucesso.</response>
        /// <response code="400">Dados inválidos ou violação de regra de domínio.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var result = await _categoryService.CreateAsync(dto);

            return Created("", result);
        }

        /// <summary>
        /// Listagem completa de categorias cadastradas.
        /// </summary>
        /// <returns>Uma coleção de categorias.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return Ok(result);
        }
    }
}