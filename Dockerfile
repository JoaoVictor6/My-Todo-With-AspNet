FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY . /src
WORKDIR /src
RUN dotnet build -o /app/build

FROM base AS final
WORKDIR /app
COPY --from=build /app/build .
RUN ls
ENTRYPOINT ["dotnet", "MyTodo.dll"]