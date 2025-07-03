using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Enums;
using Itau.Investimentos.Domain.Services;
using Itau.Investimentos.Domain.Interfaces;
using Microsoft.Extensions.Logging;
namespace Itau.Investimentos.Infrastructure.Services
{
    public class PositionCalculationService : IPositionCalculationService
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly ILogger<PositionCalculationService> _logger;

        public PositionCalculationService(ITradeRepository tradeRepository, IQuoteRepository quoteRepository, IPositionRepository positionRepository,
            ILogger<PositionCalculationService> logger)
        {
            _tradeRepository = tradeRepository;
            _quoteRepository = quoteRepository;
            _positionRepository = positionRepository;
            _logger = logger;
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

        public async Task RecalculatePnLAsync(int assetId)
        {
            var lastQuote = await _quoteRepository.GetLastQuoteByAssetIdAsync(assetId);
            if (lastQuote is null)
            {
                _logger.LogWarning("No quote found to recalculate P&L for AssetId={AssetId}", assetId);
                return;
            }

            var positions = await _positionRepository.GetByAssetIdAsync(assetId);
            if (positions is null || !positions.Any())
            {
                _logger.LogInformation("No positions found for AssetId={AssetId}", assetId);
                return;
            }

            foreach (var position in positions)
            {
                position.PnL = (lastQuote.UnitPrice - position.AveragePrice) * position.Quantity;
                await _positionRepository.AddOrUpdateAsync(position);

                _logger.LogInformation("Updated P&L: UserId={UserId}, AssetId={AssetId}, PnL={PnL}",
                    position.UserId, position.AssetId, position.PnL);
            }
        }
     }
}
