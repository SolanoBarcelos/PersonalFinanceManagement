namespace Application.DTOs.Person
{
    /// <summary>
    /// Objeto de transferência de dados que representa a resposta detalhada de uma pessoa.
    /// </summary>
    /// <remarks>
    /// Implementado como <see langword="record"/> para assegurar a imutabilidade dos dados retornados 
    /// pela camada de Application. Comparação pelo valor e não pela referência.
    /// </remarks>
    /// <param name="Id">Identificador único (GUID) da pessoa no sistema.</param>
    /// <param name="Name">Nome cadastrado da pessoa.</param>
    /// <param name="Age">Idade da pessoa, utilizada para validação de regras de transação.</param>
    public record PersonResponseDto(Guid Id, string Name, int Age);
}