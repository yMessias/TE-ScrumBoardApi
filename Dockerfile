FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copia o arquivo de projeto e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia todo o código-fonte
COPY . .

# Compila a aplicação em modo Release
RUN dotnet publish -c Release -o /app/publish

# ============================================
# ESTÁGIO 2: Produção (Execução)
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copia os arquivos publicados do estágio anterior
COPY --from=build /app/publish .

# Define a porta que a aplicação escuta
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "ScrumBoardApi.dll"]