#!/bin/bash

# --- Змінні середовища (будуть передані з Docker) ---
# Ці змінні вже є в вашому db контейнері. Ми їх будемо використовувати, щоб не хардкодити паролі.
PGDATABASE="${POSTGRES_DB}"
PGUSER="${POSTGRES_USER}"
PGPASSWORD="${POSTGRES_PASSWORD}"
PGHOST="db" # Назва сервісу бази даних в Docker Compose

# Форматуємо дату для імені файлу
TIMESTAMP=$(date +"%Y%m%d_%H%M%S")
BACKUP_FILE="/backups/backup_${TIMESTAMP}.sql"

echo "Створення резервної копії бази даних ${PGDATABASE}..."
echo "Хост: ${PGHOST}, Користувач: ${PGUSER}"
echo "Файл бекапу: ${BACKUP_FILE}"

# Виконуємо pg_dump
pg_dump -h $PGHOST -U $PGUSER -d $PGDATABASE > $BACKUP_FILE

# Перевіряємо, чи успішно створено бекап
if [ $? -eq 0 ]; then
  echo "Резервну копію успішно створено."
else
  echo "Помилка при створенні резервної копії." >&2
  exit 1
fi