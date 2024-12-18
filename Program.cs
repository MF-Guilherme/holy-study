using HolyStudy.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddControllersWithViews();
builder.Configuration.AddEnvironmentVariables();

// Configuração da string de conexão
var connectionString = builder.Configuration.GetConnectionString("HolyStudyConn");

// Verifica se a variável de ambiente DATABASE_URL está configurada para produção (Heroku)
if (string.IsNullOrEmpty(connectionString) && Environment.GetEnvironmentVariable("DATABASE_URL") is string databaseUrl)
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={uri.Port};Username={userInfo[0]};Password={userInfo[1]};Database={uri.AbsolutePath.TrimStart('/')};Pooling=true;SSL Mode=Require;Trust Server Certificate=True";
}

// Configuração do DbContext com base na string de conexão
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (connectionString.Contains("Host=") || connectionString.Contains("postgres"))
        options.UseNpgsql(connectionString);
    else
        options.UseSqlServer(connectionString);
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