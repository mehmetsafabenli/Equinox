using Eqn.BackGroundWorkers.BackgroundWorkers;
using Eqn.Threading.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Whisper.Background;

public class TestBackGround : AsyncPeriodicBackgroundWorkerBase
{
    public TestBackGround(EqnAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer,
        serviceScopeFactory)
    {
        Timer.Period = 10_000;
    }

    protected override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        Console.WriteLine("TestBackGround" + DateTime.Now);
        return Task.CompletedTask;
    }
}