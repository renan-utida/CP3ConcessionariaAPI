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

<img width="1500" height="617" alt="Diagrama_CP3_Concessionaria_558540_C#" src="https://github.com/user-attachments/assets/ea924778-82d6-4f2b-86ee-69291530ba8e" />

> Arquivo fonte disponível em `docs/diagram/Diagrama_CP3_Concessionaria_558540_C#.drawio`

**Link do Diagrama (draw.io):**

[https://drive.google.com/file/d/1FqPblDIOyZmfRef-gUjRlfD9Qo70RRlh/view?usp=sharing](https://drive.google.com/file/d/1FqPblDIOyZmfRef-gUjRlfD9Qo70RRlh/view?usp=sharing)

---

## 5. Como Rodar Localmente
 
### Pré-requisitos
 
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (ou VS Code com extensão C#)
- Acesso à rede FIAP (VPN ou presencial) para conectar ao Oracle
- [Oracle SQL Developer](https://www.oracle.com/database/sqldeveloper/) (opcional, para visualizar as tabelas)
### Credenciais Oracle
 
```
Host:     oracle.fiap.com.br
Porta:    1521
SID:      ORCL
Usuário:  Suas Credenciais
Senha:    ********
```
 
### Configurar connection string
 
Em `CP3ConcessionariaAPI/appsettings.json`, substitua com suas credenciais:
 
```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
  }
}
```
 
### Aplicar migrations e rodar
 
```bash
# Restaurar pacotes
dotnet restore
 
# Aplicar migrations (criar tabelas no Oracle)
dotnet ef database update
 
# Rodar a API
dotnet run --project CP3ConcessionariaAPI
```
 
A API estará disponível em:
- `https://localhost:7187/swagger` — Swagger UI
- `https://localhost:7187/api/...` — Endpoints REST
---
 
## 6. Endpoints Disponíveis
 
### Concessionárias
 
#### `POST /api/Concessionarias` — Cadastrar concessionária
**Request:**
```json
{
  "nmConcessionaria": "Concessionária Centro SP",
  "cep": "01310100",
  "dsEndereco": "Av. Paulista, 1000 - Bela Vista, São Paulo"
}
```
**Response:** `201 Created`
```json
{
  "idConcessionaria": 1,
  "nmConcessionaria": "Concessionária Centro SP",
  "cep": "01310100",
  "dsEndereco": "Av. Paulista, 1000 - Bela Vista, São Paulo"
}
```
 
---
 
#### `GET /api/Concessionarias/{id}` — Buscar concessionária por ID
**Response:** `200 OK`
```json
{
  "idConcessionaria": 1,
  "nmConcessionaria": "Concessionária Centro SP",
  "cep": "01310100",
  "dsEndereco": "Av. Paulista, 1000 - Bela Vista, São Paulo"
}
```
**Response:** `404 Not Found`
```json
{ "mensagem": "Concessionária não encontrada." }
```
 
---
 
#### `GET /api/Concessionarias` — Listar todas as concessionárias
**Response:** `200 OK` — array de concessionárias
 
---
 
#### `PUT /api/Concessionarias/{id}` — Atualizar concessionária
**Request:**
```json
{
  "idConcessionaria": 1,
  "nmConcessionaria": "Concessionária Paulista Atualizada",
  "cep": "01310200",
  "dsEndereco": "Av. Paulista, 2000 - Bela Vista, São Paulo"
}
```
**Response:** `204 No Content`
 
---
 
#### `DELETE /api/Concessionarias/{id}` — Deletar concessionária
**Response:** `204 No Content`
 
---
 
### Clientes
 
#### `POST /api/Clientes/pf` — Cadastrar Pessoa Física
**Request:**
```json
{
  "nmCliente": "Renan Dias",
  "idConcessionaria": 1,
  "cpf": "12345678901",
  "dataNascimento": "2001-10-10"
}
```
**Response:** `201 Created`
```json
{
  "cpf": "12345678901",
  "dataNascimento": "2001-10-10T00:00:00",
  "idCliente": 1,
  "nmCliente": "Renan Dias",
  "idConcessionaria": 1,
  "concessionaria": {
    "idConcessionaria": 1,
    "nmConcessionaria": "Concessionária Centro SP",
    "cep": "01310100",
    "dsEndereco": "Av. Paulista, 1000 - Bela Vista, São Paulo"
  }
}
```
**Response:** `400 Bad Request` — CPF duplicado
```json
{ "mensagem": "CPF já cadastrado." }
```
**Response:** `400 Bad Request` — Concessionária inexistente
```json
{ "mensagem": "Concessionária informada não existe." }
```
 
---
 
#### `POST /api/Clientes/pj` — Cadastrar Pessoa Jurídica
**Request:**
```json
{
  "nmCliente": "Empresa Teste LTDA",
  "idConcessionaria": 1,
  "cnpj": "12345678000195",
  "razaoSocial": "Empresa Teste Ltda"
}
```
**Response:** `201 Created`
 
**Response:** `400 Bad Request` — CNPJ duplicado
```json
{ "mensagem": "CNPJ já cadastrado." }
```
 
---
 
#### `GET /api/Clientes/{id}` — Buscar cliente por ID
**Response:** `200 OK`
```json
{
  "idCliente": 1,
  "nmCliente": "Renan Dias",
  "idConcessionaria": 1,
  "concessionaria": {
    "idConcessionaria": 1,
    "nmConcessionaria": "Concessionária Centro SP",
    "cep": "01310100",
    "dsEndereco": "Av. Paulista, 1000 - Bela Vista, São Paulo"
  }
}
```
**Response:** `404 Not Found`
```json
{ "mensagem": "Cliente não encontrado." }
```
 
---
 
#### `GET /api/Clientes` — Listar todos os clientes
**Response:** `200 OK` — array de clientes (PF e PJ)
 
---
 
#### `PUT /api/Clientes/pf/{id}` — Atualizar Pessoa Física
**Request:**
```json
{
  "idCliente": 1,
  "nmCliente": "Renan Dias Atualizado",
  "idConcessionaria": 1,
  "cpf": "12345678901",
  "dataNascimento": "2001-10-10"
}
```
**Response:** `204 No Content`
 
**Response:** `404 Not Found` — ID de PJ passado no PUT de PF
```json
{ "mensagem": "Cliente PF não encontrado." }
```
 
---
 
#### `PUT /api/Clientes/pj/{id}` — Atualizar Pessoa Jurídica
**Request:**
```json
{
  "idCliente": 2,
  "nmCliente": "Empresa Teste Atualizada LTDA",
  "idConcessionaria": 1,
  "cnpj": "12345678000195",
  "razaoSocial": "Empresa Teste Atualizada Ltda"
}
```
**Response:** `204 No Content`
 
---
 
#### `DELETE /api/Clientes/pf/{id}` — Deletar Pessoa Física
**Response:** `204 No Content`
 
#### `DELETE /api/Clientes/pj/{id}` — Deletar Pessoa Jurídica
**Response:** `204 No Content`
 
---
 
### Financiamentos
 
#### `POST /api/Financiamentos` — Cadastrar financiamento
> O campo `valorParcela` é calculado automaticamente pela fórmula Price sobre `(valorVeiculo - valorEntrada)`. O score é avaliado automaticamente com base no percentual de entrada.
 
**Request:**
```json
{
  "nmProduto": "Financiamento HB20",
  "valorVeiculo": 80000,
  "valorEntrada": 30000,
  "taxaJuros": 1.5,
  "prazoMeses": 48,
  "valorParcela": 0
}
```
**Response:** `201 Created` — score APROVADO
```json
{
  "financiamento": {
    "idProduto": 1,
    "nmProduto": "Financiamento HB20",
    "valorVeiculo": 80000.00,
    "valorEntrada": 30000.00,
    "taxaJuros": 1.50,
    "prazoMeses": 48,
    "valorParcela": 1468.75
  },
  "score": "APROVADO",
  "mensagem": "Financiamento criado com sucesso."
}
```
**Response:** `201 Created` — score ANALISE
```json
{
  "financiamento": { "..." },
  "score": "ANALISE",
  "mensagem": "Financiamento criado com sucesso."
}
```
**Response:** `400 Bad Request` — score REPROVADO
```json
{
  "mensagem": "Financiamento reprovado. Entrada mínima de 10% do valor do veículo.",
  "score": "REPROVADO"
}
```
 
---
 
#### `GET /api/Financiamentos/{id}` — Buscar financiamento por ID
**Response:** `200 OK`
 
**Response:** `404 Not Found`
```json
{ "mensagem": "Financiamento não encontrado." }
```
 
---
 
#### `GET /api/Financiamentos` — Listar todos os financiamentos
**Response:** `200 OK` — array de financiamentos
 
---
 
#### `PUT /api/Financiamentos/{id}` — Atualizar financiamento
> O `valorParcela` é recalculado automaticamente. Se o score for `REPROVADO`, retorna `400` sem salvar.
 
**Request:**
```json
{
  "idProduto": 1,
  "nmProduto": "Financiamento HB20 Atualizado",
  "valorVeiculo": 80000,
  "valorEntrada": 25000,
  "taxaJuros": 1.2,
  "prazoMeses": 36,
  "valorParcela": 0
}
```
**Response:** `204 No Content`
 
**Response:** `400 Bad Request` — score REPROVADO no PUT
```json
{
  "mensagem": "Financiamento reprovado. Entrada mínima de 10% do valor do veículo.",
  "score": "REPROVADO"
}
```
 
---
 
#### `DELETE /api/Financiamentos/{id}` — Deletar financiamento
**Response:** `204 No Content`
 
---
 
### Contratações
 
#### `POST /api/Contratacoes` — Solicitar contratação
**Request:**
```json
{
  "idCliente": 1,
  "idProduto": 1
}
```
**Response:** `202 Accepted`
```json
{
  "idContratacao": 1,
  "idCliente": 1,
  "cliente": {
    "idCliente": 1,
    "nmCliente": "Renan Dias",
    "idConcessionaria": 1,
    "concessionaria": { "..." }
  },
  "idProduto": 1,
  "produto": { "idProduto": 1, "nmProduto": "Financiamento HB20" },
  "status": "PENDENTE",
  "dtSolicitacao": "2026-05-20T02:04:38"
}
```
**Response:** `404 Not Found` — cliente inexistente
```json
{ "mensagem": "Cliente não encontrado." }
```
**Response:** `404 Not Found` — produto inexistente
```json
{ "mensagem": "Produto não encontrado." }
```
 
---
 
#### `GET /api/Contratacoes/{id}` — Consultar status da contratação
**Response:** `200 OK`
```json
{
  "idContratacao": 1,
  "idCliente": 1,
  "cliente": {
    "idCliente": 1,
    "nmCliente": "Renan Dias",
    "idConcessionaria": 1,
    "concessionaria": {
      "idConcessionaria": 1,
      "nmConcessionaria": "Concessionária Centro SP",
      "cep": "01310100",
      "dsEndereco": "Av. Paulista, 1000 - Bela Vista, São Paulo"
    }
  },
  "idProduto": 1,
  "produto": { "idProduto": 1, "nmProduto": "Financiamento HB20" },
  "status": "PENDENTE",
  "dtSolicitacao": "2026-05-20T02:04:38"
}
```
**Response:** `404 Not Found`
```json
{ "mensagem": "Contratação não encontrada." }
```
 
---
 
#### `GET /api/Contratacoes` — Listar todas as contratações
**Response:** `200 OK` — array de contratações com cliente, concessionária e produto completos
 
---
 
#### `PUT /api/Contratacoes/{id}` — Atualizar status da contratação
**Request:**
```json
{
  "idContratacao": 1,
  "idCliente": 1,
  "idProduto": 1,
  "status": "APROVADO",
  "dtSolicitacao": "2026-05-20T00:00:00"
}
```
**Response:** `204 No Content`
 
---
 
#### `DELETE /api/Contratacoes/{id}` — Deletar contratação
**Response:** `204 No Content`
 
---
 
### Ordem recomendada para testes
 
Para evitar erros de chave estrangeira, siga esta ordem:
 
**Cadastro:** `Concessionária` → `Financiamento` → `Cliente (PF ou PJ)` → `Contratação`
 
**Deleção:** `Contratação` → `Cliente` → `Financiamento` → `Concessionária`
 
> A coleção completa do Postman com todos os endpoints está disponível em `docs/postman/CP3ConcessionariaAPI.postman_collection.json`
 
> Os scripts SQL de consulta e reset estão disponíveis em `docs/sql/CP3_consultas.sql`
 
---
