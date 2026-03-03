using Domain.Enums;

namespace Application.DTOs.Category
{
    /// <summary>
    /// Objeto de transferência de dados (DTO) para resposta de categorias.
    /// </summary>
    /// <remarks>
    /// Implementado como <see langword="record"/> para garantir imutabilidade e comparação por valor, 
    /// ideal para o transporte de dados entre a camada de Application e a API.
    /// </remarks>
    /// <param name="Id">Identificador único da categoria.</param>
    /// <param name="Description">Descrição textual da categoria.</param>
    /// <param name="Purpose">Finalidade da categoria (Receita, Despesa ou Ambos).</param>
    public record CategoryResponseDto(Guid Id, string Description, CategoryPurpose Purpose);
}