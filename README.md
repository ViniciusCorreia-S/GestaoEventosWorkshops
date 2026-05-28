# Gestao de Eventos e Workshops

Sistema web para gerenciamento de eventos, workshops, participantes, organizadores e inscricoes. A aplicacao disponibiliza uma interface em `wwwroot` e uma API REST em ASP.NET Core, com autenticacao JWT e controle de acesso por perfis.

## Descricao do projeto

O projeto permite:

- autenticar administradores, organizadores e participantes;
- cadastrar e gerenciar eventos;
- cadastrar e gerenciar workshops vinculados a eventos;
- cadastrar participantes e organizadores;
- realizar inscricoes de participantes em workshops;
- consultar inscricoes e atualizar status como `Inscrito`, `Compareceu` e `Concluido`;
- acessar uma interface web estatica servida pela propria API.

Perfis de acesso:

- `Administrador`: acesso completo aos cadastros e operacoes administrativas.
- `Organizador`: consulta dados relacionados aos seus eventos/workshops e atualiza status de inscricoes.
- `Participante`: consulta suas proprias inscricoes.

## Tecnologias utilizadas

- C# e ASP.NET Core Web API
- .NET 10
- Entity Framework Core
- Pomelo.EntityFrameworkCore.MySql
- MySQL
- JWT Bearer Authentication
- Swagger / OpenAPI
- HTML, CSS e JavaScript puro no frontend
- Docker
- Railway para deploy

## Como executar localmente

### Pre-requisitos

- .NET SDK 10 instalado
- MySQL 8 ou uma instancia MySQL acessivel
- Git

### Configuracao

Configure a string de conexao `ConexaoPadrao` e as chaves JWT no `appsettings.Development.json`, em secrets locais ou por variaveis de ambiente.

Exemplo de estrutura:

```json
{
  "ConnectionStrings": {
    "ConexaoPadrao": "server=localhost;port=3306;database=gestao_eventos;user=root;password=sua_senha"
  },
  "Jwt": {
    "Key": "uma-chave-secreta-grande-para-assinatura-jwt",
    "Issuer": "GestaoEventosWorkshops",
    "Audience": "GestaoEventosWorkshopsUsuarios"
  }
}
```

### Passos

Clone o repositorio e acesse a pasta do projeto:

```bash
git clone <url-do-repositorio>
cd ProjetoFinal
```

Restaure as dependencias:

```bash
dotnet restore
```

Execute a aplicacao:

```bash
dotnet run --project GestaoEventosWorkshops
```

Por padrao, a aplicacao usa a porta definida na variavel de ambiente `PORT`. Caso ela nao esteja definida, o projeto sobe em:

```text
http://localhost:8080
```

Ao iniciar, as migrations do Entity Framework Core sao aplicadas automaticamente no banco configurado.

### Acessos locais

- Interface web: `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`
- Health check: `http://localhost:8080/health`

Usuario administrador padrao para login:

```text
usuario: admin
senha: 123456
```

## Acesso online

O projeto esta publicado no Railway:

