using CodeNet.Core;
using CodeNet.MongoDB;
using CodeNet.MongoDB.Repositories;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.MongoDB.Models;
using CodeNet.Parameters.Settings;
using CodeNet.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodeNet.Parameters.MongoDB.Manager;

internal sealed class ParameterMongoDBManager(MongoDBContext dbContext, ICodeNetContext codeNetContext, IOptions<ParameterSettings> options, IServiceProvider serviceProvider) : IParameterManager
{
    private readonly BaseMongoRepository<ParameterDto> _repository = new(dbContext);
    private readonly ParameterSettings? _parameterSettings = options?.Value;
    private readonly IDistributedCache<ParameterGroupWithParamsResult>? _distributedCache = serviceProvider.GetService<IDistributedCache<ParameterGroupWithParamsResult>>();

    #region Parameter Group CRUD
    public async Task<ParameterGroupWithParamsResult> AddParameterAsync(ParameterGroupWithParamsModel model, CancellationToken cancellationToken = default)
    {
        await _repository.CreateAsync(new ParameterDto
        {
            Code = model.Code,
            ApprovalRequired = model.ApprovalRequired,
            CreatedDate = DateTime.Now,
            CreatedUser = codeNetContext.UserName,
            Description = model.Description,
            Id = model.Id,
            IsActive = true,
            IsDeleted = false,
            Parameters = model.Parameters.Select(c => new ParameterModel
            {
                Code = c.Code,
                Value = c.Value,
                Id = c.Id,
                IsDefault = c.IsDefault,
                Order = c.Order
            })
        }, cancellationToken);

        var result = ModelToResult(model);
        if (_distributedCache is not null)
            await SetCacheAsync(result, cancellationToken);

        return result;
    }

    public async Task<ParameterGroupWithParamsResult?> UpdateParameterAsync(ParameterGroupWithParamsModel model, CancellationToken cancellationToken = default)
    {
        await _repository.UpdateAsync(c => c.Code == model.Code, ModelToDto(model), cancellationToken);
        var dto = await _repository.GetByIdAsync(c => c.Code == model.Code, cancellationToken);
        var result = DtoToResult(dto);

        if (_distributedCache is not null)
        {
            await RemoveCacheAsync(model.Code, cancellationToken);
            await SetCacheAsync(result, cancellationToken);
        }

        return result;
    }

    public async Task DeleteParameterAsync(string groupCode, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(c => c.Code == groupCode, cancellationToken);

        if (_distributedCache is not null)
            await RemoveCacheAsync(groupCode, cancellationToken);
    }

    public async Task<ParameterGroupWithParamsResult?> GetParameterAsync(string groupCode, CancellationToken cancellationToken = default)
    {
        var result = DtoToResult(await _repository.GetByIdAsync(c => c.Code == groupCode, cancellationToken));

        if (_distributedCache is not null && result is not null)
            await SetCacheAsync(result, cancellationToken);

        return result;
    }

    public async Task<List<ParameterGroupResult>> GetParameterGroupListAsync(int page, int count, CancellationToken cancellationToken = default)
    {
        var dtoList = await _repository.GetPagingListAsync(c => c.IsActive, page, count, cancellationToken);
        return dtoList.Select(c => new ParameterGroupResult
        {
            Code = c.Code,
            ApprovalRequired = c.ApprovalRequired,
            Description = c.Description,
            Id = c.Id
        }).ToList();
    }

    private static ParameterGroupWithParamsResult ModelToResult(ParameterGroupWithParamsModel model) => new()
    {
        Code = model.Code,
        ApprovalRequired = model.ApprovalRequired,
        Description = model.Description,
        Id = model.Id,
        Parameters = model.Parameters.Select(c => new ParameterResult
        {
            Code = c.Code,
            Value = c.Value,
            IsDefault = c.IsDefault,
            Order = c.Order,
            Id = c.Id
        })
    };

    private static ParameterGroupWithParamsResult DtoToResult(ParameterDto dto) => new()
    {
        Code = dto.Code,
        ApprovalRequired = dto.ApprovalRequired,
        Description = dto.Description,
        Id = dto.Id,
        Parameters = dto.Parameters.Select(c => new ParameterResult
        {
            Code = c.Code,
            Value = c.Value,
            IsDefault = c.IsDefault,
            Order = c.Order,
            Id = c.Id
        })
    };

    private static ParameterDto ModelToDto(ParameterGroupWithParamsModel model) => new()
    {
        Code = model.Code,
        ApprovalRequired = model.ApprovalRequired,
        Description = model.Description,
        Id = model.Id,
        Parameters = model.Parameters.Select(c => new ParameterModel
        {
            Code = c.Code,
            Value = c.Value,
            IsDefault = c.IsDefault,
            Order = c.Order,
            Id = c.Id
        })
    };
    #endregion

    private Task RemoveCacheAsync(string groupCode, CancellationToken cancellationToken)
    {
        return _distributedCache?.RemoveAsync($"{_parameterSettings!.RedisPrefix}_Code:{groupCode}", cancellationToken) ?? Task.CompletedTask;
    }

    private Task SetCacheAsync(ParameterGroupWithParamsResult result, CancellationToken cancellationToken)
    {
        return _distributedCache?.SetValueAsync(result, $"{_parameterSettings!.RedisPrefix}_Code:{result.Code}", _parameterSettings!.Time, cancellationToken) ?? Task.CompletedTask;
    }
}
