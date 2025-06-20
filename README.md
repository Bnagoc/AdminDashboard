# V. Samusev - Admin Dashboard - Mira Games

> Реализация административной панели с ASP.NET Core 8 API и React-фронтендом

## Запуск API (http://localhost:5000)

### Backend (ASP.NET Core API)

```
bash
cd backend/api
dotnet restore
dotnet run
```

## Запуск API (http://localhost:5173)

### Frontend (React + Vite)

```
bash
cd frontend
npm install
npm run dev
```

## Данные для входа

- Email: admin@mirra.dev
- Password: admin123  

## Примеры запросов

### 1. Авторизация

#### Запрос

```
curl -X POST http://localhost:5000/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@mirra.dev","password":"admin123"}'
```

#### Ответ

```
{
  "token": "string",
  "refreshToken": "string"
}
```

#### Сохранение токена в заголовке

```
Authorization: Bearer [token]
```

### 2. Получить список клиентов

#### Запрос

```
curl http://localhost:5000/clients
  -H "Authorization: Bearer [token]"
```

#### Ответ

```
{
  "items": [
    {
      "id": 0,
      "name": "string",
      "email": "string",
      "balance": 0,
      "createdAtUtc": "2025-06-20T09:42:07.369Z"
    }
  ],
  "page": 0,
  "pageSize": 0,
  "totalItems": 0,
  "hasNextPage": true,
  "hasPreviousPage": true
}
```

### 3. Получить последние N платежей

#### Запрос

```
curl http://localhost:5000/payments?take=5
  -H "Authorization: Bearer [token]"
```

#### Ответ

```
{
  "items": [
    {
      "id": 0,
      "amount": 0,
      "client": "string",
      "createdAtUtc": "2025-06-20T09:45:45.508Z",
      "rate": 0
    }
  ],
  "page": 0,
  "pageSize": 0,
  "totalItems": 0,
  "hasNextPage": true,
  "hasPreviousPage": true
}
```

### 4. Получить текущий курс

#### Запрос

```
curl http://localhost:5000/rates
```

#### Ответ

```
{
  "id": 0,
  "value": 0,
  "createdAtUtc": "2025-06-20T09:46:33.520Z"
}
```

### 5. Обновить курс

#### Запрос

```
curl -X POST http://localhost:5000/rates \
  -H "Authorization: Bearer [token]" \
  -H "Content-Type: application/json" \
  -d '{"value":15}'
```

#### Ответ

```
{
  "id": 0,
  "value": 0,
  "createdAtUtc": "2025-06-20T09:46:33.520Z"
}
```

| Технология | Назначение |
|--|--|
| ASP.NET Core 8 | Бэкенд на Minimal API |
| PostgreSQL | Хранилище данных |
| React + Vite | Фронтенд |
| TailwindCSS | Стили |
| Serilog | Логгирование |
