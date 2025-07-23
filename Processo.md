# Guia de Construção do Projeto: Sistema de Relatórios Assíncronos

## Prefácio: O Objetivo do Projeto

Este documento detalha a construção de um sistema back-end em .NET, cujo propósito principal é servir como um laboratório prático para dominar três pilares da engenharia de software moderna:

1.  **Containerização com Docker:** Isolar e gerenciar todos os serviços da aplicação (API, Banco de Dados, etc.) para criar um ambiente de desenvolvimento robusto e portátil.
2.  **Testes Automatizados:** Garantir a qualidade e a manutenibilidade do código através de testes de unidade e integração.
3.  **Sistemas de Mensageria (RabbitMQ):** Projetar uma arquitetura desacoplada e resiliente para lidar com tarefas assíncronas de longa duração.

O projeto consiste em uma API que recebe pedidos para gerar relatórios, enfileira esses pedidos e os processa em segundo plano, seguindo os princípios da **Clean Architecture**.

---

## Capítulo 1: Preparando o Terreno (Configuração do Ambiente)

O objetivo desta fase foi montar um ambiente de desenvolvimento completo e funcional, com todas as ferramentas necessárias configuradas para trabalharem em harmonia.

### 1.1. Ferramentas Essenciais

- **Visual Studio 2022:** Nossa IDE principal para o desenvolvimento .NET.
- **Docker Desktop:** A plataforma para criar e gerenciar nossos contêineres.
- **Visual Studio Code:** Usado como editor de apoio e cliente de banco de dados.

### 1.2. Configuração do Visual Studio 2022

A instalação foi customizada para incluir as **Cargas de Trabalho** essenciais para o projeto:
- `ASP.NET e desenvolvimento da Web`
- `Desenvolvimento para área de trabalho .NET`
- `Ferramentas de desenvolvimento de contêineres`

### 1.3. Configuração do Docker Desktop

A instalação foi feita utilizando a integração com **WSL 2 (Windows Subsystem for Linux)**, que é a abordagem moderna e performática para rodar Docker no Windows.

### 1.4. O Teste de "Hello World" Integrado

Para validar que todas as ferramentas estavam se comunicando, realizamos um teste crucial:
1. **Criação:** Uma nova API Web foi criada no Visual Studio com a opção "Habilitar Docker" marcada.
2. **Execução:** A aplicação foi iniciada com o perfil "Docker" no Visual Studio.
3. **Resultado:** O Visual Studio comandou o Docker Desktop para construir a imagem da API, iniciar um contêiner e abrir o Swagger no navegador, provando que a integração funcionava.

---

## Capítulo 2: A Fundação Arquitetural (Clean Architecture)

O objetivo desta fase foi criar o esqueleto da solução, seguindo as regras da Clean Architecture para garantir a separação de concerns.

### 2.1. Estrutura da Solução

Uma solução em branco foi criada, organizada com as seguintes pastas e projetos:

- **Solução `ReportSystem`**
  - **Pasta `Core`** (O coração do negócio, sem dependências externas)
    - `ReportSystem.Domain` (Biblioteca de Classes)
    - `ReportSystem.Application` (Biblioteca de Classes)
  - **Pasta `Infrastructure`** (Os detalhes de implementação)
    - `ReportSystem.Infrastructure` (Biblioteca de Classes)
  - **Pasta `Presentation`** (A camada de entrada)
    - `ReportSystem.API` (API Web do ASP.NET Core)
    - `ReportSystem.Worker` (Serviço de Trabalho)

### 2.2. Regra de Dependências

As referências entre os projetos foram configuradas para seguir a regra de dependência (sempre apontando para dentro), garantindo o desacoplamento.

### 2.3. O Coração do Domínio

A primeira entidade, **`ReportRequest`**, foi criada no projeto `Domain`, aplicando princípios de **Modelo de Domínio Rico**: encapsulamento com `private set`, um método de fábrica `Create()` para garantir a consistência e métodos de comportamento para transições de estado.

---

## Capítulo 3: Implementando a Persistência de Dados

O objetivo foi implementar a lógica para salvar e buscar nossos dados, usando o Entity Framework Core de forma desacoplada.

### 3.1. O Contrato do Repositório

Na camada de `Application`, foi definida a interface **`IReportRequestRepository`**. Este contrato define as operações de dados necessárias, escondendo os detalhes da implementação.

### 3.2. Configurando o EF Core na Infraestrutura

No projeto `Infrastructure`, foram instalados os pacotes NuGet necessários (`SqlServer`, `Tools`, `Design`). Foi criado o `AppDbContext` e a configuração da entidade `ReportRequest` foi feita usando **Fluent API** em uma classe de configuração separada, mantendo o Domínio limpo.

### 3.3. A Implementação Concreta

A classe **`ReportRequestRepository`** foi criada, implementando a interface `IReportRequestRepository` e recebendo o `AppDbContext` via injeção de dependência.

---

## Capítulo 4: Orquestrando o Ambiente com Docker Compose

O objetivo desta fase foi criar um ambiente de desenvolvimento completo e executável com um único comando.

### 4.1. A "Planta do Condomínio" (`docker-compose.yml`)

Um arquivo `docker-compose.yml` foi criado na raiz da solução, definindo dois serviços: `db` (SQL Server) e `api` (ASP.NET Core).

### 4.2. O `Dockerfile` da API

Usando a funcionalidade do Visual Studio (`Adicionar > Suporte a Docker`), um `Dockerfile` foi gerado para o projeto `ReportSystem.API`.

### 4.3. Levantando o Ambiente e Resolvendo Problemas

O ambiente foi iniciado pela primeira vez com o comando:
```shell
docker-compose up --build
```
Durante este processo, vários problemas do mundo real foram diagnosticados e resolvidos:
- **Erro de Codificação:** O `docker-compose.yml` foi salvo com a codificação correta **UTF-8 (sem BOM)**.
- **Erro de Certificado HTTPS:** O erro `Unable to configure HTTPS endpoint` foi resolvido desabilitando o redirecionamento HTTPS na API.

### 4.4. Materializando o Banco de Dados

Com os contêineres rodando, a migração do banco de dados foi aplicada.
1. **O Problema:** A execução inicial do `dotnet ef database update` falhou, pois a ferramenta (rodando no host) não conseguia encontrar o servidor `db` (que só existe na rede Docker).
2. **A Solução:** O arquivo `appsettings.Development.json` foi configurado com uma `ConnectionString` que aponta para `Server=localhost,1433`.
3. **O Comando Final:** O banco foi criado com sucesso usando o comando:
   ```shell
   dotnet ef database update --project ReportSystem.Infrastructure --startup-project ReportSystem.API
   ```

Ao final deste capítulo, tínhamos um ambiente totalmente funcional e containerizado, com a API rodando ao lado de um banco de dados SQL Server com a estrutura de tabelas correta.