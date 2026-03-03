using Application.DTOs.Category;

namespace Application.Interfaces
{
    /// <summary>
    /// Define o contrato de serviço para operações de categoria.
    /// </summary>
    /// <remarks>
    /// Esta interface beneficia a aplicação do Princípio de Inversão de Dependência (DIP). 
    /// Ela permite que a Camada de API dependa de uma abstração e não de uma implementação concreta (CategoryService), 
    /// facilitando a Inversão de Controle (IoC) atravéz de DI, e também a realização de testes unitários com Mock caso precise.
    /// </remarks>
    public interface ICategoryService
    {
        /// <summary>
        /// Criação de uma categoria, validando os dados através do DTO.
        /// </summary>
        /// <param name="dto">Dados de entrada para a nova categoria.</param>
        /// <returns>Uma task que representa a operação assíncrona, contendo o DTO de resposta.</returns>
        Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto);

        /// <summary>
        /// Recupera todas as categorias disponíveis de forma assíncrona.
        /// </summary>
        /// <returns>Uma coleção de categorias para resposta.</returns>
        Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
    }
}
