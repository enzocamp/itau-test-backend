using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Enums;
using Itau.Investimentos.Domain.Services;
using Itau.Investimentos.Infrastructure.Interfaces;
namespace Itau.Investimentos.Infrastructure.Services
{
    public class PositionCalculationService : IPositionCalculationService
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly IQuoteRepository _quoteRepository;

        public PositionCalculationService(ITradeRepository tradeRepository, IQuoteRepository quoteRepository)
        {
            _tradeRepository = tradeRepository;
            _quoteRepository = quoteRepository;
        }

        public async Task<Position> CalculatePositionAsync(int userId, int assetId)
        {
            var trades = await _tradeRepository.GetByUserAndAssetLast30DaysAsync(userId, assetId);
            var lastQuote = await _quoteRepository.GetLastQuoteByAssetIdAsync(assetId);

            if (lastQuote is null)
                throw new InvalidOperationException("No quote found for asset.");

            uint totalQuantity = 0;
            decimal totalInvested = 0;

            foreach (var trade in trades)
            {
                if (trade.TradeType == TradeType.BUY)
                {
                    totalQuantity += trade.Quantity;
                    totalInvested += trade.Quantity * trade.UnitPrice + trade.Fee;
                }
                else if (trade.TradeType == TradeType.SELL)
                {
                    totalQuantity -= trade.Quantity;
                }
            }

            decimal avgPrice = totalQuantity > 0 ? totalInvested / totalQuantity : 0;
            decimal pnl = (lastQuote.UnitPrice - avgPrice) * totalQuantity;

            return new Position
            {
                UserId = userId,
                AssetId = assetId,
                Quantity = totalQuantity,
                AveragePrice = avgPrice,
                PnL = pnl
            };
        }
    }
}
