using CodeNet.Email.Models;

namespace CodeNet.Email.Services;

public interface ITemplateService
{
    Task AddTemplate(TemplateCreateModel model);
    Task UpdateTemplate(TemplateUpdateModel model);
    Task RemoveTemplate(string code);
}