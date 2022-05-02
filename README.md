# StockPriceAlerts
.NET 6 Console App to send free SMS stock price alerts

1) get free API key from alphavantage https://www.alphavantage.co/
2) open a twilio trial account with your phone number (the one on which you want to receive the SMSs) https://www.twilio.com/
3) build the solution
4) open your BIN folder and modify `stockAlertsSettings.json` inserting alpha vantage and twilio values, and your stock price alerts

![alt text](https://maciti.github.io/assets/StockPriceAlerts/settings.png)

you can set up a cron job in linux or a task using task scheduler in windows to execute the program daily or how often you desire.







