@@ -0,0 +1,24 @@
 # Etapa 1: build
 FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
 WORKDIR /app
 
 # Copia o .csproj e restaura as dependências
 COPY ./FrotaVisionAPI/FrotaVisionAPI.csproj ./
 RUN dotnet restore
 
 # Copia o restante do código e publica
 COPY . ./
 RUN dotnet publish -c Release -o out
 
 # Etapa 2: runtime
 FROM mcr.microsoft.com/dotnet/aspnet:8.0
 WORKDIR /app
 
 # Copia o build para o container final
 COPY --from=build /app/out .
 
 # Expõe a porta 80
 EXPOSE 80
 
 # Nome exato do DLL gerado (use o nome do seu projeto)
 ENTRYPOINT ["dotnet", "FrotaVisionAPI.dll"]
