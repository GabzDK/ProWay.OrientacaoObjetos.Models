;INSERT INTO ContasPoupanca (nome, saldo) 
VALUES ('Pedro', 2990.50)

SELECT * FROM ContasPoupanca

UPDATE ContasPoupanca 
	SET saldo = 3190.50
	WHERE codigo = 3
		

DELETE FROM ContasPoupanca
	WHERE codigo = 2
