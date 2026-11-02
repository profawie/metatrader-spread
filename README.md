# metatrader-spread

Realtime bid/ask price and spread streaming from MetaTrader 4 and MetaTrader 5 to the web.

A Windows service connects to the MT4/MT5 server through the MetaQuotes Manager API, listens for tick events and publishes every price update to RabbitMQ. A Spring Boot API consumes the queue and pushes each price to browsers over WebSocket (STOMP). React widgets show live buy/sell prices and spreads by category (forex, indices, stocks, commodities, crypto).

```
MT4 / MT5 server
      │  Manager API (tick events)
      ▼
mt4-price-server / mt5-price-server   (C# .NET, Windows)
      │  RabbitMQ  MT{4,5}Price_<ACCOUNT>_Queue
      ▼
mt4-price-api / mt5-price-api         (Java Spring Boot)
      │  WebSocket STOMP  /topic/mt{4,5}price/<ACCOUNT>
      ▼
spread / spread-account               (React widgets)
```

## Modules

| Folder | Stack | Purpose |
| --- | --- | --- |
| [mt4-price-server](mt4-price-server) | C# .NET Framework 4.7.2, C++/CLI | MT4 connector and the MT4 Manager API .NET wrapper |
| [mt5-price-server](mt5-price-server) | C# .NET Framework 4.7.2 | MT5 connector (Manager API and Web API) |
| [mt4-price-api](mt4-price-api) | Java 8, Spring Boot 2.5 | REST + WebSocket gateway for MT4 prices |
| [mt5-price-api](mt5-price-api) | Java 8, Spring Boot 2.5 | REST + WebSocket gateway for MT5 prices |
| [spread](spread) | React 17, Redux Saga, MDB | Public live spread table |
| [spread-account](spread-account) | React 17, Redux Saga, MDB | Spread table per account type |

## Shared infrastructure

- **MySQL** holds platform connection settings (host, manager login), the symbol filter per account and subscription expiry.
- **RabbitMQ** carries price events from the connectors to the APIs.

## Configuration

No credentials are committed. Each module reads its settings from config files with placeholder values or environment variables; see each module's README.

## Note

The MetaQuotes Manager API DLLs (`mtmanapi64.dll`, `MT5APIManager64.dll`, `MetaQuotes.*.dll`) are proprietary and are not included. Get them from your broker or MetaQuotes and place them next to the project files before building.
