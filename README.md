# CP3ConcessionariaAPI - Backend Concessionária

Backend de uma concessionária digital desenvolvido em **ASP.NET Core Web API (.NET 8)** com **Entity Framework Core** e **Oracle Database**. Projeto avaliativo da disciplina de C# Software Development - FIAP - 3ESPW 2026.

O sistema permite cadastrar clientes (pessoa física ou jurídica), vinculá-los a uma concessionária e realizar a contratação de produtos automotivos. O produto implementado é o **Financiamento de Veículos**, com cálculo automático de parcelas pela fórmula Price e avaliação de score de crédito baseada no percentual de entrada — simulando um fluxo real de aprovação de crédito em uma concessionária digital.

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

## 7. Testes
 
> **Nota:** Testes automatizados com `dotnet test` não foram implementados neste checkpoint, pois este conteúdo não foi trabalhado em aula até a data de entrega.
 
Os fluxos críticos foram validados manualmente via **Swagger UI** e **Postman**, cobrindo todos os cenários obrigatórios:
 
| Fluxo Crítico | Validação | Status |
|---|---|---|
| Cadastro PF com CPF duplicado | Retorna `400` com mensagem | Correto |
| Cadastro PJ com CNPJ duplicado | Retorna `400` com mensagem | Correto |
| Vincular cliente a concessionária inexistente | Retorna `400` com mensagem | Correto |
| Contratação válida | Retorna `202 Accepted` com status `PENDENTE` | Correto |
| Contratação com cliente inexistente | Retorna `404` com mensagem | Correto |
| Consulta de status da contratação | Retorna contratação completa com cliente e produto | Correto |
| Financiamento reprovado por score | Retorna `400` com score `REPROVADO` | Correto |
| Cálculo automático de parcela (fórmula Price) | `valorParcela` calculado automaticamente | Correto |
| Score ANALISE (entrada entre 10% e 29%) | Retorna `201` com score `ANALISE` | Correto |
| PUT de financiamento reprovado | Retorna `400`, dado anterior mantido no banco | Correto |
 
---
 
## 8. Mensageria (RabbitMQ)
 
> **Nota:** A implementação de filas com RabbitMQ não foi trabalhada em aula até a data de entrega deste checkpoint e não faz parte do escopo avaliado.
 
---

## 9. Evidências de Funcionamento (Swagger)
 
### Endpoints
 
<img width="1918" height="1026" alt="Print_Swagger_1" src="https://github.com/user-attachments/assets/23d762a1-a174-4108-931a-bbdc581494be" />

<img width="1915" height="946" alt="Print_Swagger2" src="https://github.com/user-attachments/assets/f563bf50-f036-44ad-8ca8-36dfc0029588" />

---
 
Segue a ordem de dos principais testes realizados no Swagger:
 
### Concessionarias
 
---
 
**01 - Cadastrar Concessionaria (POST)**
 
<img width="1147" height="892" alt="01-Cadastrar_Concessionaria" src="https://github.com/user-attachments/assets/a6baba4e-b4d8-40e3-95b3-7db90942e8a3" />
 
---
 
**02 - Buscar Concessionaria por ID (GET)**
 
<img width="1143" height="622" alt="02-Buscar_Concessionaria_ID" src="https://github.com/user-attachments/assets/7c060799-ca22-4b36-81fe-be79003cf901" />

---
 
**03 - Buscar Concessionaria ID Invalido - 404 (GET)**
 
<img width="1150" height="622" alt="03-Buscar_Concessionaria_ID_Invalido" src="https://github.com/user-attachments/assets/734df7e0-a76b-45a9-aa38-df3daaabd64a" />

---
 
### Clientes
 
---
 
**04 - Cadastrar PessoaFisica (POST)**
 
<img width="1075" height="917" alt="04-Cadastrar_PF" src="https://github.com/user-attachments/assets/dd2b5bf2-503d-4ebf-9d48-2d4a82de5486" />
 
