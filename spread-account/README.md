# spread-account

Live spread table per account type for MetaTrader prices, built with React 17, Redux Saga and MDB React UI Kit. It is designed to be embedded in a WordPress (Elementor) page.

On load it fetches a price snapshot from `GET {API_URL}/start/{ACCOUNT_CODE}`, then subscribes to `/topic/mt5price/{ACCOUNT_CODE}` over WebSocket (STOMP) and updates bid, ask and spread live. Symbols are grouped by Top Traded, Stocks, Indices, Forex, Commodities and Crypto.

## Configuration

Set the API endpoint and account in `src/constant/url.js`:

```js
export const ACCOUNT_CODE = "SS";
export const API_URL = 'https://api.example.com/mt5price-api-ss';
```

## Run

```bash
yarn install
yarn start      # dev server
yarn build      # production build in build/
```
Deploy: `deploy.cmd` syncs `build/` to an S3 bucket (set your bucket name first).
