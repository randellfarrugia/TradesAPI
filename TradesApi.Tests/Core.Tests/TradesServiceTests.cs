using Moq;
using TradesApi.Core.Models;
using TradesApi.Infrastructure.Interfaces;
using TradesApi.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Xunit;
using TradesApi.Core.Services;

namespace TradesApi.Tests
{
    public class TradesServiceTests
    {
        private readonly Mock<ILogger<TradesService>> _mockLogger;
        private readonly Mock<ITradesRepository> _mockRepository;
        private readonly TradesService _tradesService;

        public TradesServiceTests()
        {
            _mockLogger = new Mock<ILogger<TradesService>>();
            _mockRepository = new Mock<ITradesRepository>();
            _tradesService = new TradesService(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task GetAllTrades_ShouldReturnMappedTradeResponses()
        {
            var tradeList = new List<Trade>
            {
                new Trade { Id = Guid.NewGuid(), Amount = 100, CurrencyCode = "USD", Fee = 1, TradeDate = DateTime.UtcNow, User = "user1" },
                new Trade { Id = Guid.NewGuid(), Amount = 200, CurrencyCode = "EUR", Fee = 2, TradeDate = DateTime.UtcNow, User = "user2" }
            };

            _mockRepository.Setup(repo => repo.GetAllTrades()).ReturnsAsync(tradeList);

            var result = await _tradesService.GetAllTrades();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(tradeList[0].Amount, result[0].Amount);
            Assert.Equal(tradeList[1].Amount, result[1].Amount);
        }

        [Fact]
        public async Task GetTradeById_ShouldReturnMappedTradeResponse()
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

            _mockRepository.Setup(repo => repo.GetTradeById(It.IsAny<Guid>())).ReturnsAsync(trade);

            var result = await _tradesService.GetTradeById(trade.Id);

            Assert.NotNull(result);
            Assert.Equal(trade.Id, result.Id);
            Assert.Equal(trade.Amount, result.Amount);
            Assert.Equal(trade.CurrencyCode, result.CurrencyCode);
            Assert.Equal(trade.Fee, result.Fee);
            Assert.Equal(trade.User, result.User);
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnTradeId()
        {
            var tradeRequest = new CreateTradeRequest
            {
                Amount = 100,
                CurrencyCode = "USD",
                Fee = 1,
                TradeDate = DateTime.UtcNow,
                User = "user1"
            };

            var trade = new Trade
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                CurrencyCode = "USD",
                Fee = 1,
                TradeDate = DateTime.UtcNow,
                User = "user1"
            };

            _mockRepository.Setup(repo => repo.CreateTrade(It.IsAny<Trade>())).ReturnsAsync(trade.Id);

            var result = await _tradesService.CreateTrade(tradeRequest);

            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(trade.Id, result);
        }
    }
}
