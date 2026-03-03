using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities
{
    /// <summary>
    /// Representa uma transação vinculada a uma pessoa e uma categoria.
    /// </summary>
    /// <remarks>
    /// Esta entidade utiliza o <see cref="DescriptionVO"/> internamente para garantir a integridade da descrição,
    /// mas expõe uma propriedade de compatibilidade para permitir o mapeamento automático pelo Dapper.
    /// </remarks>
    public class Transaction
    {
        /// <summary>Identificador único da transação gerado automaticamente.</summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Descrição textual da transação.
        /// </summary>
        /// <remarks>
        /// Esta propriedade atua como uma ponte para o Dapper. O 'get' retorna o valor bruto do VO
        /// e o 'set' privado garante que qualquer valor vindo do banco de dados seja incluido pelo <see cref="DescriptionVO"/>.
        /// </remarks>
        public string Description
        {
            get => _descriptionVO.Value;
            private set => _descriptionVO = new DescriptionVO(value);
        }

        /// <summary>Campo privado que armazena o Objeto de Valor real.</summary>
        private DescriptionVO _descriptionVO;

        /// <summary>Valor monetário da transação (deve ser maior que zero).</summary>
        public decimal Amount { get; private set; }

        /// <summary>Tipo da transação (1 - Receita / 2 - Despesa).</summary>
        public TransactionType Type { get; private set; }

        /// <summary>Identificador da categoria associada.</summary>
        public Guid CategoryId { get; private set; }

        /// <summary>Identificador da pessoa que realizou a transação.</summary>
        public Guid PersonId { get; private set; }

        /// <summary>Construtor para uso do Dapper/Micro ORM.</summary>
        protected Transaction() { }

        /// <summary>
        /// Inicializa uma transação validando regras de idade e compatibilidade de categoria.
        /// </summary>
        /// <param name="description">Texto descritivo validado pelo Objeto de Valor.</param>
        /// <param name="amount">Valor da transação.</param>
        /// <param name="type">Tipo (Receita ou Despesa).</param>
        /// <param name="categoryId">ID da Categoria associada.</param>
        /// <param name="categoryPurpose">Finalidade da categoria (para validação de regra de negócio).</param>
        /// <param name="personId">ID da Pessoa associada.</param>
        /// <param name="personAge">Idade da pessoa (para validação de restrição de menores).</param>
        /// <exception cref="ArgumentException">Lançada se o valor for inválido ou se a descrição falhar no VO.</exception>
        /// <exception cref="InvalidOperationException">Lançada se as regras de negócio forem violadas.</exception>
        public Transaction(string description, decimal amount, TransactionType type,
                           Guid categoryId, CategoryPurpose categoryPurpose,
                           Guid personId, int personAge)
        {
            Id = Guid.NewGuid();

            // Atribuição direta à propriedade pública para disparar a validação do VO no 'set'
            Description = description;
            SetAmount(amount);
            ValidateBusinessRules(type, personAge, categoryPurpose);

            Type = type;
            CategoryId = categoryId;
            PersonId = personId;
        }

        /// <summary>
        /// Atribui e valida o valor monetário.
        /// </summary>
        private void SetAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("O valor da transação precisa ser maior que zero.", nameof(amount));

            Amount = amount;
        }

        /// <summary>
        /// Centraliza a validação das regras de negócio complexas.
        /// </summary>
        /// <remarks>
        /// <para>1. Menores de 18 anos podem registrar apenas despesas.</para>
        /// <para>2. Categorias com finalidade 'Ambas' são sempre aceitas.</para>
        /// <para>3. Bloqueia o uso de categorias exclusivas de Receita em transações de Despesa e vice-versa.</para>
        /// </remarks>
        private void ValidateBusinessRules(TransactionType type, int personAge, CategoryPurpose categoryPurpose)
        {
            if (personAge < 18 && type != TransactionType.Expense)
                throw new InvalidOperationException("Menores de 18 anos possuem permissão apenas para o registro de despesas.");

            if (categoryPurpose == CategoryPurpose.Both) return;

            bool isInvalidIncome = type == TransactionType.Income && categoryPurpose == CategoryPurpose.Expense;
            bool isInvalidExpense = type == TransactionType.Expense && categoryPurpose == CategoryPurpose.Income;

            if (isInvalidIncome || isInvalidExpense)
                throw new InvalidOperationException("A categoria selecionada é incompatível com o tipo de transação informada.");
        }
    }
}