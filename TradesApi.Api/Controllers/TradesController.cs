using Microsoft.AspNetCore.Mvc;
using TradesApi.Core.Models;
using TradesApi.Infrastructure;
using TradesApi.Infrastructure.Services;

namespace TradesApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly TradeDbContext _context;
        private readonly MessageQueueService _messageQueueService;
        private readonly ITradesService _tradesService;

        public TradesController(TradeDbContext context, MessageQueueService messageQueueService, ITradesService tradesService)
        {
            _context = context;
            _messageQueueService = messageQueueService;
            _tradesService = tradesService;
        }

        [HttpPost]
        public async Task<IActionResult> PostTrade([FromBody] CreateTradeRequest trade)
        {
            trade.TradeDate = DateTime.UtcNow;

            var tradeId = await _tradesService.CreateTrade(trade);
            var message = $"Trade executed by {trade.User} with - {trade.Amount} {trade.CurrencyCode} for a fee of - {trade.Fee}";
            _messageQueueService.SendTradeMessage(message);

            return Ok("{\"tradeId\":\"" + tradeId + "\"}");
        }

        [HttpGet]
        public async Task<IActionResult> GetTrades()
        {
            var trades = await _tradesService.GetAllTrades();
            return Ok(trades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTradeById(Guid id)
        {
            var trade = await _tradesService.GetTradeById(id);

            if (trade == null)
            {
                return NotFound();
            }

            return Ok(trade);
        }
    }
}
