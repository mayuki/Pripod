FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pripod.SampleApp/Pripod.SampleApp.csproj", "ConsoleApp1/"]
RUN dotnet restore "Pripod.SampleApp/Pripod.SampleApp.csproj"
COPY . .
WORKDIR "/samples/Pripod.SampleApp"
RUN dotnet build "Pripod.SampleApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pripod.SampleApp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pripod.SampleApp.dll"]