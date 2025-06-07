# Використовуємо образ SDK для збірки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копіюємо файл проекту. Вказуємо точну назву файлу з лапками, щоб обробити пробіли.
COPY "StaffFlow api.csproj" ./ 

# Відновлюємо залежності для конкретного файлу проекту, вказуючи його явно
RUN dotnet restore "StaffFlow api.csproj" 

# Копіюємо весь вихідний код
COPY . .

# Публікуємо додаток. Знову вказуємо точну назву файлу проекту.
RUN dotnet publish "StaffFlow api.csproj" -c Release -o out

# Використовуємо образ Runtime для запуску (менший за розміром)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final 
WORKDIR /app

# Копіюємо опубліковані файли з етапу build
COPY --from=build /app/out .

# Важливо: налаштувати, щоб ASP.NET Core слухав на всіх інтерфейсах (0.0.0.0),
# а не тільки на localhost, щоб бути доступним для Docker та зовнішніх запитів.
ENV ASPNETCORE_URLS=http://+:80

# Порт, який буде слухати контейнер. Це внутрішній порт контейнера.
EXPOSE 80

# Точка входу для запуску застосунку
ENTRYPOINT ["dotnet", "StaffFlow api.dll"]