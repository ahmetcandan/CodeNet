using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

public interface IMakerCheckerHistoryRepository
{
    MakerCheckerHistory Add(MakerCheckerHistory makerCheckerHistory);
    Task<MakerCheckerHistory> AddAsync(MakerCheckerHistory makerCheckerHistory);
    Task<MakerCheckerHistory> AddAsync(MakerCheckerHistory makerCheckerHistory, CancellationToken cancellationToken);
}