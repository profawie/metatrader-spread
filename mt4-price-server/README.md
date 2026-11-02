# mt4-price-server

Windows console service that connects to a MetaTrader 4 server and publishes realtime bid/ask prices to RabbitMQ.

## Projects

- **MT4Manager/** – C++/CLI .NET wrapper around the MetaQuotes MT4 Manager API (`MT4ManagerAPI.h`), with a small `TestMT4Manager` console app. See [MT4Manager/README.md](MT4Manager/README.md).
- **MT4Price.Server/** – the connector service (.NET Framework 4.7.2).

## How it works

1. Reads `Account_Code` from `App.config` and loads the platform connection (host, port, manager login) and the enabled symbols for that account from MySQL.
2. Checks the account subscription expiry.
3. Connects to RabbitMQ and declares `MT4Price_<ACCOUNT>_Action` (commands in) and `MT4Price_<ACCOUNT>_Queue` (prices out).
4. Logs in to MT4 with pumping mode, and for each tick of a filtered symbol publishes a JSON price (symbol, bid, ask, spread, high, low, digits, time, platform) to the queue.
5. `Program` loops and restarts `ScheduleService` when the MT4 connection drops.

## Configuration

Edit `MT4Price.Server/MT4Price.Server/App.config` (placeholders are committed, do not commit real values):

`RabbitMQ_Host`, `RabbitMQ_Port`, `RabbitMQ_Username`, `RabbitMQ_Password`, `Mysql_Host`, `Mysql_Port`, `Mysql_Login`, `Mysql_Password`, `Mysql_Database`, `Account_Code`.

## Build

Open `MT4Price.Server/MT4Price.Server.sln` in Visual Studio, restore NuGet packages, and copy `mtmanapi64.dll` (from MetaQuotes / your broker) into the project folder. Build for x64.
