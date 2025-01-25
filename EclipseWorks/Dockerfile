# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EclipseWorks/EclipseWorks.csproj", "EclipseWorks/"]
COPY ["EclipseWorks.Application/EclipseWorks.Application.csproj", "EclipseWorks.Application/"]
COPY ["EclipseWorks.Domain/EclipseWorks.Domain.csproj", "EclipseWorks.Domain/"]
COPY ["EclipseWorks.Infra.Data/EclipseWorks.Infra.Data.csproj", "EclipseWorks.Infra.Data/"]
COPY ["EclipseWorks.Infra.IOC/EclipseWorks.Infra.IOC.csproj", "EclipseWorks.Infra.IOC/"]

RUN dotnet restore "./EclipseWorks/EclipseWorks.csproj"
COPY . .

WORKDIR "/src/EclipseWorks"
RUN dotnet build "./EclipseWorks.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EclipseWorks.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EclipseWorks.dll"]