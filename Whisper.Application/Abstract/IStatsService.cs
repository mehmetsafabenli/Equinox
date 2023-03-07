using Whisper.Domain.Stats;

namespace Whisper.Application.Abstract;

public interface IStatsService
{
    Task<bool> SaveStatsAsync(RecognizeStats stats);
}