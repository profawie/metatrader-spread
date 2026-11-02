# mt5-price-server

Windows console service that connects to a MetaTrader 5 server and publishes realtime bid/ask prices to RabbitMQ.

## How it works

1. Reads `Account_Code` from `App.config` and loads the platform connection and enabled symbols for that account from MySQL.
2. Checks the account subscription expiry.
3. Connects to RabbitMQ and declares `MT5Price_<ACCOUNT>_Action` (commands in) and `MT5Price_<ACCOUNT>_Queue` (prices out).
4. Connects to MT5 through the MetaQuotes Manager API (`MT5Service`, tick sink in `MT5ServiceEvent`) and the MT5 Web API (`MT5WebAPIService`), then publishes a JSON price (symbol, bid, ask, last, volume, digits, time, platform) for every tick.
5. An `Expired` command on the action queue stops publishing for an expired subscription.

## Configuration

Edit `MT5Price.Server/App.config` (placeholders are committed):

`RabbitMQ_Host`, `RabbitMQ_Port`, `RabbitMQ_Username`, `RabbitMQ_Password`, `Mysql_Host`, `Mysql_Port`, `Mysql_Login`, `Mysql_Password`, `Mysql_Database`, `Account_Code`.

## Build

Open `MT5Price.Server.sln` in Visual Studio, restore NuGet packages, and copy `MT5APIManager64.dll`, `MetaQuotes.MT5CommonAPI64.dll`, `MetaQuotes.MT5ManagerAPI64.dll` and `MetaQuotes.MT5WebAPI.dll` (from the MetaQuotes MT5 Manager SDK) into `MT5Price.Server/`. Build for x64.
