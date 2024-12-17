using HolyStudy.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Themes}/{action=Index}/{id?}");

app.Run();