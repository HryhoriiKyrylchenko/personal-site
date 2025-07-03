using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PersonalSite.Application.Services.Common;
using PersonalSite.Infrastructure.BackgroundProcessing.BackgroundQueue;

namespace PersonalSite.Application.Tests.Services;

public class BackgroundPublisherTests
{
    private readonly Mock<IBackgroundQueue> _queueMock;
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock;
    private readonly Mock<IServiceScope> _scopeMock;
    private readonly Mock<IServiceProvider> _scopedServiceProviderMock;
    private readonly Mock<IMediator> _mediatorMock;

    private readonly BackgroundPublisher _publisher;

    public BackgroundPublisherTests()
    {
        _queueMock = new Mock<IBackgroundQueue>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        _scopeFactoryMock = new Mock<IServiceScopeFactory>();
        _scopeMock = new Mock<IServiceScope>();
        _scopedServiceProviderMock = new Mock<IServiceProvider>();
        _mediatorMock = new Mock<IMediator>();

        // Setup the IServiceProvider to return a scope factory
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
            .Returns(_scopeFactoryMock.Object);

        // Setup scope factory to return scope
        _scopeFactoryMock
            .Setup(sf => sf.CreateScope())
            .Returns(_scopeMock.Object);

        // Setup scope to return scoped service provider
        _scopeMock
            .Setup(s => s.ServiceProvider)
            .Returns(_scopedServiceProviderMock.Object);

        // Setup scoped service provider to return mediator
        _scopedServiceProviderMock
            .Setup(sp => sp.GetService(typeof(IMediator)))
            .Returns(_mediatorMock.Object);

        _publisher = new BackgroundPublisher(_queueMock.Object, serviceProviderMock.Object);
    }

    [Fact]
    public async Task Schedule_Should_Call_QueueSchedule_With_CorrectDelay_And_Send_Command()
    {
        // Arrange
        // var testCommand = new TestCommand { Value = 42 };
        // var executeAtUtc = DateTime.UtcNow.AddSeconds(value: 5);
        // TimeSpan capturedDelay = TimeSpan.Zero;
        // Func<CancellationToken, Task>? capturedFunc = null;
        //
        // _queueMock.Setup(expression: q => q.Schedule(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<TimeSpan>()))
        //     .Callback<Func<CancellationToken, Task>, TimeSpan>(action: (func, delay) =>
        //     {
        //         capturedFunc = func;
        //         capturedDelay = delay;
        //     });
        //
        // // Act
        // _publisher.Schedule(command: testCommand, executeAtUtc: executeAtUtc);
        //
        // // Assert delay is approximately correct (allow slight difference for test execution time)
        // var expectedDelay = executeAtUtc - DateTime.UtcNow;
        // if (expectedDelay < TimeSpan.Zero)
        //     expectedDelay = TimeSpan.Zero;
        //
        // Assert.NotNull(@object: capturedFunc);
        // Assert.InRange(actual: capturedDelay.TotalSeconds, low: expectedDelay.TotalSeconds - 1, high: expectedDelay.TotalSeconds + 1);
        //
        // // Now invoke the captured scheduled function and verify mediator.Send is called
        // var cancellationToken = new CancellationToken();
        //
        // // Run the scheduled func
        // var task = capturedFunc!(arg: cancellationToken);
        // task.Wait();
        //
        // _mediatorMock.Verify(expression: m => m.Send(It.Is<TestCommand>(c => c.Value == testCommand.Value), cancellationToken), times: Times.Once);
        
        
        // Arrange
        var testCommand = new TestCommand { Value = 42 };
        var executeAtUtc = DateTime.UtcNow.AddSeconds(5);

        TimeSpan capturedDelay = TimeSpan.Zero;
        Func<CancellationToken, Task>? capturedFunc = null;

        _queueMock
            .Setup(q => q.Schedule(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<TimeSpan>()))
            .Callback<Func<CancellationToken, Task>, TimeSpan>((func, delay) =>
            {
                capturedFunc = func;
                capturedDelay = delay;
            });

        // Act
        _publisher.Schedule(testCommand, executeAtUtc);

        // Assert delay is approximately correct
        var expectedDelay = executeAtUtc - DateTime.UtcNow;
        if (expectedDelay < TimeSpan.Zero)
            expectedDelay = TimeSpan.Zero;

        Assert.NotNull(capturedFunc);
        Assert.InRange(capturedDelay.TotalSeconds, expectedDelay.TotalSeconds - 1, expectedDelay.TotalSeconds + 1);

        // Act: execute the captured function
        var cancellationToken = CancellationToken.None;
        await capturedFunc!(cancellationToken);

        // Assert mediator was called
        _mediatorMock.Verify(
            m => m.Send(It.Is<TestCommand>(c => c.Value == testCommand.Value), cancellationToken),
            Times.Once);
    }
    
    [Fact]
    public void Schedule_Should_Use_ZeroDelay_If_ExecuteAtUtc_Is_Past()
    {
        // Arrange
        var testCommand = new TestCommand { Value = 10 };
        var executeAtUtc = DateTime.UtcNow.AddSeconds(-5); // in the past
        TimeSpan capturedDelay = TimeSpan.MaxValue;

        _queueMock.Setup(q => q.Schedule(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<TimeSpan>()))
            .Callback<Func<CancellationToken, Task>, TimeSpan>((_, delay) =>
            {
                capturedDelay = delay;
            });

        // Act
        _publisher.Schedule(testCommand, executeAtUtc);

        // Assert delay should be zero, never negative
        Assert.Equal(TimeSpan.Zero, capturedDelay);
    }

    // Helper test command class for serialization
    private class TestCommand
    {
        public int Value { get; init; }
    }
}