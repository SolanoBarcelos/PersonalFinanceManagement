using Domain.Enums;

namespace Application.DTOs.Category
{
    /// <summary>
    /// Objeto de transferência de dados para a criação de uma nova categoria.
    /// </summary>
    /// <remarks>
    /// Implementado como <see langword="record"/> para garantir a imutabilidade dos dados de entrada. 
    /// A comparação é feita pelo valor e não pela referência.
    /// </remarks>
    /// <param name="Description">Descrição detalhada da categoria (Ex: "Supermercado", "Salário").</param>
    /// <param name="Purpose">Finalidade da categoria que define quais tipos de transação ela aceita.</param>
    public record CreateCategoryDto(string Description, CategoryPurpose Purpose);
}