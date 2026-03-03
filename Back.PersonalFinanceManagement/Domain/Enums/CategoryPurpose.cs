namespace Domain.Enums
{
    /// <summary>
    /// Finalidades permitidas para uma Categoria:
    /// <para>1 - Receita (Entrada)</para>
    /// <para>2 - Despesa (Saída)</para>
    /// <para>3 - Ambos tipos - Receita e despesa</para>
    /// </summary>
    public enum CategoryPurpose
    {
        Income = 1,
        Expense = 2,
        Both = 3
    }
}