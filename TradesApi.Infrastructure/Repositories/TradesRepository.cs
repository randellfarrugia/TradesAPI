
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TradesApi.Infrastructure.Interfaces;
using TradesApi.Infrastructure.Models;

namespace TradesApi.Infrastructure.Repositories
{
    public class TradesRepository : ITradesRepository
    {
        private readonly ILogger<TradesRepository> _logger;
        private readonly TradeDbContext _context;


        public TradesRepository(ILogger<TradesRepository> logger, TradeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<Trade>> GetAllTrades()
        {
            _logger.LogInformation("Getting all trades");
            return await _context.Trades.ToListAsync();
        }

        public async Task<Trade> GetTradeById(Guid id)
        {
            _logger.LogInformation($"Getting Trade by id - {id}");
            return await _context.Trades.Where(x => x.Id == id)?.FirstOrDefaultAsync();
        }

        public async Task<Guid> CreateTrade(Trade trade)
        {
            var tradeId = Guid.NewGuid();
            trade.Id = tradeId;

            _logger.LogInformation($"Creating Trade with id - {tradeId}");

            _context.Trades.Add(trade);
            _context.SaveChanges();

            return tradeId;
        }
    }
}
