using Microsoft.EntityFrameworkCore;
using TradesApi.Infrastructure.Models;

namespace TradesApi.Infrastructure
{
    public class TradeDbContext : DbContext
    {
        public TradeDbContext (DbContextOptions<TradeDbContext> options) : base(options) { }

        public DbSet<Trade> Trades { get; set; }
    }
}
