using FluentAssertions;
using Domain.Entities;

namespace UnitTests.Domain
{
    public class DomainEntitiesTests
    {
        #region Testes da Entidade Person

        [Fact(DisplayName = "Criar Person com dados válidos deve ter sucesso")]
        public void Person_CriarComDadosValidos_DeveTerSucesso()
        {
            var person = new Person("Solano", 20);

            person.Should().NotBeNull();
            person.Id.Should().NotBeEmpty();
            person.Name.Should().Be("Solano");
            person.Age.Should().Be(20);
        }

        [Theory(DisplayName = "Criar Person com nome inválido deve lançar exceção")]
        [InlineData("")]
        [InlineData(null)]
        public void Person_CriarComNomeInvalido_DeveLancarExcecao(string invalidName)
        {
            Action act = () => new Person(invalidName, 20);
            act.Should().Throw<ArgumentException>();
        }

        #endregion               

        #region Testes da Entidade Category

        //Testes da Category

        #endregion

        #region Testes da Entidade Transaction

        // Testes da Transaction

        #endregion
    }
}