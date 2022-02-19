#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["banking-webAPI/banking-webAPI.csproj", "banking-webAPI/"]
RUN dotnet restore "banking-webAPI/banking-webAPI.csproj"
COPY . .
WORKDIR "/src/banking-webAPI"
RUN dotnet build "banking-webAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "banking-webAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "banking-webAPI.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet banking-webAPI.dll