# Використовуємо образ SDK для збірки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY StaffFlowApi.csproj ./ 
RUN dotnet restore StaffFlowApi.csproj 
COPY . .
RUN dotnet publish StaffFlowApi.csproj -c Release -o out

# Використовуємо образ Runtime для запуску (менший за розміром)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Перемикаємось на користувача root, щоб встановити пакети та змінити дозволи
USER root
RUN apt-get update && apt-get install -y postgresql-client

# --- НОВИЙ КРОК: Створюємо директорію для бекапів та надаємо права ---
RUN mkdir /backups
RUN chown app /backups
# -------------------------------------------------------------------

# Повертаємось до стандартного користувача 'app'
USER app

# Копіюємо опубліковані файли з етапу build
COPY --from=build /app/out .

# Налаштування для запуску в Docker
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Точка входу для запуску застосунку
ENTRYPOINT ["dotnet", "StaffFlowApi.dll"]