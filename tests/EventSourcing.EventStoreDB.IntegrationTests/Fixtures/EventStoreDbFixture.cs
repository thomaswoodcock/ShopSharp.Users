using Docker.DotNet;
using Docker.DotNet.Models;
using EventStore.Client;
using Polly;
using Polly.Extensions.Http;

namespace ShopSharp.Users.EventSourcing.EventStoreDB.Fixtures;

public class EventStoreDbFixture : IAsyncLifetime
{
    private const string DockerImageName = "eventstore/eventstore:22.10.1-buster-slim";
    private const string ContainerName = "eventstoredb-test";
    private const string ContainerPort = "2114";
    private const string ContainerNetworkName = "eventstoredb-test-network";

    public const string ConnectionString = $"esdb://localhost:{ContainerPort}?tls=false";

    private readonly DockerClient _dockerClient;
    private readonly HttpClient _httpClient = new();

    public EventStoreDbFixture()
    {
        DockerClientConfiguration dockerConfiguration = new();
        _dockerClient = dockerConfiguration.CreateClient();

        var eventStoreClientSettings = EventStoreClientSettings.Create(ConnectionString);
        Client = new EventStoreClient(eventStoreClientSettings);
    }

    public EventStoreClient Client { get; }

    public async Task InitializeAsync()
    {
        await RemoveDockerResourcesIfNeeded()
            .ConfigureAwait(false);

        await CreateDockerResources()
            .ConfigureAwait(false);

        await WaitForEventStoreDbToBeReadyAsync()
            .ConfigureAwait(false);
    }

    public async Task DisposeAsync()
    {
        await RemoveDockerResourcesIfNeeded()
            .ConfigureAwait(false);

        _dockerClient.Dispose();
        _httpClient.Dispose();
    }

    private Task CreateEventStoreDbTestNetwork()
    {
        return _dockerClient.Networks.CreateNetworkAsync(new NetworksCreateParameters { Name = ContainerNetworkName });
    }

    private async Task PullDockerImageIfNeeded()
    {
        ImagesListParameters imagesListParameters = new()
        {
            Filters = new Dictionary<string, IDictionary<string, bool>>
            {
                { "reference", new Dictionary<string, bool> { { DockerImageName, true } } }
            }
        };

        var existingImages = await _dockerClient.Images.ListImagesAsync(imagesListParameters)
            .ConfigureAwait(false);

        if (!existingImages.Any())
        {
            await _dockerClient.Images.CreateImageAsync(
                    new ImagesCreateParameters { FromImage = DockerImageName }, null, new Progress<JSONMessage>())
                .ConfigureAwait(false);
        }
    }

    private async Task StartEventStoreDbTestContainer()
    {
        await PullDockerImageIfNeeded()
            .ConfigureAwait(false);

        CreateContainerParameters createContainerParameters = new()
        {
            Image = DockerImageName,
            Name = ContainerName,
            Env = new List<string> { "EVENTSTORE_INSECURE=true", "EVENTSTORE_MEM_DB=true" },
            ExposedPorts = new Dictionary<string, EmptyStruct> { { "2113", new EmptyStruct() } },
            HostConfig = new HostConfig
            {
                NetworkMode = ContainerNetworkName,
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    { "2113", new List<PortBinding> { new() { HostPort = ContainerPort } } }
                }
            }
        };

        var container = await _dockerClient.Containers.CreateContainerAsync(createContainerParameters)
            .ConfigureAwait(false);

        await _dockerClient.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
    }

    private async Task RemoveNetworkIfExists()
    {
        var networks = await _dockerClient.Networks.ListNetworksAsync(new NetworksListParameters
            {
                Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    { "name", new Dictionary<string, bool> { { ContainerNetworkName, true } } }
                }
            })
            .ConfigureAwait(false);

        if (networks.SingleOrDefault() is { } existingNetwork)
        {
            await _dockerClient.Networks.DeleteNetworkAsync(existingNetwork.ID)
                .ConfigureAwait(false);
        }
    }

    private async Task StopAndRemoveContainerIfExists()
    {
        var containers =
            await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters
                {
                    Filters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        { "name", new Dictionary<string, bool> { { ContainerName, true } } }
                    },
                    Limit = 1
                })
                .ConfigureAwait(false);

        if (containers.SingleOrDefault() is { } existingContainer)
        {
            await _dockerClient.Containers.StopContainerAsync(existingContainer.ID, new ContainerStopParameters())
                .ConfigureAwait(false);

            await _dockerClient.Containers.RemoveContainerAsync(existingContainer.ID, new ContainerRemoveParameters())
                .ConfigureAwait(false);
        }
    }

    private async Task CreateDockerResources()
    {
        await CreateEventStoreDbTestNetwork()
            .ConfigureAwait(false);

        await StartEventStoreDbTestContainer()
            .ConfigureAwait(false);
    }

    private async Task RemoveDockerResourcesIfNeeded()
    {
        await StopAndRemoveContainerIfExists()
            .ConfigureAwait(false);

        await RemoveNetworkIfExists()
            .ConfigureAwait(false);
    }

    private async Task WaitForEventStoreDbToBeReadyAsync()
    {
        const string healthCheckUrl = $"http://localhost:{ContainerPort}/health/live";
        const int retryCount = 10;
        const int retryIntervalSeconds = 2;

        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(retryCount, _ => TimeSpan.FromSeconds(retryIntervalSeconds));

        await retryPolicy.ExecuteAsync(async () => await _httpClient.GetAsync(healthCheckUrl).ConfigureAwait(false))
            .ConfigureAwait(false);
    }
}
