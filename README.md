# Personal Finance Management

Sistema de gestão financeira pessoal desenvolvido com .NET 8 e React (Vite).

## Estrutura do Projeto
- **/Back.PersonalFinanceManagement**: API REST utilizando ASP.NET Core, Dapper e Clean Architecture + DDD.
- **/Front.PersonalFinanceManagement**: Interface com React e TypeScript.

## Tecnologias Principais
- **Backend:** C# (.NET 8), PostgreSQL, Dapper (Micro-ORM).
- **Frontend:** React, Vite, Axios, Tailwind CSS.
- **Infraestrutura:** Docker e Docker Compose para |Banco de Dados.

## Pipeline CI
- Configuração de pipeline CI utilizando GitHub Actions para automação de testes.

## Configuração e Execução

## 1. Banco de Dados (Docker)
- O projeto utiliza um container **PostgreSQL**. Na raiz do diretório de Backend, execute:
```bash
docker-compose up -d
```
- Para apagar o container e volumes associados:
```bash
docker-compose down -v
```
## 2. Frontend

- Instale as dependências do projeto:
```bash
npm install
```

- Rode o projeto localmente:
```bach
npm run dev
```
- Acesse o link localhost, ex.: `http://localhost:5173`, e atualize a program.cs no backend.
```bash
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            // Adicionar porta que o front abrir no localhost aqui!
            policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:5175")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
```
## 3. Backend
- Apenas execute o projeto de forma local:
```bash
dotnet run --project Back.PersonalFinanceManagement
```
## Obs.:
Informações arquiteturais, padrões de projeto e outras escolhas técnicas podem ser encontradas no README de cada projeto.

## Melhorias Futuras:
- Arquivos em ingles no Frontend.
- Implementação do Design Pattern Extrategy + Single Factory para enuns na Domain.