# Account Service API

Микросервис "Банковские счета" для обслуживания процессов розничного банка.

## Описание

Сервис предоставляет REST API для управления банковскими счетами и транзакциями. Реализован с использованием паттернов CQRS, MediatR и FluentValidation. Поддерживает JWT аутентификацию через Keycloak и контейнеризацию с Docker.

## Технологии

- .NET 9
- ASP.NET Core Web API
- MediatR (CQRS)
- FluentValidation
- Swagger/OpenAPI
- JWT Authentication (Keycloak)
- Docker & Docker Compose
- CORS

## Архитектурные особенности

### MbResult Pattern
Все API методы возвращают унифицированный тип `MbResult<T>`, который инкапсулирует либо успешный результат, либо ошибку:

```csharp
public class MbResult<T>
{
    public T? Result { get; private set; }
    public MbError? Error { get; private set; }
    public bool IsSuccess => Error == null;
    public bool IsFailure => !IsSuccess;
}
```

### Валидация
- FluentValidation настроен для возврата только первой ошибки на поле
- Все ошибки валидации возвращаются через `MbResult.Failure()`
- Отключена стандартная модель валидации ASP.NET Core

### Аутентификация
- JWT Bearer токены через Keycloak
- Все API методы защищены атрибутом `[Authorize]`
- Автоматическая подстановка "Bearer" в Swagger UI

## Функциональность

### Управление счетами
- Создание счета
- Обновление счета
- Удаление счета
- Получение списка счетов
- Получение счета по ID
- Проверка существования счета

### Управление транзакциями
- Создание транзакции по счету
- Перевод между счетами

### Типы счетов
- Checking (Текущий счет)
- Deposit (Депозит)
- Credit (Кредитный)

### Типы транзакций
- Credit (Пополнение)
- Debit (Списание)

## Запуск проекта

### Локальный запуск
1. Убедитесь, что у вас установлен .NET 9
2. Клонируйте репозиторий
3. Перейдите в папку `AccountService.API`
4. Выполните команду:
   ```bash
   dotnet run
   ```
5. Откройте браузер и перейдите по адресу: `https://localhost:7001` или `http://localhost:5001`

### Запуск с Docker Compose (рекомендуется)
1. Убедитесь, что установлен Docker и Docker Compose
2. В корневой папке проекта выполните:
   ```bash
   docker-compose up --build -d
   ```
3. API будет доступен по адресу: `http://localhost`
4. Keycloak будет доступен по адресу: `http://localhost:8080`

## Аутентификация

### Получение токена
```bash
curl -X POST "http://localhost:8080/realms/AccountServiceRealm/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id=account-service&grant_type=password&username=testuser&password=password"
```

### Тестовые учетные данные
- **Username**: testuser
- **Password**: password
- **Realm**: AccountServiceRealm
- **Client**: account-service

## Swagger документация

После запуска приложения Swagger UI будет доступен по адресу:
- Локально: `https://localhost:7001/swagger` или `http://localhost:5001/swagger`
- Docker: `http://localhost/swagger`

Для тестирования API в Swagger:
1. Получите JWT токен через Keycloak
2. В Swagger UI нажмите кнопку "Authorize"
3. Вставьте токен в поле (система автоматически добавит "Bearer ")

## API Endpoints

### Счета
- `GET /api/accounts` - Получить список всех счетов
- `GET /api/accounts?ownerId={guid}` - Получить счета по владельцу
- `GET /api/accounts/{id}` - Получить счет по ID
- `POST /api/accounts` - Создать новый счет
- `PUT /api/accounts/{id}` - Обновить счет целиком
- `PATCH /api/accounts/{id}` - Частичное обновление счета
- `DELETE /api/accounts/{id}` - Удалить счет
- `GET /api/accounts/{id}/exists` - Проверить существование счета

### Транзакции
- `POST /api/accounts/transactions` - Создать транзакцию
- `POST /api/accounts/transfer` - Выполнить перевод между счетами

## HTTP Status Codes

- **200 OK** - Успешная операция
- **400 Bad Request** - Ошибки валидации или бизнес-логики
- **401 Unauthorized** - Отсутствует или недействительный JWT токен
- **404 Not Found** - Ресурс не найден
- **500 Internal Server Error** - Внутренняя ошибка сервера

## Примеры запросов

