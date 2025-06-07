// Необхідні using директиви
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
// using System.Text; // Може знадобитися, якщо BasicAuthenticationHandler знаходиться тут. Якщо в окремому файлі, там мають бути свої using.

// Створення WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

// --- Конфігурація сервісів (Dependency Injection Container) ---

// Додайте базову аутентифікацію
// Переконайтеся, що ім'я схеми "BasicAuthentication" збігається з тим, що ви використовуєте в атрибутах [Authorize(AuthenticationSchemes = "BasicAuthentication")]
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddControllers();

// Конфігурація бази даних
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Реєстрація репозиторіїв (Scoped lifetime)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDayTypeRepository, DayTypeRepository>();
builder.Services.AddScoped<IWorkingDayRepository, WorkingDayRepository>();
builder.Services.AddScoped<IUserWorkingDayRepository, UserWorkingDayRepository>();
builder.Services.AddScoped<IChangeRequestRepository, ChangeRequestRepository>();
builder.Services.AddScoped<IUserChangeRequestRepository, UserChangeRequestRepository>();
builder.Services.AddScoped<ISensorDataRepository, SensorDataRepository>(); // Репозиторій для даних сенсорів

// Реєстрація сервісів (Scoped lifetime)
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DayTypeService>();
builder.Services.AddScoped<WorkingDayService>();
builder.Services.AddScoped<UserWorkingDayService>();
builder.Services.AddScoped<ChangeRequestService>();
builder.Services.AddScoped<UserChangeRequestService>();
builder.Services.AddScoped<SensorDataService>(); // Сервіс для даних сенсорів

// Реєстрація маперів (Scoped lifetime)
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<DayTypeMapper>();
builder.Services.AddScoped<WorkingDayMapper>();
builder.Services.AddScoped<ChangeRequestMapper>();
builder.Services.AddScoped<UserWorkingDayMapper>();
builder.Services.AddScoped<UserChangeRequestMapper>();
builder.Services.AddScoped<SensorDataMapper>(); // Мапер для даних сенсорів


// Додайте Swagger для документації API
builder.Services.AddEndpointsApiExplorer(); // Потрібно для Swagger з Top-level statements
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Додайте базову аутентифікацію до Swagger UI
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic", // Це ім'я схеми, яке відображатиметься в Swagger UI
        In = ParameterLocation.Header,
        Description = "Basic Authentication"
    });

    // Додайте вимогу безпеки для всіх операцій в Swagger UI
    // Це додає кнопку "Authorize" і дозволяє надсилати Basic Auth заголовок для тестування API
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" }
            },
            new string[] {} // Пустий масив означає, що ця вимога застосовується до всіх Scopes/Permissions (для Basic Auth це зазвичай так)
        }
    });
});


// --- Налаштування конвеєра HTTP-запитів (Middleware Pipeline) ---

var app = builder.Build();

// Налаштування HTTP-запитів залежно від середовища
if (app.Environment.IsDevelopment())
{
    // В середовищі розробки використовуємо сторінку винятків для розробників
    app.UseDeveloperExceptionPage();

    // Включаємо Swagger UI в середовищі розробки
    // ЦІ MIDDLEWARE МАЮТЬ БУТИ ДО UseAuthentication/UseAuthorization, ЩОБ SWAGGER UI БУВ ДОСТУПНИЙ БЕЗ АВТЕНТИФІКАЦІЇ
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // Можна додати інші налаштування Swagger UI тут
        // c.RoutePrefix = string.Empty; // Щоб Swagger UI був доступний за кореневою адресою "/"
    });
}
else // Наприклад, Production, Staging
{
    // В інших середовищах можна використовувати іншу сторінку помилок
    // app.UseExceptionHandler("/Home/Error"); // Потрібно створити такий контролер/сторінку

    // Можливо, ви також захочете включити Swagger в не-dev середовищі, але тоді його СЛІД ЗАХИСТИТИ!
    // Наприклад, за IP-адресою, ключем, або іншою автентифікацією/авторизацією.
    // Для лабораторної роботи, якщо потрібно, щоб Swagger був доступний через ngrok і в "не-dev" режимі:
    app.UseDeveloperExceptionPage(); // Для лаби можна залишити DeveloperExceptionPage і тут
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // c.RoutePrefix = string.Empty;
    });
    // В реальному проекті тут був би більш безпечний обробник помилок та, можливо, відсутність Swagger
}

// Додайте обслуговування статичних файлів (наприклад, index.html, CSS, JS з папки wwwroot)
// Зазвичай розміщується до UseRouting, щоб запити до статичних файлів не проходили через роутинг контролерів
app.UseStaticFiles();

// app.UseHttpsRedirection(); // Якщо ви використовуєте ngrok з HTTPS -> HTTP, це middleware може спричиняти проблеми.
// Ngrok вже забезпечує HTTPS на публічному боці. Зазвичай відключають, якщо ngrok перенаправляє на HTTP локально.

app.UseRouting(); // Визначає відповідний Endpoint (контролер, Razor Page тощо) для запиту

// !!! ПЕРЕМІСТІТЬ UseAuthentication() та UseAuthorization() СЮДИ !!!
// Після UseRouting, але до MapControllers/MapRazorPages/MapEndpoints
// Це гарантує, що автентифікація та авторизація застосовуються до Endpoint-ів, визначених роутингом.
// Запити до Swagger UI (оброблені раніше) не пройдуть через ці middleware.
app.UseAuthentication(); // Виконує автентифікацію (наприклад, перевіряє Basic Auth заголовок)
app.UseAuthorization();  // Виконує авторизацію (перевіряє права доступу на основі автентифікованого користувача)


// Налаштовує маршрутизацію для контролерів з атрибутами [Route]
// Це має бути після UseRouting, UseAuthentication, UseAuthorization
app.MapControllers();

// Запуск додатку
app.Run();

// Примітка: Клас BasicAuthenticationHandler має бути визначений в окремому файлі
// BasicAuthenticationHandler.cs у вашому проекті.
// Переконайтеся, що в цьому файлі є необхідні using директиви.