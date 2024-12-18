using HolyStudy.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddControllersWithViews();
builder.Configuration.AddEnvironmentVariables();

// Aqui está o código de configuração que vai verificar se a variável de ambiente "DATABASE_URL" está configurada
var connectionString = builder.Configuration.GetConnectionString("HolyStudyConn");

// Verifica se a variável de ambiente DATABASE_URL está configurada para produção (Heroku)
if (string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_URL")))
{
    connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
}

// Configuração do DbContext com base na string de conexão
if (connectionString.Contains("postgres"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));  // Usando PostgreSQL se a string de conexão for do Heroku
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));  // Usando SQL Server local
}

var app = builder.Build();

// Configurar o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // O valor padrão de HSTS é 30 dias. Você pode querer mudar isso para cenários de produção, veja https://aka.ms/aspnetcore-hsts.
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

// Bind para a porta do Heroku (usando variável de ambiente PORT)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";  // Se a variável PORT não estiver definida, usa 8080 por padrão.
app.Run($"http://0.0.0.0:{port}");  // Escutando na porta configurada

app.Run();