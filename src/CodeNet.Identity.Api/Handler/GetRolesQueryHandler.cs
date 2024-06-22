﻿using MediatR;
using CodeNet.Core.Models;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Api.Handler
{
    public class GetRolesQueryHandler(IIdentityRoleManager IdentityRoleManager) : IRequestHandler<GetRoleQuery, ResponseBase<IEnumerable<RoleViewModel>>>
    {
        public async Task<ResponseBase<IEnumerable<RoleViewModel>>> Handle(GetRoleQuery query, CancellationToken cancellationToken)
        {
            return await IdentityRoleManager.GetRoles(cancellationToken);
        }
    }
}
