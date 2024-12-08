using Microsoft.EntityFrameworkCore;
using Moq;
using TradesApi.Infrastructure.Models;
using TradesApi.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Xunit;
using TradesApi.Infrastructure;

namespace TradesApi.Tests
{
    public class TradesRepositoryTests
    {
        private readonly Mock<ILogger<TradesRepository>> _mockLogger;
        private readonly TradeDbContext _dbContext;
        private readonly TradesRepository _repository;

        public TradesRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TradeDbContext>()
                .UseInMemoryDatabase(databaseName: "TradeTestDb")
                .Options;

            _dbContext = new TradeDbContext(options);
            
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _mockLogger = new Mock<ILogger<TradesRepository>>();
            _repository = new TradesRepository(_mockLogger.Object, _dbContext);
        }

        [Fact]
        public async Task GetAllTrades_ShouldReturnAllTrades()
        {
            var trade1 = new Trade
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                CurrencyCode = "USD",
                Fee = 1,
                TradeDate = DateTime.UtcNow,
                User = "user1"
            };
            var trade2 = new Trade
            {
                Id = Guid.NewGuid(),
                Amount = 200,
                CurrencyCode = "EUR",
                Fee = 2,
                TradeDate = DateTime.UtcNow,
                User = "user2"
            };

            _dbContext.Trades.Add(trade1);
            _dbContext.Trades.Add(trade2);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetAllTrades();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Id == trade1.Id);
            Assert.Contains(result, t => t.Id == trade2.Id);
        }

        [Fact]
        public async Task GetTradeById_ShouldReturnTrade()
        {
            var trade = new Trade
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                CurrencyCode = "USD",
                Fee = 1,
                TradeDate = DateTime.UtcNow,
                User = "user1"
            };

            _dbContext.Trades.Add(trade);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetTradeById(trade.Id);

            Assert.NotNull(result);
            Assert.Equal(trade.Id, result.Id);
            Assert.Equal(trade.Amount, result.Amount);
            Assert.Equal(trade.CurrencyCode, result.CurrencyCode);
            Assert.Equal(trade.User, result.User);
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnTradeId()
        {
            var trade = new Trade
            {
                Amount = 100,
                CurrencyCode = "USD",
                Fee = 1,
                TradeDate = DateTime.UtcNow,
                User = "user1"
            };

            var result = await _repository.CreateTrade(trade);

            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(trade.Id, result);
            var createdTrade = await _dbContext.Trades.FindAsync(result);
            Assert.NotNull(createdTrade);
            Assert.Equal(trade.Amount, createdTrade.Amount);
        }
    }
}
