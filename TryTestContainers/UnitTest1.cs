using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Configurations.MessageBrokers;
using DotNet.Testcontainers.Containers.Modules.Abstractions;
using DotNet.Testcontainers.Containers.Modules.Databases;
using DotNet.Testcontainers.Containers.Modules.MessageBrokers;
using MySql.Data.MySqlClient;
using NUnit.Framework;

namespace TryTestContainers;

public sealed class UnitTest1
{
    private readonly TestcontainerDatabase _testcontainers = new TestcontainersBuilder<MySqlTestcontainer>()
        .WithDatabase(new MySqlTestcontainerConfiguration
        {
            Database = "db",
            Username = "mysql",
            Password = "mysql",
        })
        .Build();
    private readonly RabbitMqTestcontainer _testMessageBroker = new TestcontainersBuilder<RabbitMqTestcontainer>()
        .WithMessageBroker(new RabbitMqTestcontainerConfiguration())
        .Build();
    
    [SetUp]
    public async Task SetUp()
    {
        await _testcontainers.StartAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _testcontainers.DisposeAsync().AsTask();
    }

    [Test]
    public void ExecuteCommand()
    {
        using MySqlConnection connection = new MySqlConnection(_testcontainers.ConnectionString);
        using var command = new MySqlCommand();
        connection.Open();
        command.Connection = connection;
        command.CommandText = "SELECT 1";
        command.ExecuteReader();
    }
}