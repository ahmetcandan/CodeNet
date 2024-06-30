using CodeNet.Core.Models;
using CodeNet.Parameters.Models;
using MediatR;

namespace CodeNet.Parameters.Handler.Request;

internal class UpdateParameterRequest : UpdateParameterModel, IRequest<ResponseBase<ParameterResult>>
{
}
