# 📚 BibliotecaAPI

API REST para gerenciamento de autores, livros, alunos e empréstimos de uma biblioteca.

Este projeto foi desenvolvido como projeto prático utilizando ASP.NET Core 8, Entity Framework Core e SQLite. A aplicação aplica separação de responsabilidades, injeção de dependência, DTOs, Repository Pattern, Services e tratamento global de erros com `ProblemDetails`.

---

## 📌 Sobre o projeto

A BibliotecaAPI automatiza o controle do acervo e do fluxo de empréstimos, permitindo:

- Cadastrar e consultar autores.
- Cadastrar e consultar livros.
- Filtrar livros por título ou autor.
- Cadastrar e consultar alunos.
- Validar matrícula única.
- Registrar empréstimos.
- Controlar automaticamente o estoque.
- Registrar devoluções.
- Impedir empréstimos duplicados.
- Retornar erros padronizados no formato `ProblemDetails`.

---

## 🚀 Tecnologias utilizadas

| Tecnologia | Utilização |
|---|---|
| C# | Linguagem de programação |
| .NET 8 | Plataforma de desenvolvimento |
| ASP.NET Core Web API | Construção da API REST |
| Entity Framework Core 8 | ORM para acesso a dados |
| SQLite | Banco de dados relacional |
| Swagger / OpenAPI | Documentação e testes da API |
| Git | Controle de versão |

---

## 🧱 Arquitetura

O projeto foi organizado em camadas para separar responsabilidades:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
DbContext
    ↓
SQLite
```

### Responsabilidade de cada camada

- **Controllers:** recebem requisições HTTP e retornam respostas.
- **DTOs:** controlam os dados de entrada e saída da API.
- **Services:** concentram as regras de negócio.
- **Repositories:** realizam o acesso aos dados.
- **Models:** representam as entidades do domínio.
- **Data:** contém o `DbContext` do Entity Framework Core.
- **Exceptions:** contém exceções personalizadas.
- **Middleware:** realiza o tratamento global de erros.
- **Migrations:** armazenam o histórico de evolução do banco.

---

## 📁 Estrutura do projeto

```text
BibliotecaAPI/
├── Controllers/
│   ├── AlunosController.cs
│   ├── AutoresController.cs
│   ├── EmprestimosController.cs
│   └── LivrosController.cs
├── Data/
│   └── BibliotecaContext.cs
├── DTOs/
│   ├── AlunoResponseDto.cs
│   ├── AutorResponseDto.cs
│   ├── CriarAlunoDto.cs
│   ├── CriarAutorDto.cs
│   ├── CriarEmprestimoDto.cs
│   ├── CriarLivroDto.cs
│   ├── EmprestimoResponseDto.cs
│   └── LivroResponseDto.cs
├── Exceptions/
│   ├── BusinessConflictException.cs
│   └── NotFoundException.cs
├── Migrations/
├── Middleware/
│   └── GlobalExceptionHandler.cs
├── Models/
│   ├── Aluno.cs
│   ├── Autor.cs
│   ├── Emprestimo.cs
│   ├── Livro.cs
│   └── StatusEmprestimo.cs
├── Repositories/
├── Services/
├── appsettings.json
├── BibliotecaAPI.csproj
├── biblioteca.db
├── Program.cs
└── README.md
```

---

## ⚙️ Pré-requisitos

Antes de executar o projeto, instale:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/downloads)
- Visual Studio Code ou Visual Studio 2022

Verifique a instalação do .NET:

```bash
dotnet --version
```

---

## ▶️ Como executar

### 1. Clonar o repositório

```bash
git clone https://github.com/ErnandesCosta/BibliotecaAPI.git
cd BibliotecaAPI/BibliotecaAPI
```

### 2. Restaurar os pacotes

```bash
dotnet restore
```

### 3. Criar ou atualizar o banco

```bash
dotnet ef database update
```

Se a ferramenta do Entity Framework ainda não estiver instalada:

```bash
dotnet tool install --global dotnet-ef --version 8.0.23
```

### 4. Compilar o projeto

```bash
dotnet build
```

### 5. Executar a API

```bash
dotnet run
```

### 6. Acessar o Swagger

Com a aplicação em execução, acesse:

```text
http://localhost:5140/swagger
```

A porta pode ser diferente dependendo da configuração local. Sempre utilize a porta exibida no terminal após executar `dotnet run`.

---

## 🔗 Endpoints

### ✍️ Autores

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/autores` | Cadastra um autor |
| `GET` | `/api/autores` | Lista todos os autores |
| `GET` | `/api/autores/{id}` | Busca um autor por ID |

#### Exemplo de criação

```json
{
  "nome": "Robert C. Martin",
  "dataNascimento": "1952-12-05",
  "nacionalidade": "Americana"
}
```

