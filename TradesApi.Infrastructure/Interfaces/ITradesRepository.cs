using TradesApi.Infrastructure.Models;

namespace TradesApi.Infrastructure.Interfaces;
public interface ITradesRepository
{
    Task<List<Trade>> GetAllTrades();
    Task<Trade> GetTradeById(Guid id);
    Task<Guid> CreateTrade(Trade tradeRequest);

}