namespace StockPriceAlerts;

public class StockAlertSettings
{
    public AlphaVantage AlphaVantage { get; set; }

    public Twilio Twilio { get; set; }

    public List<PriceAlert> PriceAlerts { get; set; }

    public bool LogOnFile { get; set; }
}

public class AlphaVantage
{
    public string ApiKey { get; set; }
}

public class Twilio
{
    public string AccountSid { get; set; }

    public string AuthToken { get; set; }

    public string SmsFrom { get; set; }

    public string SmsTo { get; set; }
}

public class PriceAlert
{
    public string StockSymbol { get; set; }

    public decimal? PriceLowerThan { get; set; }

    public decimal? PriceHigherThan { get; set; }
}
