# V. Samusev - Admin Dashboard - Mira Games

> ���������� ���������������� ������ � ASP.NET Core 8 API � React-����������

## ������ API (http://localhost:5000)

### Backend (ASP.NET Core API)

```
bash
cd backend/api
dotnet restore
dotnet run
```

## ������ API (http://localhost:5173)

### Frontend (React + Vite)

```
bash
cd frontend
npm install
npm run dev
```

## ������ ��� �����

- Email: admin@mirra.dev
- Password: admin123  

## ������� ��������

### 1. �����������

#### ������

```
curl -X POST http://localhost:5000/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@mirra.dev","password":"admin123"}'
```

#### �����

```
{
  "token": "string",
  "refreshToken": "string"
}
```

#### ���������� ������ � ���������

```
Authorization: Bearer [token]
```

### 2. �������� ������ ��������

#### ������

```
curl http://localhost:5000/clients
  -H "Authorization: Bearer [token]"
```

#### �����

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

### 3. �������� ��������� N ��������

#### ������

```
curl http://localhost:5000/payments?take=5
  -H "Authorization: Bearer [token]"
```

#### �����

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

### 4. �������� ������� ����

#### ������

```
curl http://localhost:5000/rates
```

#### �����

```
{
  "id": 0,
  "value": 0,
  "createdAtUtc": "2025-06-20T09:46:33.520Z"
}
```

### 5. �������� ����

#### ������

```
curl -X POST http://localhost:5000/rates \
  -H "Authorization: Bearer [token]" \
  -H "Content-Type: application/json" \
  -d '{"value":15}'
```

#### �����

```
{
  "id": 0,
  "value": 0,
  "createdAtUtc": "2025-06-20T09:46:33.520Z"
}
```

| ���������� | ���������� |
|--|--|
| ASP.NET Core 8 | ������ �� Minimal API |
| PostgreSQL | ��������� ������ |
| React + Vite | �������� |
| TailwindCSS | ����� |
| Serilog | ������������ |
