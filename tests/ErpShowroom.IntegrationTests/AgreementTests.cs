using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using ErpShowroom.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.API.Controllers.fin;
using ErpShowroom.Application.fin.Commands;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.IntegrationTests;

[Collection("SharedContainerCollection")]
[Trait("Category", "Integration")]
public class AgreementTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ContainerFixture _fixture;

    public AgreementTests(ContainerFixture fixture)
    {
        _fixture = fixture;
        _factory = new CustomWebApplicationFactory<Program>(_fixture);
    }

    public async Task InitializeAsync() => await _factory.SeedDataAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task CreateHPAgreement_ShouldSucceed()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new CreateHPAgreementCommand(1, 1, 20000m, 12, 12m);

        // Act
        var response = await client.PostAsJsonAsync("/api/Agreements/hp-agreement", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK); // Assuming OK from controller logic
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNullOrEmpty();

        // Verify in DB
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var agreement = await db.HPAgreements
            .Include(a => a.EMISchedules)
            .FirstOrDefaultAsync(a => a.CustomerId == 1);

        agreement.Should().NotBeNull();
        agreement!.EMISchedules.Should().HaveCount(12);
        agreement.InstallmentAmount.Should().BeInRange(7100, 7120);
        
        var firstEMI = agreement.EMISchedules!.OrderBy(e => e.InstallmentNo).First();
        firstEMI.DueDate.Should().BeAfter(DateTime.UtcNow.AddWeeks(3));
    }

    [Fact]
    public async Task AddPayment_ShouldUpdateEMIAndCreateRecord()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // 1. Create Agreement & EMI first
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var agreement = new HPAgreement { Id = 10, CustomerId = 1, ProductId = 1, InstallmentAmount = 7100 };
            agreement.EMISchedules = new List<EMISchedule> 
            { 
                new EMISchedule { Id = 100, InstallmentNo = 1, DueDate = DateTime.UtcNow.AddMonths(1), TotalDue = 7100, PaidAmount = 0 }
            };
            db.HPAgreements.Add(agreement);
            await db.SaveChangesAsync();
        }

        var command = new AddPaymentCommand(10, 100, 5000m, "Cash");

        // Act
        var response = await client.PostAsJsonAsync("/api/Agreements/10/payments", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify in DB
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var emi = await db.EMISchedules.FindAsync(100L);
            var payments = await db.Payments.Where(p => p.EMIId == 100).ToListAsync();

            emi!.PaidAmount.Should().Be(5000m);
            emi.Status.Should().Be(ErpShowroom.Domain.Common.EMIPaymentStatus.Partial);
            payments.Should().HaveCount(1);
            payments.First().Amount.Should().Be(5000m);
        }
    }
}
