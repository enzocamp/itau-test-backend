using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Itau.Investimentos.Domain.Entities;
using Itau.Investimentos.Domain.Enums;
using Itau.Investimentos.Domain.Services;
using Itau.Investimentos.Infrastructure.Interfaces;
using Itau.Investimentos.Infrastructure.Services;
using Moq;
using Xunit;

namespace Itau.Investimentos.Tests
{
    public class PositionCalculationServiceTests
    {
        private readonly Mock<ITradeRepository> _tradeRepoMock = new();
        private readonly Mock<IQuoteRepository> _quoteRepoMock = new();

        private PositionCalculationService CreateService()
        {
            return new PositionCalculationService(_tradeRepoMock.Object, _quoteRepoMock.Object);
        }

        [Fact]
        public async Task CalculatePositionAsync_WithSingleBuyTrade_ReturnsCorrectPosition()
        {
            _tradeRepoMock.Setup(r => r.GetByUserAndAssetLast30DaysAsync(1, 1)).ReturnsAsync(new List<Trade>
            {
                new Trade { Quantity = 10, UnitPrice = 10, Fee = 1, TradeType = TradeType.BUY }
            });

            _quoteRepoMock.Setup(r => r.GetLastQuoteByAssetIdAsync(1)).ReturnsAsync(new Quote { UnitPrice = 12 });

            var service = CreateService();
            var result = await service.CalculatePositionAsync(1, 1);

            Assert.Equal(10u, result.Quantity);
            Assert.Equal(10.1m, result.AveragePrice);
            Assert.Equal(19m, result.PnL);
        }

        [Fact]
        public async Task CalculatePositionAsync_WithMultipleTrades_ComputesWeightedAverage()
        {
            _tradeRepoMock.Setup(r => r.GetByUserAndAssetLast30DaysAsync(1, 1)).ReturnsAsync(new List<Trade>
            {
                new Trade { Quantity = 5, UnitPrice = 10, Fee = 1, TradeType = TradeType.BUY },
                new Trade { Quantity = 10, UnitPrice = 20, Fee = 2, TradeType = TradeType.BUY }
            });

            _quoteRepoMock.Setup(r => r.GetLastQuoteByAssetIdAsync(1)).ReturnsAsync(new Quote { UnitPrice = 25 });

            var service = CreateService();
            var result = await service.CalculatePositionAsync(1, 1);

            Assert.Equal(15u, result.Quantity);
            Assert.Equal(16.866666666666666666666666667m, result.AveragePrice);
        }

        [Fact]
        public async Task CalculatePositionAsync_WithSellReducesQuantity()
        {
            _tradeRepoMock.Setup(r => r.GetByUserAndAssetLast30DaysAsync(1, 1)).ReturnsAsync(new List<Trade>
            {
                new Trade { Quantity = 10, UnitPrice = 10, Fee = 1, TradeType = TradeType.BUY },
                new Trade { Quantity = 4, TradeType = TradeType.SELL }
            });

            _quoteRepoMock.Setup(r => r.GetLastQuoteByAssetIdAsync(1)).ReturnsAsync(new Quote { UnitPrice = 15 });

            var service = CreateService();
            var result = await service.CalculatePositionAsync(1, 1);

            Assert.Equal(6u, result.Quantity);
        }

        [Fact]
        public async Task CalculatePositionAsync_WithEmptyTrades_ReturnsZeroPosition()
        {
            _tradeRepoMock.Setup(r => r.GetByUserAndAssetLast30DaysAsync(1, 1)).ReturnsAsync(new List<Trade>());
            _quoteRepoMock.Setup(r => r.GetLastQuoteByAssetIdAsync(1)).ReturnsAsync(new Quote { UnitPrice = 20 });

            var service = CreateService();
            var result = await service.CalculatePositionAsync(1, 1);

            Assert.Equal(0u, result.Quantity);
            Assert.Equal(0, result.AveragePrice);
            Assert.Equal(0, result.PnL);
        }

        [Fact]
        public async Task CalculatePositionAsync_NoQuoteFound_ThrowsInvalidOperation()
        {
            _tradeRepoMock.Setup(r => r.GetByUserAndAssetLast30DaysAsync(1, 1)).ReturnsAsync(new List<Trade>());
            _quoteRepoMock.Setup(r => r.GetLastQuoteByAssetIdAsync(1)).ReturnsAsync((Quote?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.CalculatePositionAsync(1, 1));
        }
    }
}

