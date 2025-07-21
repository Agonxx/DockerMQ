# Report System - Sistema de Geração de Relatórios Assíncronos

Este repositório contém um projeto em desenvolvimento de um sistema back-end para geração de relatórios, construído com .NET e C#.

## Visão Geral do Projeto

A aplicação consiste em uma API que recebe solicitações para gerar relatórios complexos. Para não bloquear o usuário, essas solicitações são processadas de forma assíncrona. A API aceita o pedido, o enfileira e o processa em segundo plano através de um serviço de trabalho (Worker Service). O usuário pode consultar o status do seu relatório e fazer o download quando o processamento estiver concluído.

## 🎯 Propósito Principal: Um Laboratório de Aprendizagem

Mais do que um projeto final, este sistema foi concebido como um **laboratório prático** para aprofundar e solidificar conhecimentos em tecnologias e arquiteturas modernas essenciais para o desenvolvimento back-end.

O foco principal do desenvolvimento está na aplicação e no domínio dos seguintes pilares:

1.  **🐳 Docker & Docker Compose:** Containerizar todos os serviços da aplicação (API, Worker, Banco de Dados, Fila de Mensagens) para criar um ambiente de desenvolvimento e produção consistente, portátil e isolado.

2.  **🧪 Testes Automatizados (com xUnit):** Implementar uma suíte de testes robusta, cobrindo testes de unidade para a lógica de negócio e testes de integração para garantir a correta comunicação entre as diferentes camadas e serviços da arquitetura.

3.  **🐇 Mensageria com RabbitMQ:** Utilizar um message broker para desacoplar a solicitação do processamento, permitindo que o sistema seja mais resiliente, escalável e responsivo.

## 🏛️ Arquitetura e Tecnologias

O projeto está sendo construído sobre uma base de **Clean Architecture**, garantindo uma clara separação de concerns entre as camadas de Domínio, Aplicação, Infraestrutura e Apresentação.

*   **Linguagem:** C#
*   **Framework:** .NET 8
*   **Arquitetura:** Clean Architecture
*   **Comunicação API:** REST com ASP.NET Core
*   **Persistência:** Entity Framework Core com SQL Server
*   **Autenticação:** JWT (JSON Web Tokens)
*   **Mensageria:** RabbitMQ
*   **Testes:** xUnit
*   **Containerização:** Docker

## Status Atual

O projeto está em sua fase inicial de estruturação. A arquitetura base e as fundações de persistência estão sendo construídas. As próximas etapas envolvem a implementação dos casos de uso, a integração com o RabbitMQ e a configuração do ambiente com Docker Compose.

---

_Este repositório será atualizado frequentemente à medida que novas funcionalidades e aprendizados forem incorporados._
