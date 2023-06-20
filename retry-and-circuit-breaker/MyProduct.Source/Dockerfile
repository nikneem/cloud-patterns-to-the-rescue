#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyProduct.Source/MyProduct.Source.csproj", "MyProduct.Source/"]
RUN dotnet restore "MyProduct.Source/MyProduct.Source.csproj"
COPY . .
WORKDIR "/src/MyProduct.Source"
RUN dotnet build "MyProduct.Source.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyProduct.Source.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyProduct.Source.dll"]