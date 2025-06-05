
# Projeto `Itau.Investimentos.Domain`

Este projeto representa a camada de domínio da aplicação de investimentos. Ele contém as **entidades centrais**, **lógicas de negócio puras** e **tipos básicos** utilizados em toda a solução.

## 🎯 Objetivo

A camada de domínio é o coração do sistema e deve se manter **independente de tecnologias externas**. Aqui não há dependência de frameworks como Entity Framework, Dapper, Kafka, etc.

Tudo nesta camada representa **o modelo de negócio** da corretora de investimentos.

## 📦 Conteúdo

- Entidades: `User`, `Asset`, `Trade`, `Quote`, `Position`
- Enumeração: `TradeType` (`BUY`, `SELL`)
- Regras e propriedades essenciais do negócio

## 📘 Tradução de tabelas e entidades

| Tabela / Entidade | Significado em português         |
|-------------------|----------------------------------|
| `users`           | Usuários                         |
| `assets`          | Ativos financeiros (ações, FIIs) |
| `trades`          | Operações (compra e venda)       |
| `quotes`          | Cotações                         |
| `positions`       | Posições de investimento         |

## 🧾 Tradução de campos principais

| Campo           | Significado                       |
|-----------------|-----------------------------------|
| `unit_price`    | Preço unitário do ativo           |
| `fee`           | Corretagem (valor em R$)          |
| `fee_percentage`| Percentual de corretagem (%)      |
| `trade_type`    | Tipo de operação (compra/venda)   |
| `pnl`           | Lucro ou prejuízo (Profit & Loss) |
| `executed_at`   | Data/hora da operação             |
| `quoted_at`     | Data/hora da cotação              |
| `average_price` | Preço médio ponderado             |

## 🧱 Padrões adotados

- **Clean Architecture**
- **Domain-Driven Design (DDD)**
- Código 100% em **inglês**
- Convenções de nome em **`camelCase` / `PascalCase`** no código e **`snake_case`** no banco

## ✅ Independência

Este projeto **não possui dependências externas** e deve se manter assim. Toda lógica implementada aqui pode ser testada e utilizada por qualquer interface (API, Worker, etc).
