# Usar a imagem base do SDK .NET 8.0 para desenvolvimento e execução em um único estágio
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Definir o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copiar o arquivo de projeto e restaurar as dependências
COPY *.csproj ./
RUN dotnet restore

# Copiar o restante dos arquivos da aplicação
COPY . .

# Publicar a aplicação
RUN dotnet publish -c Release -o /app/publish

# Definir o diretório de trabalho para a pasta publicada
WORKDIR /app/publish

# Expor a porta 8080 para o contêiner
EXPOSE 8080

# Configurar o comando de execução da aplicação
ENTRYPOINT ["dotnet", "NomeDoSeuProjeto.dll"]
