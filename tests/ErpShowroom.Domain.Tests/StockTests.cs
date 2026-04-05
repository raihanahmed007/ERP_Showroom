using System;
using System.Linq;
using ErpShowroom.Domain.inv.Entities;
using ErpShowroom.Domain.inv.Events;
using ErpShowroom.Domain.Common.Exceptions;
using FluentAssertions;
using Xunit;

namespace ErpShowroom.Domain.Tests;

[Trait("Category", "Unit")]
public class StockTests
{
    [Fact]
    public void StockBalance_Reserve_ShouldDecreaseOnHandAndIncreaseReserved()
    {
        // Arrange
        var stock = new StockBalance
        {
            ProductId = 1,
            QuantityOnHand = 100,
            QuantityReserved = 0
        };

        // Act
        stock.Reserve(20);

        // Assert
        stock.QuantityOnHand.Should().Be(80);
        stock.QuantityReserved.Should().Be(20);
        stock.DomainEvents.Should().ContainSingle(e => e is StockReservedEvent);
    }

    [Fact]
    public void StockBalance_Reserve_InsufficientStock_ShouldThrowException()
    {
        // Arrange
        var stock = new StockBalance { QuantityOnHand = 10 };

        // Act
        Action reserve = () => stock.Reserve(20);

        // Assert
        reserve.Should().Throw<DomainException>().WithMessage("Insufficient quantity on hand.");
    }

    [Fact]
    public void StockBalance_Release_ShouldIncreaseOnHandAndDecreaseReserved()
    {
        // Arrange
        var stock = new StockBalance
        {
            ProductId = 1,
            QuantityOnHand = 80,
            QuantityReserved = 20
        };

        // Act
        stock.Release(10);

        // Assert
        stock.QuantityOnHand.Should().Be(90);
        stock.QuantityReserved.Should().Be(10);
        stock.DomainEvents.Should().ContainSingle(e => e is StockReleasedEvent);
    }

    [Fact]
    public void StockBalance_Release_InsufficientReserved_ShouldThrowException()
    {
        // Arrange
        var stock = new StockBalance { QuantityReserved = 5 };

        // Act
        Action release = () => stock.Release(10);

        // Assert
        release.Should().Throw<DomainException>().WithMessage("Insufficient reserved quantity.");
    }
}
