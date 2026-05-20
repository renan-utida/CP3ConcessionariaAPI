SET ECHO ON

-- Renan Dias Utida - RM 558540 - 3ESPW
-- CP3ConcessionariaAPI - Scripts SQL
-- Oracle SQL Developer

-- Consultar todas as concessionarias
SELECT * FROM CP3_CONCESSIONARIA;

-- Consultar todos os clientes
SELECT * FROM CP3_CLIENTE;

-- Consultar todos os produtos (financiamentos)
SELECT * FROM CP3_PRODUTO;

-- Consultar todas as contratacoes
SELECT * FROM CP3_CONTRATACAO;

-- Consultar tudo relacionado (JOIN completo)
SELECT 
    ct.ID_CONTRATACAO,
    cl.NM_CLIENTE,
    cl.TIPO_CLIENTE,
    co.NM_CONCESSIONARIA,
    p.NM_PRODUTO,
    p.TIPO_PRODUTO,
    ct.STATUS,
    ct.DT_SOLICITACAO
FROM CP3_CONTRATACAO ct
JOIN CP3_CLIENTE cl ON ct.ID_CLIENTE = cl.ID_CLIENTE
JOIN CP3_CONCESSIONARIA co ON cl.ID_CONCESSIONARIA = co.ID_CONCESSIONARIA
JOIN CP3_PRODUTO p ON ct.ID_PRODUTO = p.ID_PRODUTO;