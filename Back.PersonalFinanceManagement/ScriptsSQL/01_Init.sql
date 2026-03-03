-- Cria Tabela para o Enum CategoryPurposes
CREATE TABLE CategoryPurposes (
    Id SMALLINT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE
);

INSERT INTO CategoryPurposes (Id, Name) VALUES 
(1, 'Receita'), 
(2, 'Despesa'), 
(3, 'Ambas');

-- Cria Tabela para o Enum TransactionType
CREATE TABLE TransactionTypes (
    Id SMALLINT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE
);

INSERT INTO TransactionTypes (Id, Name) VALUES 
(1, 'Receita'), 
(2, 'Despesa');

-- Cria Tabela Persons
CREATE TABLE Persons (
    Id UUID PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Age INTEGER NOT NULL CHECK (Age >= 0)
);

-- Cria Tabela Categories
CREATE TABLE Categories (
    Id UUID PRIMARY KEY,
    Description VARCHAR(400) NOT NULL,
    PurposeId SMALLINT NOT NULL,
    
    CONSTRAINT FK_Categories_CategoryPurposes FOREIGN KEY (PurposeId) 
        REFERENCES CategoryPurposes(Id) ON DELETE RESTRICT
);

-- Cria Tabela Transactions
CREATE TABLE Transactions (
    Id UUID PRIMARY KEY,
    Description VARCHAR(400) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL CHECK (Amount > 0),
    TypeId SMALLINT NOT NULL,
    CategoryId UUID NOT NULL,
    PersonId UUID NOT NULL,
    
    CONSTRAINT FK_Transactions_TransactionTypes FOREIGN KEY (TypeId) 
        REFERENCES TransactionTypes(Id) ON DELETE RESTRICT,
        
    CONSTRAINT FK_Transactions_Categories FOREIGN KEY (CategoryId) 
        REFERENCES Categories(Id) ON DELETE RESTRICT,
        
    CONSTRAINT FK_Transactions_Persons FOREIGN KEY (PersonId) 
        REFERENCES Persons(Id) ON DELETE CASCADE
);

-- Cria Índices para otimizar os relatórios e procuras
CREATE INDEX IX_Transactions_PersonId ON Transactions(PersonId);
CREATE INDEX IX_Transactions_CategoryId ON Transactions(CategoryId);
CREATE INDEX IX_Transactions_TypeId ON Transactions(TypeId);
CREATE INDEX IX_Categories_PurposeId ON Categories(PurposeId)