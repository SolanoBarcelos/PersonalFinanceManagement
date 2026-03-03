-- 1. Criando Pessoas (Uma maior de idade e uma menor)
INSERT INTO Persons (Id, Name, Age) VALUES 
(gen_random_uuid(), 'João Silva (Adulto)', 30),
(gen_random_uuid(), 'Pedro Santos (Menor)', 15);

-- 2. Criando Categorias
-- Obs.: Precisa buscar o ID de uma pessoa e categoria para as transações abaixo.

INSERT INTO Categories (Id, Description, PurposeId) VALUES 
(gen_random_uuid(), 'Salário Mensal', 1), -- Purpose: Receita
(gen_random_uuid(), 'Alimentação', 2);    -- Purpose: Despesa

-- 3. Criando Transações 
-- Exemplo 1: Receita para o Adulto
INSERT INTO Transactions (Id, Description, Amount, TypeId, CategoryId, PersonId) 
VALUES (
    gen_random_uuid(), 
    'Recebimento de Salário', 
    5000.00, 
    1, -- Type: Receita
    (SELECT Id FROM Categories WHERE Description = 'Salário Mensal' LIMIT 1),
    (SELECT Id FROM Persons WHERE Name LIKE 'João%' LIMIT 1)
);

-- Exemplo 2: Despesa para o Menor
INSERT INTO Transactions (Id, Description, Amount, TypeId, CategoryId, PersonId) 
VALUES (
    gen_random_uuid(), 
    'Lanche na Escola', 
    111.50, 
    2, -- Type: Despesa
    (SELECT Id FROM Categories WHERE Description = 'Alimentação' LIMIT 1),
    (SELECT Id FROM Persons WHERE Name LIKE 'Pedro%' LIMIT 1)
);