---
 
**05 - Cadastrar PF - CPF Duplicado - 400 (POST)**
 
<img width="1076" height="843" alt="05-Cadastrar_PF_CPF_DUPLICADO" src="https://github.com/user-attachments/assets/02ce0761-0dbc-4cab-b308-63030a040e51" />
 
---
 
**06 - Cadastrar PessoaJuridica (POST)**
 
<img width="1076" height="917" alt="06-Cadastrar_PJ" src="https://github.com/user-attachments/assets/70c6f3f8-5d92-4fd3-b708-424c0017e92b" />
 
---
 
**07 - Cadastrar PJ - CNPJ Duplicado - 400 (POST)**
 
<img width="1147" height="902" alt="07-Cadastrar_PJ_CNPJ_DUPLICADO" src="https://github.com/user-attachments/assets/80a5a0cb-75cc-4f43-ac9e-38e1fb5f42c1" />
 
---
 
**08 - Cadastrar PF - Concessionaria Inexistente - 400 (POST)**
 
<img width="1146" height="903" alt="08-Cadastrar_PF_CONCESSIONARIA_INEXISTENTE" src="https://github.com/user-attachments/assets/6bfc2295-9548-4889-95fb-c8ed37c6d760" />
 
---
 
**09 - Buscar Cliente por ID (GET)**
 
<img width="1296" height="887" alt="09-Buscar_Cliente_ID" src="https://github.com/user-attachments/assets/27232b79-4ffc-47d7-a672-945d911ed1d6" />
 
---
 
**10 - Buscar Cliente ID Invalido - 404 (GET)**
 
<img width="1300" height="790" alt="10-Buscar_Cliente_ID_INVALIDO" src="https://github.com/user-attachments/assets/90c02554-35d6-4c80-8194-7b7a0ebae025" />
 
---
 
### Financiamentos
 
---
 
**11 - Cadastrar Financiamento - APROVADO (POST)**
 
<img width="1073" height="936" alt="11-Cadastrar_Financiamento_APROVADO" src="https://github.com/user-attachments/assets/e3013026-7a31-4c83-b35c-958a5ec1381c" />
 
---
 
**12 - Cadastrar Financiamento - ANALISE (POST)**
 
<img width="1075" height="940" alt="12-Cadatrar_Financiamento_ANALISE" src="https://github.com/user-attachments/assets/be1a564c-26a1-41d3-8933-2b67f77061c8" />
 
---
 
**13 - Cadastrar Financiamento - REPROVADO - 400 (POST)**
 
<img width="1147" height="927" alt="13-Cadastrar_Financiamento_REPROVADO" src="https://github.com/user-attachments/assets/f7f02240-2f0d-4b1c-8860-b894aea55a8c" />
 
---
 
**14 - Buscar Financiamento por ID (GET)**
 
<img width="1152" height="657" alt="14-Buscar_Financiamento_ID" src="https://github.com/user-attachments/assets/b6d67200-cbd0-4a63-859e-e51b30c0f2cd" />
 
---
 
**15 - Buscar Financiamento ID Invalido - 404 (GET)**
 
<img width="1437" height="782" alt="15-Buscar_Financiamento_ID_INVALIDO" src="https://github.com/user-attachments/assets/f4b16a9f-0386-4648-8ec9-a8afde29140d" />

---
 
### Contratacoes
 
---
 
**16 - Solicitar Contratacao - 202 (POST)**
 
<img width="1076" height="937" alt="16-Criar_Contratacao" src="https://github.com/user-attachments/assets/2cbbaa00-17d1-4225-aa31-f5275d73a3bb" />

---
 
**17 - Contratar - Cliente Inexistente - 404 (POST)**
 
<img width="1147" height="881" alt="17-Criar_Contratacao_CLIENTE_INVALIDO" src="https://github.com/user-attachments/assets/7ed96fa1-28a5-4c2a-b41f-cf9f362289f8" />
 
---
 
