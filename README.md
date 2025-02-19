# HolyStudy

HolyStudy é uma aplicação web desenvolvida com **ASP.NET Core 8** e **Entity Framework Core** para o gerenciamento de estudos bíblicos. O sistema permite cadastrar temas de estudo e associar passagens bíblicas a cada um deles. 

## 📌 Funcionalidades

- Criar, editar e excluir **temas de estudo**.
- Adicionar **passagens bíblicas** associadas a um tema.
- Filtrar temas por nome e marcar temas favoritos.
- Interface responsiva desenvolvida com **Bootstrap** e **SCSS**.
- **Mensagens de feedback** com TempData para indicar o sucesso ou erro das operações.

---

## 🛠️ Tecnologias Utilizadas

- **Backend:** ASP.NET Core 8, Entity Framework Core
- **Banco de Dados:** SQL Server (local)
- **Frontend:** Bootstrap, SCSS, JQuery
- **ORM:** Entity Framework Core
- **Gerenciamento de Dependências:** NuGet
- **Controle de Versão:** Git e GitHub

---

## 📁 Estrutura do Projeto

O projeto segue uma estrutura organizada conforme os padrões do ASP.NET Core MVC:

```bash
HolyStudy/
│-- bin/                        # Diretório gerado após build/publicação
│-- obj/                        # Diretório gerado para metadados da compilação
│-- Controllers/                 # Contém os controladores da aplicação
│   │-- ThemesController.cs      # Controlador responsável pelos temas
│   │-- PassagesController.cs    # Controlador responsável pelas passagens bíblicas
│   │-- BooksController.cs       # Controlador responsável pelos capítulos e versículos da bíblia
│-- Data/                        # Configuração do banco de dados e contexto do EF Core
│   │-- ApplicationDbContext.cs  # Configuração do contexto do Entity Framework
│-- Migrations/                  # Arquivos de migração do Entity Framework Core
│-- Models/                      # Modelos da aplicação
│   │-- Book.cs                  # Modelo para os livros
│   │-- Theme.cs                 # Modelo para os temas
│   │-- Passage.cs               # Modelo para as passagens
│-- Views/                       # Views (arquivos Razor) da aplicação
│   │-- Shared/                  # Views compartilhadas (ex.: _Layout.cshtml)
│   │-- Themes/                  # Views relacionadas aos temas
│   │-- Passages/                # Views relacionadas às passagens
│-- wwwroot/                     # Arquivos estáticos (CSS, JS, imagens, etc.)
│   │-- css/                     # Arquivos CSS personalizados
│   │-- js/                      # Scripts JS personalizados
│   │-- images/                  # Logos e imagens utilizadas
│-- appsettings.json              # Configurações gerais do aplicativo
│-- appsettings.Development.json  # Configurações específicas para ambiente de desenvolvimento
│-- Program.cs                    # Arquivo principal para inicialização do ASP.NET Core
│-- HolyStudy.csproj               # Arquivo de configuração do projeto
│-- README.md                      # Documentação do projeto
```

---

## 🚀 Como Rodar o Projeto Localmente

### 1️⃣ Pré-requisitos

- .NET SDK 8 instalado ([Download .NET](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))
- SQL Server instalado e rodando localmente
- Git instalado

### 2️⃣ Configurar o Banco de Dados (Local)

Certifique-se de que o SQL Server esteja rodando e configure sua string de conexão no arquivo **appsettings.Development.json**:

```json
"ConnectionStrings": {
  "HolyStudyConn": "Server=localhost;Database=HolyStudyDB;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True;"
}
```

Crie o banco e execute as migrações:

```bash
dotnet ef database update
```

### 3️⃣ Executar a Aplicação

```bash
dotnet run
```

A aplicação estará disponível em http://localhost:5000


