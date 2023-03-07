using Eqn.Core.DependencyInjection;
using Whisper.Application.Abstract;
using Whisper.Domain.Stats;

namespace Whisper.Application.Services;

public class StatsService : IStatsService, ITransientDependency
{
    public async Task<bool> SaveStatsAsync(RecognizeStats stats)
    {
        throw new NotImplementedException();
    }
}