---

### 📖 Livros

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/livros` | Cadastra um livro |
| `GET` | `/api/livros` | Lista todos os livros |
| `GET` | `/api/livros/{id}` | Busca um livro por ID |
| `GET` | `/api/livros?titulo=clean` | Filtra pelo título |
| `GET` | `/api/livros?autor=martin` | Filtra pelo nome do autor |

#### Exemplo de criação

```json
{
  "isbn": "9780132350884",
  "titulo": "Clean Code",
  "anoPublicacao": 2008,
  "quantidade": 3,
  "autorId": 1
}
```

---

### 🎓 Alunos

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/alunos` | Cadastra um aluno |
| `GET` | `/api/alunos` | Lista todos os alunos |
| `GET` | `/api/alunos/{id}` | Busca um aluno por ID |

#### Exemplo de criação

```json
{
  "nome": "José da Silva",
  "matricula": "2026001",
  "email": "jose@example.com"
}
```

---

### 🔄 Empréstimos

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/emprestimos` | Registra um empréstimo |
| `GET` | `/api/emprestimos` | Lista os empréstimos |
| `GET` | `/api/emprestimos/{id}` | Busca um empréstimo por ID |
| `PUT` | `/api/emprestimos/{id}/devolucao` | Registra a devolução |

#### Exemplo de criação

```json
{
  "alunoId": 1,
  "livroId": 1,
  "dataPrevistaDevolucao": "2026-08-21T18:00:00"
}
```

---

## 📦 Regras de negócio

A API retorna `409 Conflict` nas seguintes situações:

- Tentativa de emprestar um livro sem exemplares disponíveis.
- Tentativa de criar dois empréstimos ativos do mesmo livro para o mesmo aluno.
- Tentativa de devolver um empréstimo que já foi devolvido.
- Tentativa de cadastrar um aluno com matrícula já existente.

### Controle de estoque

Ao criar um empréstimo:

```text
Quantidade do livro = Quantidade atual - 1
```

Ao registrar uma devolução:

```text
Quantidade do livro = Quantidade atual + 1
```

### Status do empréstimo

| Valor | Status |
|---:|---|
| `0` | Ativo |
| `1` | Devolvido |
| `2` | Atrasado |

---

## 🛑 Códigos HTTP utilizados

| Código | Significado |
|---:|---|
| `200` | Operação de consulta realizada com sucesso |
| `201` | Recurso criado com sucesso |
| `400` | Dados de entrada inválidos |
| `404` | Recurso não encontrado |
| `409` | Conflito com uma regra de negócio |
| `500` | Erro inesperado no servidor |

---

## 🧾 Exemplo de erro

As exceções são tratadas globalmente e retornadas no formato `ProblemDetails`:

```json
{
  "title": "Conflito de negócio",
  "status": 409,
  "detail": "O livro não possui exemplares disponíveis.",
  "instance": "/api/emprestimos"
}
```

---

## 🗃️ Banco de dados

O projeto utiliza SQLite com a seguinte configuração:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=biblioteca.db"
  }
}
```

As migrations ficam armazenadas na pasta `Migrations/` e podem ser aplicadas com:

```bash
dotnet ef database update
```

Para criar uma nova migration após alterar os Models:

```bash
dotnet ef migrations add NomeDaMigration
```

---

## ✅ Checklist de validação

- [x] SQLite configurado.
- [x] Entity Framework Core configurado.
- [x] Migration inicial criada.
- [x] Swagger configurado.
- [x] DTOs implementados.
- [x] Repositories implementados.
- [x] Services implementados.
- [x] Middleware global de exceções implementado.
- [x] Cadastro de autores implementado.
- [x] Cadastro e filtro de livros implementados.
- [x] Cadastro de alunos implementado.
- [x] Controle de matrícula duplicada implementado.
- [x] Cadastro de empréstimos implementado.
- [x] Controle de estoque implementado.
- [x] Devolução de empréstimos implementada.
- [x] Regras de conflito implementadas.

---

## 🌿 Padronização de commits

O projeto utiliza mensagens de commit semânticas:

```text
feat: adiciona uma nova funcionalidade
fix: corrige um problema
chore: realiza configuração ou manutenção
docs: atualiza documentação
```

Exemplos:

```bash
git commit -m "feat: implementa cadastro de livros"
git commit -m "fix: corrige atualização do estoque"
git commit -m "docs: atualiza README"
```

---

## 👨‍💻 Autores(a)

Desenvolvido por **José Ernandes**, **Ruan Carvalho**, **Rayssa Victória**.

Projeto acadêmico desenvolvido para fins de estudo e demonstração de conhecimentos em desenvolvimento de APIs REST com .NET.