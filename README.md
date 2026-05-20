# CP3ConcessionariaAPI - Backend Concessionária Digital

Backend de uma concessionária digital desenvolvido em **ASP.NET Core Web API (.NET 8)** com **Entity Framework Core** e **Oracle Database**. Projeto avaliativo da disciplina de C# Software Development - FIAP - 3ESPW 2026.

**Turma** - 3ESPW

**Professor** - Rafael Santos Novo Pereira

**Link Repositório** - [Repositório GitHub CP3ConcessionariaAPI](https://github.com/renan-utida/CP3ConcessionariaAPI)

---

## 1. Identificação

| Nome | RM |
|---|---|
| Renan Dias Utida | RM558540 |

---

## 2. Produto Escolhido

**Produto:** Financiamento

**Justificativa:** Ao analisar o tema do Checkpoint 3 escolhido por mim - Concessionária - identifiquei três especializações de Produto adequadas ao contexto: `Financiamento`, `SeguroVeiculo` e `PlanoRevisao`. Optei pelo Financiamento por ser o produto mais representativo do dia a dia de uma concessionária e por permitir a implementação de regras de negócio reais e relevantes. Além do CRUD básico, foram implementadas duas regras de negócio:

- **Cálculo automático de parcela** via fórmula Price (juros compostos): `PMT = PV * (i * (1+i)^n) / ((1+i)^n - 1)`, onde `PV` é o valor financiado (`valorVeiculo - valorEntrada`). O valor da parcela é calculado automaticamente pelo sistema ao cadastrar ou atualizar - o usuário não precisa informá-lo.
- **Avaliação de score de crédito** com base no percentual de entrada sobre o valor do veículo: entrada ≥ 30% é `APROVADO`, entre 10% e 29% fica em `ANALISE`, e abaixo de 10% é `REPROVADO`.

---

## 3. Decisão de Modelagem de Filas

> **Nota:** A implementação de mensageria com RabbitMQ (filas, consumers, Docker) não foi trabalhada em aula até a data de entrega deste checkpoint, portanto não faz parte do escopo avaliado desta entrega.

O endpoint `POST /api/contratacoes` simula o comportamento assíncrono retornando **202 Accepted**, com o status da contratação iniciando como `PENDENTE`. O status pode ser atualizado via `PUT /api/contratacoes/{id}` para refletir o resultado do processamento (`APROVADO`, `REPROVADO`, `ANALISE`).

Caso fosse implementado com RabbitMQ, a decisão seria: **1 fila por tipo de produto** (ex: `fila.financiamento`, `fila.seguro-veiculo`, `fila.plano-revisao`), pois cada produto tem regras de processamento distintas e consumidores independentes, facilitando o reprocessamento seletivo em caso de falha.

---

## 4. Diagrama de Classes

<!-- Print -->

> Arquivo fonte disponível em `docs/diagram/Diagrama_CP3_Concessionaria_558540_C#.drawio`

**Link do Diagrama (draw.io):**

[https://drive.google.com/file/d/1FqPblDIOyZmfRef-gUjRlfD9Qo70RRlh/view?usp=sharing](https://drive.google.com/file/d/1FqPblDIOyZmfRef-gUjRlfD9Qo70RRlh/view?usp=sharing)

---

