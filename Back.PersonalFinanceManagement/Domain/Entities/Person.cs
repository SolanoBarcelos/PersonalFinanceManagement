namespace Domain.Entities
{
    /// <summary>
    /// Representa uma pessoa.
    /// </summary>
    public class Person
    {
        /// <summary>Identificador único da pessoa gerado automaticamente.</summary>
        public Guid Id { get; private set; }

        /// <summary>Nome da pessoa (máx. 200 caracteres).</summary>
        public string Name { get; private set; }

        /// <summary>Idade da pessoa.</summary>
        /// <remarks>Utilizada para validar se o usuário pode registrar receitas ou apenas despesas.</remarks>
        public int Age { get; private set; }

        /// <summary>Construtor para uso do Dapper/Micro ORM.</summary>
        protected Person() { }

        /// <summary>
        /// Inicializa uma nova instância de uma pessoa com validação de dados.
        /// </summary>
        /// <param name="name">Nome da pessoa.</param>
        /// <param name="age">Idade da pessoa.</param>
        /// <exception cref="ArgumentException">Lançada se o nome for inválido ou a idade for negativa.</exception>
        public Person(string name, int age)
        {
            Id = Guid.NewGuid();
            SetName(name);
            SetAge(age);
        }

        /// <summary>
        /// Atualiza os dados cadastrais da pessoa respeitando as regras de domínio.
        /// </summary>
        /// <param name="name">Novo nome.</param>
        /// <param name="age">Nova idade.</param>
        public void Update(string name, int age)
        {
            SetName(name);
            SetAge(age);
        }

        /// <summary>
        /// Valida e atribui o nome da pessoa.
        /// </summary>
        /// <remarks>Regra: Não pode ser nulo/vazio e deve ter no máximo 200 caracteres.</remarks>
        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 200)
                throw new ArgumentException("Nome não pode ser vazio e deve ter entre 1 e 200 caracteres");

            Name = name;
        }

        /// <summary>
        /// Valida e atribui a idade.
        /// </summary>
        /// <remarks>Regra: A idade não pode ser negativa.</remarks>
        private void SetAge(int age)
        {
            if (age < 0)
                throw new ArgumentException("A idade não pode ser um número negativo");

            Age = age;
        }
    }
}