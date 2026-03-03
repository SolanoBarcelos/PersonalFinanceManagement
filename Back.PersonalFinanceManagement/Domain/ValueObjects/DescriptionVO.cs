namespace Domain.ValueObjects
{
    /// <summary>
    /// Objeto de Valor que representa uma descrição validada no domínio.
    /// </summary>
    /// <remarks>
    /// Centraliza as regras de validação para Description usada nas entidades, garantindo que 
    /// a integridade dos dados seja mantida desde a sua criação.
    /// </remarks>
    public sealed record DescriptionVO
    {
        /// <summary>O valor textual bruto da descrição.</summary>
        public string Value { get; init; }

        /// <summary>
        /// Inicializa uma nova instância de <see cref="DescriptionVO"/> com validações de regra de negócio.
        /// </summary>
        /// <param name="description">Texto da descrição a ser processado.</param>
        /// <exception cref="ArgumentException">Lançada se a descrição for nula, vazia ou exceder o limite de caracteres.</exception>
        public DescriptionVO(string description)
        {
            // Validação de nulidade e conteúdo vazio
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("A descrição é obrigatória e não pode ser vazia.", nameof(description));

            description = description.Trim();

            // Validação de limite de caracteres (Regra de Negócio)
            if (description.Length > 400)
                throw new ArgumentException("A descrição não pode exceder 400 caracteres.", nameof(description));

            this.Value = description;
        }

        /// <summary>
        /// Retorna a string encapsulada no objeto de valor.
        /// </summary>
        /// <returns>O valor da descrição.</returns>
        public override string ToString() => Value;
    }
}