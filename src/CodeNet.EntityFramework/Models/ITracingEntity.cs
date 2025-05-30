﻿namespace CodeNet.EntityFramework.Models;

public interface ITracingEntity : IBaseEntity
{
    string CreatedUser { get; set; }
    DateTime CreatedDate { get; set; }
    string? ModifiedUser { get; set; }
    DateTime? ModifiedDate { get; set; }
}
