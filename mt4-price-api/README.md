# mt4-price-api

Spring Boot gateway that streams MetaTrader 4 prices to the browser.

It consumes price events from RabbitMQ queue `MT4Price_<ACCOUNT_CODE>_Queue` (published by [mt4-price-server](../mt4-price-server)), broadcasts each one over WebSocket and caches the last price per symbol on disk so new clients get a snapshot straight away.

## Endpoints

| Method | Path | Description |
| --- | --- | --- |
| WS (STOMP) | `/stomp` → subscribe `/topic/mt4price/{accountCode}` | Live price stream |
| GET | `/start/{accountCode}` | Latest cached price for every symbol enabled for the account |
| GET | `/price/{symbolCode}` | Latest cached price for one symbol |
| GET | `/price/list` | Symbols currently in the cache |

`static/docs.html` documents the API and `static/sample.html` is a small STOMP client for testing.

## Configuration

`src/main/resources/application.properties` reads from environment variables:

| Variable | Default |
| --- | --- |
| `ACCOUNT_CODE` | `SS` |
| `CACHE_DIRECTORY` | `./cache` |
| `RABBITMQ_HOST`, `RABBITMQ_PORT`, `RABBITMQ_USERNAME`, `RABBITMQ_PASSWORD` | `localhost`, `5672`, `guest`, `guest` |
| `MYSQL_HOST`, `MYSQL_PORT`, `MYSQL_USERNAME`, `MYSQL_PASSWORD` | `localhost`, `3306`, `spread_realtime`, `change-me` |

## Build and run

```bash
./mvnw clean package          # builds a WAR for Tomcat
./mvnw spring-boot:run        # or run locally
```

Requires Java 8+, MySQL and RabbitMQ.
