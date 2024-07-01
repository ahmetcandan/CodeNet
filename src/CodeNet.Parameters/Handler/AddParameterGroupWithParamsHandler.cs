using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;

namespace CodeNet.Parameters.Handler;

internal class AddParameterGroupWithParamsHandler(ParametersDbContext dbContext, ICodeNetHttpContext identityContext) : IRequestHandler<AddParameterGroupWithParamsRequest, ResponseBase<ParameterGroupWithParamsResult>>
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);
    private readonly ParameterRepositoryResolver _parameterRepositoryResolver = new(dbContext, identityContext);

    public async Task<ResponseBase<ParameterGroupWithParamsResult>> Handle(AddParameterGroupWithParamsRequest request, CancellationToken cancellationToken)
    {
        var parameterRepository = _parameterRepositoryResolver.GetParameterRepository(request.ApprovalRequired);
        var parameterGroup = new ParameterGroup
        {
            Code = request.Code,
            ApprovalRequired = request.ApprovalRequired,
            Description = request.Description
        };
        var addGroupResponse = await _parameterGroupRepository.AddAsync(parameterGroup, cancellationToken);
        var parameterResultList = new List<ParameterResult>();
        foreach (var item in request.AddParameters)
            parameterResultList.Add((await parameterRepository.AddAsync(new Parameter
            {
                Code = item.Code,
                Value = item.Value,
                GroupId = addGroupResponse.Id,
                IsDefault = item.IsDefault,
                Order = item.Order
            }, cancellationToken)).ToParameterResult());

        await _parameterGroupRepository.SaveChangesAsync(cancellationToken);
        return new ResponseBase<ParameterGroupWithParamsResult>(new ParameterGroupWithParamsResult 
        {
            Code = addGroupResponse.Code,
            ApprovalRequired = addGroupResponse.ApprovalRequired,
            Description = addGroupResponse.Description,
            Id = addGroupResponse.Id,
            Parameters = parameterResultList
        });
    }
}
