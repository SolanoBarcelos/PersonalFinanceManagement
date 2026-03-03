# Personal Finance Management

Sistema de gestão financeira pessoal desenvolvido com .NET 8 e React (Vite).
Vídeo documental de funcionalidades do Frontend e Swagger estão na pasta Video na raiz do repositório

## Estrutura do Projeto
- **/Back.PersonalFinanceManagement**: API REST utilizando ASP.NET Core, Dapper e Clean Architecture + DDD.
- **/Front.PersonalFinanceManagement**: Interface com React e TypeScript.

## Tecnologias Principais
- **Backend:** C# (.NET 8), PostgreSQL, Dapper (Micro-ORM).
- **Frontend:** React, Vite, Axios, Tailwind CSS.
- **Infraestrutura:** Docker e Docker Compose para |Banco de Dados.

## Pipeline CI
- Configuração de pipeline CI utilizando GitHub Actions para automação de testes, o gatilho é cada push ou pull request na branch main. Para Verificar o pipeline, acesse a aba "Actions" no repositório do GitHub. 

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

- Resetar o package.json (se estiver corrompido/vazio):
```bash
npm init -y
```
- 

- Instale as dependências do projeto:
```bash
npm install
npm init -y
npm install vite @vitejs/plugin-react --save-dev
npm install react react-dom
npm install @tailwindcss/vite tailwindcss
npm install tailwind-merge clsx lucide-react
npm install axios tailwind-variants
```
ou

```bash
npm init -y
npm install vite @vitejs/plugin-react @tailwindcss/vite tailwindcss --save-dev
npm install react react-dom axios tailwind-merge tailwind-variants clsx lucide-react
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
Informações arquiteturais, padrões de projeto e outras escolhas técnicas podem ser encontradas no README de cada projeto e também em comentários/documentação no código

## Melhorias Futuras:
- Arquivos em ingles no Frontend.
- Implementação do Design Pattern Extrategy + Single Factory para enuns na Domain.