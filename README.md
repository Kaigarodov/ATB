## Description
[Artsofte Test Back](https://github.com/Artsofte-Inc/test-back/blob/main/README.md) repository.

## Running the app
### Сборка
```bash
# development
$ docker-compose up   
```
### Подключение
```
Порт хоста 4050
Приложение доступно на url:
https://localhost:4050/
https://localhost:4050/account/...
https://localhost:4050/swagger
```

### Конфигурация хранилища
файл конфигурации: appsetting.json
```json
{
  "Database":{
    "StorageType": "PostgreSql"
  }
}
```
```
StorageType : ["PostgreSql" | "LocalStorage"]
```
