using HolyStudy.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddControllersWithViews();
builder.Configuration.AddEnvironmentVariables();

// Configuração da string de conexão
var connectionString = builder.Configuration.GetConnectionString("HolyStudyConn");

// Verifica se a variável de ambiente DATABASE_URL está configurada (Heroku)
if (string.IsNullOrEmpty(connectionString))
{
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (!string.IsNullOrEmpty(databaseUrl))
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        connectionString = $"Host={uri.Host};Port={uri.Port};Username={userInfo[0]};Password={userInfo[1]};Database={uri.AbsolutePath.TrimStart('/')};Pooling=true;SSL Mode=Require;Trust Server Certificate=True";
    }
}

// Lança uma exceção se nenhuma string de conexão for configurada
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão não foi configurada. Verifique as variáveis de ambiente ou o arquivo appsettings.json.");
}

// Configuração do DbContext com base na string de conexão
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (connectionString.Contains("Host=") || connectionString.Contains("postgres"))
        options.UseNpgsql(connectionString); // PostgreSQL (Heroku)
    else
        options.UseSqlServer(connectionString); // SQL Server (Local)
});

var app = builder.Build();

// Configurar o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configurar o endpoint da aplicação
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Themes}/{action=Index}/{id?}");

app.Run();