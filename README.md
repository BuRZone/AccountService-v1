# Account Service API

Микросервис "Банковские счета" для обслуживания процессов розничного банка.

## Описание

Сервис предоставляет REST API для управления банковскими счетами и транзакциями. Реализован с использованием паттернов CQRS, MediatR и FluentValidation.

## Технологии

- .NET 9
- ASP.NET Core Web API
- MediatR (CQRS)
- FluentValidation
- Swagger/OpenAPI

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

1. Убедитесь, что у вас установлен .NET 9
2. Клонируйте репозиторий
3. Перейдите в папку `AccountService.API`
4. Выполните команду:
   ```bash
   dotnet run
   ```
5. Откройте браузер и перейдите по адресу: `https://localhost:7001` или `http://localhost:5001`

## Swagger документация

После запуска приложения Swagger UI будет доступен по адресу: `https://localhost:7001` или `http://localhost:5001`

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

## Примеры запросов

### Создание текущего счета
```json
POST /api/accounts
{
  "ownerId": "11111111-1111-1111-1111-111111111111",
  "type": "Checking",
  "currency": "RUB"
}
```

### Создание депозита
```json
POST /api/accounts
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
{
  "balance": 5000.00,
  "interestRate": 4.5
}
```

## Структура проекта

```
AccountService.API/
├── Attributes/        # Кастомные атрибуты валидации
├── Behaviors/         # Поведения MediatR pipeline
├── Commands/          # Команды MediatR
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
- **500 Internal Server Error** - Необработанные исключения

### Формат ответа об ошибке
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Ошибка валидации данных.",
  "status": 400,
  "errors": {
    "fieldName": ["Описание ошибки"]
  },
  "traceId": "..."
}
``` 