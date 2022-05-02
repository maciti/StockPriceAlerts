using Newtonsoft.Json;
using StockPriceAlerts;
using System.Reflection;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

var logs = DateTime.Now.ToString();

var currentLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

var stockAlertsSettings = JsonConvert.DeserializeObject<StockAlertSettings>(File.ReadAllText($"{currentLocation}/stockAlertsSettings.json"));

try
{
    var alphaVantageQuery = $"query?function=GLOBAL_QUOTE&symbol=##symbol##&apikey={stockAlertsSettings.AlphaVantage.ApiKey}";

    var alert = string.Empty;

    var alphaVantageClient = new HttpClient { BaseAddress = new Uri("https://www.alphavantage.co/") };

    foreach (var priceAlert in stockAlertsSettings.PriceAlerts)
    {
        var result = await alphaVantageClient.GetAsync(alphaVantageQuery.Replace("##symbol##", priceAlert.StockSymbol));

        dynamic globalQuote = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(await result.Content.ReadAsStringAsync())["Global Quote"];

        var quoteDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(globalQuote.ToString());

        var currentPrice = Convert.ToDecimal(quoteDict["05. price"]);

        if (priceAlert.PriceLowerThan.HasValue && currentPrice < priceAlert.PriceLowerThan)
        {
            alert += $"{priceAlert.StockSymbol}:{currentPrice.ToString("F2")} ";
        }

        if (priceAlert.PriceHigherThan.HasValue && currentPrice > priceAlert.PriceHigherThan)
        {
            alert += $"{priceAlert.StockSymbol}:{currentPrice.ToString("F2")} ";
        }
    }

    if (!string.IsNullOrEmpty(alert))
    {
        TwilioClient.Init(stockAlertsSettings.Twilio.AccountSid, stockAlertsSettings.Twilio.AuthToken);

        var message = MessageResource.Create(
            body: alert,
            from: new Twilio.Types.PhoneNumber(stockAlertsSettings.Twilio.SmsFrom),
            to: new Twilio.Types.PhoneNumber(stockAlertsSettings.Twilio.SmsTo)
        );

        logs += $" - MESSAGE SID: {message.Sid}, BODY: {alert}, SENT ";
    }

    logs += " - JOB EXECUTED!";

}
catch (Exception ex)
{
    logs += ex.Message;
}
finally
{
    if (stockAlertsSettings.LogOnFile)
    {
        using (File.OpenWrite($"{currentLocation}/logs.txt")) ;

        File.AppendAllText($"{currentLocation}/logs.txt", logs + Environment.NewLine);
    }
}
