namespace TradesApi.Infrastructure.Models
{
    public class Trade
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public DateTime TradeDate { get; set; }
    }
}