### Создание текущего счета
```json
POST /api/accounts
Authorization: Bearer <your-jwt-token>
{
  "ownerId": "11111111-1111-1111-1111-111111111111",
  "type": "Checking",
  "currency": "RUB"
}
```

### Создание депозита
```json
POST /api/accounts
Authorization: Bearer <your-jwt-token>
{
  "ownerId": "11111111-1111-1111-1111-111111111111",
  "type": "Deposit",
  "currency": "RUB",
  "interestRate": 3.0
}
```

### Создание транзакции
```json
POST /api/accounts/transactions
Authorization: Bearer <your-jwt-token>
{
  "accountId": "account-guid-here",
  "amount": 1000.00,
  "currency": "RUB",
  "type": "Credit",
  "description": "Пополнение счета"
}
```

### Перевод между счетами
```json
POST /api/accounts/transfer
Authorization: Bearer <your-jwt-token>
{
  "fromAccountId": "from-account-guid",
  "toAccountId": "to-account-guid",
  "amount": 200.00,
  "currency": "RUB",
  "description": "Перевод между счетами"
}
```

### Частичное обновление счета
```json
PATCH /api/accounts/{id}
Authorization: Bearer <your-jwt-token>
{
  "balance": 5000.00,
  "interestRate": 4.5
}
```

## Структура проекта

```
AccountService.API/
├── Attributes/        # Кастомные атрибуты валидации
├── Behaviors/         # Поведения MediatR pipeline (включая ValidationBehavior)
├── Commands/          # Команды MediatR
├── Common/           # Общие типы (MbResult, MbError)
├── Controllers/       # API контроллеры
├── DTOs/             # Data Transfer Objects
├── Filters/          # Фильтры для обработки ошибок
├── Handlers/         # Обработчики команд и запросов
├── Models/           # Модели данных
├── Queries/          # Запросы MediatR
├── Services/         # Сервисы (заглушки)
└── Validators/       # Валидаторы FluentValidation
```

## Валидация

Проект использует FluentValidation и Data Annotations для валидации входящих данных:

- Проверка существования клиента
- Проверка поддерживаемых валют (кастомный атрибут CurrencyValidation)
- Валидация типов счетов и транзакций
- Проверка корректности дат
- Валидация сумм и процентных ставок
- Проверка достаточности средств при списании
- Валидация принадлежности счетов одному клиенту при переводах
- Проверка совпадения валют при переводах

### Сообщения об ошибках
Все сообщения об ошибках валидации выводятся на русском языке.

### Кастомные атрибуты валидации
- **CurrencyValidationAttribute** - проверяет корректность кода валюты по стандарту ISO 4217 (3 символа) и поддерживаемые валюты

## Заглушки сервисов

В проекте реализованы заглушки для следующих сервисов:

- **AccountStorageService** - хранение счетов в памяти
- **ClientVerificationService** - верификация клиентов
- **CurrencyService** - проверка поддерживаемых валют

## Поддерживаемые валюты

- RUB (Российский рубль)
- USD (Доллар США)
- EUR (Евро)
- GBP (Фунт стерлингов)
- JPY (Японская иена)
- CNY (Китайский юань)

## Тестовые клиенты

В заглушке ClientVerificationService предустановлены следующие клиенты:
- `11111111-1111-1111-1111-111111111111` - Иван
- `22222222-2222-2222-2222-222222222222` - Анна
- `33333333-3333-3333-3333-333333333333` - Алексей

## Обработка ошибок

### Типы ошибок
- **400 Bad Request** - Ошибки валидации и бизнес-логики
- **401 Unauthorized** - Ошибки аутентификации
- **404 Not Found** - Ресурс не найден
- **500 Internal Server Error** - Необработанные исключения

### Формат ответа об ошибке
```json
{
  "result": null,
  "error": {
    "code": "ValidationError",
    "message": "One or more validation errors occurred.",
    "details": {
      "fieldName": ["Описание ошибки"]
    }
  },
  "isSuccess": false,
  "isFailure": true
}
```

## Docker конфигурация

### Dockerfile
- Многоэтапная сборка для оптимизации размера образа
- Публикация на порту 80 внутри контейнера
- Базовый образ .NET 9.0

### Docker Compose
- **account-service**: API на порту 80
- **keycloak**: Сервер аутентификации на порту 8080
- Автоматический импорт realm конфигурации
- Настроенная сеть между сервисами

## CORS

Настроен для разрешения всех источников (`AllowAll`) для разработки. В продакшене рекомендуется настроить конкретные домены. 