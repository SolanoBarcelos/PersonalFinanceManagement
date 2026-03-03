namespace Application.DTOs.Person
{
    /// <summary>
    /// Objeto de transferência de dados para atualização de uma pessoa existente.
    /// </summary>
    /// <remarks>
    /// Implementado como <see langword="record"/> para garantir a imutabilidade dos dados durante o processo de atualização. 
    /// Contém o identificador único para garantir que a alteração seja aplicada ao registro correto. A comparação é feita pelo valor e não pela referência.
    /// </remarks>
    /// <param name="Id">Identificador único (GUID) da pessoa a ser atualizada.</param>
    /// <param name="Name">Novo nome da pessoa (respeitando o limite de 200 caracteres).</param>
    /// <param name="Age">Nova idade da pessoa (deve ser um valor não negativo).</param>
    public record UpdatePersonDto(Guid Id, string Name, int Age);
}