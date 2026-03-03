using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities
{
    /// <summary>
    /// Representa uma categoria.
    /// </summary>
    /// <remarks>
    /// Esta entidade utiliza o <see cref="DescriptionVO"/> para garantir a integridade da descrição,
    /// mas expõe uma propriedade de compatibilidade para permitir o mapeamento automático pelo Dapper.
    /// </remarks>
    public class Category
    {
        /// <summary>Identificador único da categoria gerado automaticamente.</summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Descrição textual da categoria.
        /// </summary>
        /// <remarks>
        /// Esta propriedade atua como uma ponte para o Dapper. O 'get' retorna o valor bruto do VO
        /// e o 'set' privado garante que qualquer valor vindo do banco de dados seja revalidado pelo <see cref="DescriptionVO"/>.
        /// </remarks>
        public string Description
        {
            get => _descriptionVO.Value;
            private set => _descriptionVO = new DescriptionVO(value);
        }

        /// <summary>Campo privado que armazena o Objeto de Valor real.</summary>
        private DescriptionVO _descriptionVO;

        /// <summary>Finalidade da categoria (1 - Receita / 2 - Despesa / 3 - Ambas).</summary>
        public CategoryPurpose Purpose { get; private set; }

        /// <summary>Construtor protegido para persistência via Dapper/Micro ORM.</summary>
        protected Category() { }

        /// <summary>
        /// Inicializa uma nova instância de categoria com validação rigorosa.
        /// </summary>
        /// <param name="description">Texto descritivo validado pelo Objeto de Valor.</param>
        /// <param name="purpose">Finalidade vinculada à categoria.</param>
        public Category(string description, CategoryPurpose purpose)
        {
            Id = Guid.NewGuid();
            // Atribuição direta à propriedade pública para disparar a validação do VO no 'set'
            Description = description;
            Purpose = purpose;
        }

        /// <summary>
        /// Atualiza os dados da categoria respeitando as regras de integridade do domínio.
        /// </summary>
        /// <param name="description">Nova descrição a ser validada.</param>
        /// <param name="purpose">Nova finalidade da categoria.</param>
        public void Update(string description, CategoryPurpose purpose)
        {
            Description = description;
            Purpose = purpose;
        }
    }
}