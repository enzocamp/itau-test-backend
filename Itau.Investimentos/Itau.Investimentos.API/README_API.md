# Itau.Investimentos.API

Esta é a camada de **API RESTful** do projeto **Itaú Investimentos**, responsável por expor os recursos relacionados a usuários, ativos, operações (trades), cotações e posições de investimentos.

---

## 🔧 Requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- [MySQL 8.x](https://hub.docker.com/_/mysql) (ou usar via Docker Compose com banco `itau_investments`)

---

## 🚀 Executando a API localmente

1. Clone o repositório:

   ```bash
   git clone https://github.com/enzocamp/itau-test-backend.git
   cd itau-test-backend
   ```

2. Suba o banco de dados com Docker Compose (caso não esteja rodando localmente):

   ```bash
   docker-compose up -d
   ```

3. Navegue até o projeto da API:

   ```bash
   cd Itau.Investimentos/Itau.Investimentos.API
   ```

4. Execute o projeto:

   ```bash
   dotnet run
   ```

5. Acesse a documentação Swagger:

   ```
   http://localhost:5000/swagger
   ```

---

## 📌 Endpoints disponíveis

### 📁 `api/users`
- `POST /` - Criação de usuário
- `GET /{id}` - Buscar usuário por ID
- `GET /` - Listar todos os usuários
- `PUT /{id}` - Atualizar usuário
- `DELETE /{id}` - Remover usuário

### 📁 `api/assets`
- `POST /` - Cadastrar ativo
- `GET /{id}` - Buscar ativo por ID
- `GET /` - Listar todos os ativos
- `PUT /{id}` - Atualizar ativo
- `DELETE /{id}` - Remover ativo

### 📁 `api/trades`
- `POST /` - Registrar uma operação
- `GET /{id}` - Buscar operação por ID
- `GET /` - Listar todas as operações
- `PUT /{id}` - Atualizar operação
- `DELETE /{id}` - Excluir operação
- `GET /filter?userId=1&assetId=2` - Buscar operações do usuário + ativo nos últimos 30 dias

### 📁 `api/quotes`
- `POST /` - Registrar cotação
- `GET /{id}` - Buscar cotação por ID
- `GET /?assetId=1` - Listar cotações de um ativo
- `GET /last?assetId=1` - Buscar última cotação de um ativo

### 📁 `api/position`
- `GET /{userId}/{assetId}` - Retornar posição calculada (quantidade, preço médio, P&L)

---

## ⚙️ Observações técnicas

- Datas são registradas com `DateTime.UtcNow`.
- Os enums como `TradeType` são salvos no banco como texto (`BUY`/`SELL`) com `EnumToStringConverter`.
- Padrão de nomenclatura: **snake_case** no banco e **PascalCase** nas entidades .NET.
- Todas as operações de escrita/leitura seguem padrão assíncrono (`async/await`).
- Tratamento de exceções centralizado com `DbOperationException`.

---

## 👨‍💻 Desenvolvedor

- Nome: Enzo Campolongo
- GitHub: [@enzocamp](https://github.com/enzocamp)
