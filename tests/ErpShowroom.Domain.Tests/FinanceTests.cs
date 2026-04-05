using System;
using System.Linq;
using ErpShowroom.Domain.fin.Entities;
using FluentAssertions;
using Xunit;

namespace ErpShowroom.Domain.Tests;

[Trait("Category", "Unit")]
public class FinanceTests
{
    [Fact]
    public void HPAgreement_CalculateEMI_ShouldCalculateCorrectly()
    {
        // Arrange
        var agreement = new HPAgreement
        {
            ProductPrice = 100000m,
            DownPayment = 20000m,
            InterestRate = 12m,
            InstallmentCount = 12
        };

        // Act
        agreement.CalculateEMI();

        // Assert
        // Formula check: (100000-20000) * (0.01 * (1.01)^12) / ((1.01)^12 - 1) ≈ 7107.87
        agreement.FinanceAmount.Should().Be(80000m);
        agreement.InstallmentAmount.Should().BeInRange(7100m, 7120m);
        agreement.TotalPayable.Should().Be(agreement.InstallmentAmount * 12);
    }

    [Theory]
    [InlineData(100000, 20000, 0, 10, 8000)] // Zero interest
    [InlineData(100000, 0, 12, 12, 8884.88)] // Zero down payment
    [InlineData(12000, 0, 12, 1, 12120)] // Single installment
    public void HPAgreement_CalculateEMI_EdgeCases(decimal price, decimal down, decimal rate, int count, decimal expectedEmi)
    {
        // Arrange
        var agreement = new HPAgreement
        {
            ProductPrice = price,
            DownPayment = down,
            InterestRate = rate,
            InstallmentCount = count
        };

        // Act
        agreement.CalculateEMI();

        // Assert
        agreement.InstallmentAmount.Should().BeApproximately(expectedEmi, 5m);
    }

    [Fact]
    public void EMISchedule_CalculatePenalty_ShouldCreatePenaltyRecord()
    {
        // Arrange
        var emi = new EMISchedule
        {
            TotalDue = 5000m,
            PenaltyAmount = 0
        };

        // Act
        var result = emi.CalculatePenalty(10, 2m);

        // Assert
        result.Should().Be(1000m);
        emi.PenaltyAmount.Should().Be(1000m);
        emi.Penalties.Should().NotBeNull().And.HaveCount(1);
        emi.Penalties!.First().PenaltyAmount.Should().Be(1000m);
        emi.Penalties.First().DaysOverdue.Should().Be(10);
    }

    [Fact]
    public void EMISchedule_CalculatePenalty_NoOverdue_ShouldReturnZero()
    {
        // Arrange
        var emi = new EMISchedule
        {
            TotalDue = 5000m,
            PenaltyAmount = 0
        };

        // Act
        var result = emi.CalculatePenalty(0, 2m);

        // Assert
        result.Should().Be(0);
        emi.PenaltyAmount.Should().Be(0);
        emi.Penalties.Should().BeNullOrEmpty();
    }
}
