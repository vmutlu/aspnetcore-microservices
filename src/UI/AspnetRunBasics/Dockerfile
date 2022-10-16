#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["UI/AspnetRunBasics/AspnetRunBasics.csproj", "UI/AspnetRunBasics/"]
COPY ["Core/Core.Messages/Core.Messages.csproj", "Core/Core.Messages/"]
RUN dotnet restore "UI/AspnetRunBasics/AspnetRunBasics.csproj"
COPY . .
WORKDIR "/src/UI/AspnetRunBasics"
RUN dotnet build "AspnetRunBasics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspnetRunBasics.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspnetRunBasics.dll"]