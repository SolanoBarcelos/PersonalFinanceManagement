using Application.DTOs.Category;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    /// <summary>
    /// Serviço de aplicação para gerenciamento de categorias.
    /// </summary>
    /// <remarks>
    /// Esta classe orquestra o fluxo de dados entre a API e o domínio, garantindo que as 
    /// operações persistidas respeitem as regras da entidade <see cref="Category"/> e seu Objeto de Valor.
    /// </remarks>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="CategoryService"/>.
        /// </summary>
        /// <param name="categoryRepository">Repositório de persistência de categorias.</param>
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _repository = categoryRepository;
        }

        /// <summary>
        /// Cria uma nova categoria após validar as regras de negócio no domínio via VO.
        /// </summary>
        /// <remarks>
        /// A validação ocorre através da propriedade 'Description' da entidade, que aciona 
        /// internamente o <see cref="DescriptionVO"/>.
        /// </remarks>
        /// <param name="dto">Objeto DTO contendo os dados de entrada da categoria.</param>
        /// <returns>Dados da categoria criada para retorno à API.</returns>
        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
        {
            // O construtor da Entidade recebe a string e valida via VO internamente.
            var category = new Category(dto.Description, dto.Purpose);

            await _repository.AddAsync(category);

            // Acesso a 'Description' diretamente (retorna a string validada do VO).
            return new CategoryResponseDto(
                category.Id,
                category.Description,
                category.Purpose
            );
        }

        /// <summary>
        /// Recupera todas as categorias cadastradas no sistema.
        /// </summary>
        /// <returns>Uma coleção de DTOs mapeados a partir das entidades de domínio.</returns>
        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();

            // Mapeamento manual: a propriedade 'Description' já entrega o valor textual necessário.
            return categories.Select(c => new CategoryResponseDto(
                c.Id,
                c.Description,
                c.Purpose
            ));
        }
    }
}