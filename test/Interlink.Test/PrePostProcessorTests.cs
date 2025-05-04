namespace Interlink.Test;

public class PrePostProcessorTests
{
    [Fact]
    public async Task PreAndPostProcessors_AreCalled()
    {
        // Arrange
        SamplePreProcessor.Reset();
        SamplePostProcessor.Reset();

        var services = new ServiceCollection();
        services.AddInterlink(assemblies: [typeof(SampleRequestHandler).Assembly]);

        var provider = services.BuildServiceProvider();
        var sender = provider.GetRequiredService<ISender>();

        // Act
        var result = await sender.Send(new SampleRequest());

        // Assert
        Assert.Equal("Handled", result);
        Assert.True(SamplePreProcessor.WasCalled);
        Assert.True(SamplePostProcessor.WasCalled);
    }
}

public class SamplePreProcessor : IRequestPreProcessor<SampleRequest>
{
    public static bool WasCalled { get; private set; }

    public Task Process(SampleRequest request, CancellationToken cancellationToken)
    {
        WasCalled = true;
        return Task.CompletedTask;
    }

    public static void Reset() => WasCalled = false;
}

public class SamplePostProcessor : IRequestPostProcessor<SampleRequest, string>
{
    public static bool WasCalled { get; private set; }

    public Task Process(SampleRequest request, string response, CancellationToken cancellationToken)
    {
        WasCalled = true;
        return Task.CompletedTask;
    }

    public static void Reset() => WasCalled = false;
}

public class SampleRequest : IRequest<string> { }

public class SampleRequestHandler : IRequestHandler<SampleRequest, string>
{
    public Task<string> Handle(SampleRequest request, CancellationToken cancellationToken)
        => Task.FromResult("Handled");
}
