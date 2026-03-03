# Personal Finance Management API

Uma API robusta para controle de finanças pessoais, desenvolvida em **.NET 8** (C#) e **PostgreSQL**. Este projeto foi construído com um foco em separação camadas, utilizando **Clean Architecture**, **Domain-Driven Design (DDD)** e **CQRS**.

## Decisões Arquiteturais e Padrões de Projeto

### 1. Regras de Negócio: O Domínio como "Fonte Única de Verdade"
A responsabilidade de validar regras de negócio complexas (ex: se uma transação de "Despesa" pode ser associada a uma categoria de "Receita", ou validações de idade) reside nas **Entidades de Domínio** e **Value Objects**. 

### 2. Separação de Leitura e Escrita (Conceito CQRS) e a Ausência de "Views"
Para a geração de relatórios e totais, foi isolada a responsabilidade em consultas diretas (`ReportQuery`) em vez de utilizar as classes de Repositório tradicionais.
* **Por que não usar Views no PostgreSQL?** Criar *Views* na base de dados para cálculos agregados cria acoplamento entre a aplicação e o motor do banco de dados, além de dificultar o versionamento da lógica (regra fica "escondida" no banco). Ao mantermos as *Queries* analíticas escritas em SQL diretamente nas classes de relatórios da Infraestrutura via **Dapper**, garantimos que toda a lógica de agregação esteja versionada no Git, seja fácil de rastrear e execute com performance.

### 3. Middleware Global de Tratamento de Exceções
Os *Controllers* da API são estritamente anêmicos e não possuem blocos `try/catch`.
Implementação de um Middleware Global que intercepta toda a esteira HTTP. Se o Domínio lançar um erro de regra de negócio (`InvalidOperationException` ou `ArgumentException`), o Middleware captura e o traduz para um HTTP Status `400 Bad Request`. Erros de recurso não encontrado (`KeyNotFoundException`) viram `404 Not Found`. Erros `500` são tratados para não exporem informações sensíveis, retornando uma mensagem genérica.


### 4. Gerenciamento de Conexão: O Padrão `DbSession`
Em vez de injetar o `IDbConnection` diretamente nos Repositórios, foi criado `DbSession`.
Injetado com ciclo de vida do container como `Scoped`, o `DbSession` garante que uma única conexão com o PostgreSQL seja aberta por requisição HTTP.


### 6. Desacoplamento da Injeção de Dependência
O arquivo `Program.cs` foi mantido enxuto utilizando *Extension Methods* (`AddApplication` e `AddInfrastructure`). A camada de Apresentação (API) não conhece as implementações concretas da Infraestrutura, apenas solicita as interfaces, garantindo que o acoplamento ocorra apenas em tempo de execução.
