# Imagem base do .NET para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar tudo para o container
COPY . ./

# Restaurar dependências
RUN dotnet restore

# Publicar o aplicativo
RUN dotnet publish -c Release -o out

# Imagem final para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar os binários publicados
COPY --from=build-env /app/out .

# Expor a porta usada pelo seu app
EXPOSE 5000

# Comando para rodar o aplicativo
ENTRYPOINT ["dotnet", "HolyStudy.dll"]
