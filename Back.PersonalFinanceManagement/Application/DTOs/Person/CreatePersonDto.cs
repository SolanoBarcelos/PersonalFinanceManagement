namespace Application.DTOs.Person
{
    /// <summary>
    /// Objeto de transferência de dados para a criação de uma nova pessoa.
    /// </summary>
    /// <remarks>
    /// Implementado como <see langword="record"/> para aproveitar a imutabilidade e a comparação por valor.
    /// Este DTO transporta os dados básicos que serão validados pela entidade de domínio <see cref="Domain.Entities.Person"/>.
    /// </remarks>
    /// <param name="Name">Nome completo da pessoa (máx. 200 caracteres).</param>
    /// <param name="Age">Idade da pessoa (deve ser um valor não negativo).</param>
    public record CreatePersonDto(string Name, int Age);
}