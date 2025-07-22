# Report System - Sistema de Geração de Relatórios Assíncronos

Este repositório contém um projeto em desenvolvimento de um sistema back-end para geração de relatórios, construído com .NET e C#, com foco em boas práticas de arquitetura e tecnologias modernas.

## 🚀 Visão Geral do Projeto

A aplicação consiste em uma API que recebe solicitações para gerar relatórios complexos. Para não bloquear o usuário, essas solicitações são processadas de forma assíncrona. A API aceita o pedido, o enfileira e o processa em segundo plano através de um serviço de trabalho (Worker Service). O usuário pode consultar o status do seu relatório e fazer o download quando o processamento estiver concluído.

## 🎯 Propósito Principal: Um Laboratório de Aprendizagem

Mais do que um projeto final, este sistema foi concebido como um **laboratório prático** para aprofundar e solidificar conhecimentos em tecnologias e arquiteturas essenciais para o desenvolvimento back-end.

O foco principal do desenvolvimento está na aplicação e no domínio dos seguintes pilares:

1.  **🐳 Docker & Docker Compose:** Containerizar todos os serviços da aplicação (API, Worker, Banco de Dados, Fila de Mensagens) para criar um ambiente de desenvolvimento e produção consistente, portátil e isolado.

2.  **🧪 Testes Automatizados (com xUnit):** Implementar uma suíte de testes robusta, cobrindo testes de unidade para a lógica de negócio e testes de integração para garantir a correta comunicação entre as diferentes camadas e serviços da arquitetura.

3.  **🐇 Mensageria com RabbitMQ:** Utilizar um message broker para desacoplar a solicitação do processamento, permitindo que o sistema seja mais resiliente, escalável e responsivo.

## 🛠️ Arquitetura e Tecnologias

*   **Linguagem:** C#
*   **Framework:** .NET 8
*   **Arquitetura:** Clean Architecture
*   **Comunicação API:** REST com ASP.NET Core
*   **Persistência:** Entity Framework Core com SQL Server
*   **Autenticação:** JWT (JSON Web Tokens)
*   **Mensageria:** RabbitMQ
*   **Testes:** xUnit
*   **Containerização:** Docker

---

## 🏃 Como Executar o Projeto

Para executar este projeto em seu ambiente local, você precisará ter o **.NET 8 SDK** e o **Docker Desktop** instalados e em execução.

### 1. Configuração Inicial

Antes de executar, é necessário configurar a senha do banco de dados.

1.  Abra o arquivo `docker-compose.yml` e defina uma senha forte para o usuário `sa` na variável de ambiente `SA_PASSWORD` do serviço `db`.
2.  Abra o arquivo `ReportSystem.API/appsettings.json` e insira a **mesma senha** no campo `Password` da `DefaultConnection`.
3.  Abra o arquivo `ReportSystem.API/appsettings.Development.json` e faça o mesmo.

### 2. Subindo os Contêineres

Com o Docker Desktop rodando, abra um terminal na pasta raiz do projeto (onde o arquivo `docker-compose.yml` está localizado) e execute o seguinte comando:

```shell
docker-compose up --build
```

Este comando irá:
- Baixar a imagem do SQL Server.
- Construir a imagem da API.
- Iniciar os contêineres do banco de dados e da API.

Aguarde até que os logs se estabilizem e você veja a mensagem `Now listening on: http://[::]:80` vinda da API.

### 3. Aplicando a Migração do Banco de Dados

Com os contêineres em execução, o banco de dados está online, mas ainda vazio. Precisamos criar a estrutura de tabelas.

Abra um **novo terminal** (mantenha o primeiro rodando!) e execute o seguinte comando:

```shell
dotnet ef database update --project ReportSystem.Infrastructure --startup-project ReportSystem.API
```

Este comando usará a `ConnectionString` do `appsettings.Development.json` para se conectar ao contêiner do banco de dados e aplicar todas as migrações pendentes.

### 4. Verificando a Aplicação

Se tudo correu bem, o ambiente está pronto!

- A API estará acessível em `http://localhost:8080`.
- A documentação da API (Swagger UI) pode ser acessada em `http://localhost:8080/swagger`.
- Você pode se conectar ao banco de dados usando seu cliente SQL preferido com as seguintes credenciais:
  - **Servidor:** `localhost,1433`
  - **Usuário:** `sa`
  - **Senha:** A senha que você configurou.

## Status Atual

O ambiente Docker está configurado e a camada de persistência inicial foi estabelecida. As próximas etapas envolvem a implementação dos casos de uso e a criação dos endpoints na API.