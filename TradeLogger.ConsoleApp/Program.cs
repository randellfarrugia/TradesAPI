using System;
using System.Threading.Tasks;

namespace TradeLogger.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var tradeListener = new TradeListener();
            var listenerTask = tradeListener.ListenForTrades();
            
            await listenerTask;
        }
    }
}
