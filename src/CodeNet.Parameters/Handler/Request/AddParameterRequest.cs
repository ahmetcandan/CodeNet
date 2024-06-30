using CodeNet.Core.Models;
using CodeNet.Parameters.Models;
using MediatR;

namespace CodeNet.Parameters.Handler.Request;

internal class AddParameterRequest : AddParameterModel, IRequest<ResponseBase<ParameterResult>>
{
}
