using System;
using System.Threading.Tasks;
using Testcontainers.SqlEdge;
using Testcontainers.Redis;
using Testcontainers.RabbitMq;
using Xunit;

namespace ErpShowroom.IntegrationTests;

public class ContainerFixture : IAsyncLifetime
{
    public SqlEdgeContainer SqlContainer { get; } = new SqlEdgeBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    public RedisContainer RedisContainer { get; } = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();

    public RabbitMqContainer RabbitMqContainer { get; } = new RabbitMqBuilder()
        .WithImage("rabbitmq:management")
        .Build();

    public async Task InitializeAsync()
    {
        await Task.WhenAll(
            SqlContainer.StartAsync(),
            RedisContainer.StartAsync(),
            RabbitMqContainer.StartAsync()
        );
    }

    public async Task DisposeAsync()
    {
        await Task.WhenAll(
            SqlContainer.DisposeAsync().AsTask(),
            RedisContainer.DisposeAsync().AsTask(),
            RabbitMqContainer.DisposeAsync().AsTask()
        );
    }
}

[CollectionDefinition("SharedContainerCollection")]
public class ContainerCollection : ICollectionFixture<ContainerFixture> { }
