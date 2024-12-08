using TradesApi.Core.Models;

public interface ITradesService
{
    Task<List<GetTradeResponse>> GetAllTrades();
    Task<GetTradeResponse> GetTradeById(Guid id);
    Task<Guid> CreateTrade(CreateTradeRequest tradeRequest);
}