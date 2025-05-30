﻿using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Parameters.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Parameters.Repositories;

internal class ParameterTracingRepository(DbContext dbContext, ICodeNetContext identityContext) : TracingRepository<Parameter>(dbContext, identityContext), IParameterRepository
{
}
