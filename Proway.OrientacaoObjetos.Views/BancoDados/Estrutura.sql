DROP TABLE ContasPoupanca

CREATE TABLE ContasPoupanca(
	codigo INT PRIMARY KEY IDENTITY,
	nome VARCHAR(100),
	saldo DECIMAL (6, 2) NOT NULL
);
-- VARCHAR(100) campo de texto permite até 100 caracteres
-- DECIMAL número do tipo real
-- INT inteiro
-- IDENTITY para ser auto incrementado a coluna ou seja, primeiro registo 1, 2, 3
-- PRIMARY KEY chave primária 
-- NOT NULL não permite nulo()