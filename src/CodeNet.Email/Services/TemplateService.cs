using CodeNet.Email.Builder;
using CodeNet.Email.Models;
using CodeNet.Email.Repositories;

namespace CodeNet.Email.Services;

internal class TemplateService(MailTemplateRepositories templateRepositories) : ITemplateService
{
    public async Task AddTemplate(TemplateCreateModel model)
    {
        var builder = TemplateBuilder.Compile(model.Content);
        await templateRepositories.CreateAsync(new MailTemplate
        {
            Bcc = model.Bcc,
            Builder = builder,
            Cc = model.Cc,
            Code = model.Code,
            From = model.From,
            IsBodyHtml = model.IsBodyHtml,
            To = model.To
        });
    }

    public async Task UpdateTemplate(TemplateUpdateModel model)
    {
        var builder = TemplateBuilder.Compile(model.Content);
        await templateRepositories.UpdateAsync(c => c.Code == model.Code ,new MailTemplate
        {
            Bcc = model.Bcc,
            Builder = builder,
            Cc = model.Cc,
            Code = model.Code,
            From = model.From,
            IsBodyHtml = model.IsBodyHtml,
            To = model.To
        });
    }

    public Task RemoveTemplate(string code)
    {
        return templateRepositories.DeleteAsync(c => c.Code == code);
    }
}