**18 - Contratar - Produto Inexistente - 404 (POST)**
 
<img width="1146" height="881" alt="18-Criar_Contratacao_PRODUTO_INVALIDO" src="https://github.com/user-attachments/assets/512ecbd1-92fc-416e-97de-1638183a55c2" />

---
 
**19 - Buscar Contratacao por ID (GET)**
 
<img width="1147" height="800" alt="19-Buscar_Contratacoes_ID" src="https://github.com/user-attachments/assets/9aa80de2-aedf-4fed-bec6-77578d27f2bd" />
 
---
 
**20 - Buscar Contratacao ID Invalido - 404 (GET)**
 
<img width="1573" height="858" alt="20-Buscar_Contratacoes_ID_INVALIDO" src="https://github.com/user-attachments/assets/fcb756d8-13c4-4ea5-abc5-e6bf6405accd" />
 
---
 
### Concessionarias
 
---
 
**21 - Listar Todas as Concessionarias (GET)**
 
 <img width="1583" height="917" alt="21-Listar_TODAS_Concessionarias" src="https://github.com/user-attachments/assets/b111f3ff-a43e-4a64-aada-946227ccfe59" />

---
 
**22 - Atualizar Concessionaria (PUT)**
 
<img width="1076" height="891" alt="22-Atualizar_Concessionaria" src="https://github.com/user-attachments/assets/26e4334e-2780-4882-a98d-0dddf4fe06bb" />

---
 
### Clientes
 
---
 
**23 - Listar Todos os Clientes (GET)**
 
<img width="1293" height="890" alt="23-Listar_TODOS_Clientes" src="https://github.com/user-attachments/assets/38075d75-1a6e-49b6-a123-047277889ade" />
 
---
 
**24 - Atualizar PessoaFisica (PUT)**
 
<img width="1077" height="902" alt="24-Atualizar_PF" src="https://github.com/user-attachments/assets/2176b087-2b0e-43a9-9f2c-b1010c4a638c" />

---
 
**25 - Atualizar PessoaJuridica (PUT)**
 
<img width="1077" height="901" alt="25-Atualizar_PJ" src="https://github.com/user-attachments/assets/8b5b6ad9-bac7-47ec-a910-8e21d9de269a" />

---
 
### Financiamentos
 
---
 
**26 - Listar Todos os Financiamentos (GET)**
 
<img width="1077" height="677" alt="26-Listar_TODOS_Financiamentos" src="https://github.com/user-attachments/assets/d62fc7e2-d804-4c21-8284-41187c5860a1" />
 
---
 
**27 - Atualizar Financiamento (PUT)**
 
<img width="1073" height="917" alt="27-Atualizar_Financiamentos" src="https://github.com/user-attachments/assets/f75b7eae-c3ff-4005-84df-2b920d3e2911" />
 
---
 
### Contratacoes
 
---
 
**28 - Listar Todas as Contratacoes (GET)**
 
<img width="1290" height="891" alt="28-Listar_TODAS_Contratacoes" src="https://github.com/user-attachments/assets/fa67ee0b-19d9-41f1-a983-016e8e490f4f" />
 
---
 
**29 - Atualizar Status Contratacao - APROVADO (PUT)**
 
<img width="1077" height="901" alt="29-Atualizar_Concessionarias" src="https://github.com/user-attachments/assets/55f4a920-b4fb-4808-aad5-3fd0e91365a3" />
 
---
 
### Delete
 
Para deletar, é recomendado remover nessa ordem:
 
**Contratacao → Financiamento → Clientes → Concessionaria**
 
> Delete a contratação antes do cliente e do financiamento, senão vai dar erro de FK!
 
---
 
**30 - Deletar Contratacao (DELETE)**
 
<img width="1076" height="552" alt="30-Deletar_Contratacoes" src="https://github.com/user-attachments/assets/559c48c7-1137-416d-8dfc-5d534201a418" />
 
---
 
**31 - Deletar Financiamento (DELETE)**
 