[https://gestaoeventosworkshops-production.up.railway.app](https://gestaoeventosworkshops-production.up.railway.app)

Endpoints uteis no ambiente online:

- Interface web: `https://gestaoeventosworkshops-production.up.railway.app`
- Swagger: `https://gestaoeventosworkshops-production.up.railway.app/swagger`
- Health check: `https://gestaoeventosworkshops-production.up.railway.app/health`

## Autenticacao

A API usa JWT Bearer Token. Para obter um token, envie as credenciais para:

```http
POST /api/auth/login
```

Exemplo de corpo:

```json
{
  "usuario": "admin",
  "senha": "123456",
  "aceiteTermosLgpd": true
}
```

Use o token retornado no header das requisicoes protegidas:

```http
Authorization: Bearer <token>
```

## Endpoints da API

Base local:

```text
http://localhost:8080
```

Base online:

```text
https://gestaoeventosworkshops-production.up.railway.app
```

### Auth

| Metodo | Endpoint | Autenticacao | Descricao |
| --- | --- | --- | --- |
| POST | `/api/auth/login` | Publico | Realiza login e retorna token JWT. |

Corpo esperado:

```json
{
  "usuario": "admin",
  "senha": "123456",
  "aceiteTermosLgpd": true
}
```

### Eventos

| Metodo | Endpoint | Autenticacao | Descricao |
| --- | --- | --- | --- |
| GET | `/api/eventos` | Publico, com filtro por organizador quando autenticado como organizador | Lista eventos. |
| GET | `/api/eventos/{id}` | Publico | Busca evento por id. |
| POST | `/api/eventos` | Administrador | Cria evento. |
| PUT | `/api/eventos/{id}` | Administrador | Atualiza evento. |
| DELETE | `/api/eventos/{id}` | Administrador | Remove evento. |

Corpo para criar/atualizar:

```json
{
  "nome": "Semana de Tecnologia",
  "codigo": "TEC2026",
  "local": "Auditorio Central",
  "dataInicio": "2026-06-01",
  "dataFim": "2026-06-03",
  "organizadorId": 1
}
```

### Workshops

| Metodo | Endpoint | Autenticacao | Descricao |
| --- | --- | --- | --- |
| GET | `/api/workshops` | Publico, com filtro por organizador quando autenticado como organizador | Lista workshops. |
| GET | `/api/workshops/{id}` | Publico | Busca workshop por id. |
| POST | `/api/workshops` | Administrador | Cria workshop. |
| PUT | `/api/workshops/{id}` | Administrador | Atualiza workshop. |
| DELETE | `/api/workshops/{id}` | Administrador | Remove workshop. |

Corpo para criar/atualizar:

```json
{
  "nome": "Introducao a APIs",
  "codigo": "API101",
  "cargaHoraria": 8,
  "eventoId": 1
}
```

### Participantes

| Metodo | Endpoint | Autenticacao | Descricao |
| --- | --- | --- | --- |
| GET | `/api/participantes` | Publico, com filtro por organizador quando autenticado como organizador | Lista participantes. |
| GET | `/api/participantes/{id}` | Publico | Busca participante por id. |
| POST | `/api/participantes` | Publico | Cria participante. |
| PUT | `/api/participantes/{id}` | Administrador ou Organizador | Atualiza participante. |
| DELETE | `/api/participantes/{id}` | Administrador | Remove participante. |

Corpo para criar:

```json
{
  "nome": "Maria Silva",
  "email": "maria@email.com",
  "codigoInscricao": "MAT123",
  "dataNascimento": "2000-05-20",
  "fotoPerfil": null,
  "aceiteTermosLgpd": true
}
```

Corpo para atualizar:

```json
{
  "nome": "Maria Silva",
  "email": "maria@email.com",
  "dataNascimento": "2000-05-20",
  "ativo": true
}
```

### Organizadores

| Metodo | Endpoint | Autenticacao | Descricao |
| --- | --- | --- | --- |
| GET | `/api/organizadores` | Administrador | Lista organizadores. |
| POST | `/api/organizadores` | Administrador | Cria organizador. |
| DELETE | `/api/organizadores/{id}` | Administrador | Remove organizador. |

Corpo para criar:

```json
{
  "nome": "Joao Organizador",
  "email": "organizador@email.com",
  "senha": "123456",
  "fotoPerfil": null
}
```

### Inscricoes

| Metodo | Endpoint | Autenticacao | Descricao |
| --- | --- | --- | --- |
| GET | `/api/inscricoes` | Administrador ou Organizador | Lista inscricoes. Organizadores visualizam somente registros vinculados aos seus eventos/workshops. |
| GET | `/api/inscricoes/minhas` | Participante | Lista inscricoes do participante autenticado. |
| POST | `/api/inscricoes` | Administrador | Cria inscricao em workshop. |
| PATCH | `/api/inscricoes/{id}/status` | Administrador ou Organizador | Atualiza status da inscricao. |
| DELETE | `/api/inscricoes/{id}` | Administrador | Remove inscricao. |

Corpo para criar:

```json
{
  "participanteId": 1,
  "workshopId": 1
}
```

Corpo para atualizar status:

```json
{
  "status": "Compareceu"
}
```

Status aceitos:

- `Inscrito`
- `Compareceu`
- `Concluido`

## Observacoes

- A documentacao interativa da API tambem esta disponivel via Swagger em `/swagger`.
- O endpoint `/health` pode ser usado para verificar se a aplicacao esta ativa.
- A aplicacao aplica migrations automaticamente na inicializacao.
