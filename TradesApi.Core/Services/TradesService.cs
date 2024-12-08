
using Microsoft.Extensions.Logging;
using TradesApi.Core.Models;
using TradesApi.Infrastructure.Interfaces;
using TradesApi.Infrastructure.Models;

namespace TradesApi.Core.Services
{
    public class TradesService : ITradesService
    {
        private readonly ILogger<TradesService> _logger;
        private readonly ITradesRepository _repository;
        public TradesService(ILogger<TradesService> logger, ITradesRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<List<GetTradeResponse>> GetAllTrades()
        {
            var tradesList = await _repository.GetAllTrades();
            var tradesResponseList = new List<GetTradeResponse>();

            foreach (var trade in tradesList)
            {
                var tradeResponse = MapTradetoTradeResponse(trade);
                tradesResponseList.Add(tradeResponse);
            }

            return tradesResponseList;
        }

        public async Task<GetTradeResponse> GetTradeById(Guid id)
        {
            var trade = await _repository.GetTradeById(id);
            return MapTradetoTradeResponse(trade);
        }

        public async Task<Guid> CreateTrade(CreateTradeRequest tradeRequest)
        {
            var trade = MapTradeRequestToTrade(tradeRequest);
            return await _repository.CreateTrade(trade);
        }

        //Mappers
        private Trade MapTradeRequestToTrade(CreateTradeRequest trade)
        {
            return new Trade()
            {
                Amount = trade.Amount,
                CurrencyCode = trade.CurrencyCode,
                Fee = trade.Fee,
                TradeDate = trade.TradeDate,
                User = trade.User
            };
        }

        private GetTradeResponse MapTradetoTradeResponse(Trade trade)
        {
            return new GetTradeResponse()
            {
                Amount = trade.Amount,
                CurrencyCode = trade.CurrencyCode,
                Id = trade.Id,
                Fee = trade.Fee,
                TradeDate = trade.TradeDate,
                User = trade.User
            };
        }
    }
}