<img width="1073" height="555" alt="31-Deletar_Financiamento" src="https://github.com/user-attachments/assets/d9e0f7e9-c033-4847-a186-d530bc729950" />
 
---
 
**32 - Deletar PessoaFisica (DELETE)**
 
<img width="1077" height="547" alt="32-Deletar_PF" src="https://github.com/user-attachments/assets/2a361191-ec7c-491d-8238-b32063035886" />
 
---
 
**33 - Deletar PessoaJuridica (DELETE)**
 
<img width="1073" height="548" alt="33-Deletar_PJ" src="https://github.com/user-attachments/assets/ea7d7a49-b100-4aad-8259-f47f5ba7c604" />
 
---
 
**34 - Deletar Concessionaria (DELETE)**
 
<img width="1295" height="667" alt="34-Deletar_Concessionaria" src="https://github.com/user-attachments/assets/2c9c1e21-794d-4b0a-86ea-0c45894b9fa8" />
 
---
 
> As requisições estão disponíveis no Swagger (F5) e no Postman (`docs/postman/`)
 
---

## Banco de Dados (Oracle SQL Developer)

> Prints das tabelas criadas e dos scripts funcionando
> 
> Esses scripts foram executados após concluirmos do item 1 ao 29 (com exceção dos Deletes)

<img width="912" height="597" alt="Scripts_SQL_E_TABELAS_CRIADAS" src="https://github.com/user-attachments/assets/5afe0d15-6739-4aa4-bc34-7bb441382f21" />

<img width="1623" height="457" alt="Scripts_SQL_Resultados" src="https://github.com/user-attachments/assets/d80cdf1f-ea4a-46ba-86fe-bbf2dad69df4" />

<img width="1875" height="306" alt="Sripts_SQL_Resultados_2" src="https://github.com/user-attachments/assets/2157f737-87e2-4490-a071-aa6c5748017e" />

<img width="1457" height="122" alt="Sripts_SQL_Resultados_3" src="https://github.com/user-attachments/assets/59dfa119-1c9e-44d6-ab46-56e7a37f7a32" />

---
 
## Arquitetura do Projeto
 
```
CP3ConcessionariaAPI/
│
├── Controllers/
│   ├── ConcessionariasController.cs
│   ├── ClientesController.cs
│   ├── ContratacoesController.cs
│   └── FinanciamentosController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Migrations/
│   └── (gerado automaticamente pelo EF Core)
│
├── Models/
│   ├── Concessionaria.cs
│   ├── Cliente.cs              ← abstract
│   ├── PessoaFisica.cs         ← herda Cliente
│   ├── PessoaJuridica.cs       ← herda Cliente
│   ├── Produto.cs              ← abstract
│   ├── Financiamento.cs        ← herda Produto
│   └── Contratacao.cs
│
├── Services/
│   └── FinanciamentoService.cs
│
├── docs/
│   ├── diagram/
│   │   ├── Diagrama_CP3_Concessionaria_558540_C#.drawio
│   │   └── Diagrama_CP3_Concessionaria_558540_C#.png
│   ├── evidence/
│   │   ├── (prints do Swagger)
│   │   ├── (prints dos testes - 01 ao 34)
│   │   └── (prints do banco de dados Oracle)
│   ├── postman/
│   │   └── CP3ConcessionariaAPI.postman_collection.json
│   ├── sql/
│   │   └── CP3_consultas.sql
│   └── CP3ConcessionariaAPI-Evidencias.pdf
│
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── README.md
```
 
---
 
## Stack Utilizada
 
| Camada | Tecnologia |
|---|---|
| Runtime | .NET 8.0 |
| API | ASP.NET Core Web API |
| ORM | Entity Framework Core 9.0 |
| Banco de Dados | Oracle (oracle.fiap.com.br:1521/ORCL) |
| Documentação | Swagger / OpenAPI |
| Testes manuais | Postman + Swagger UI |
