using CodeNet.Email.Models;
using CodeNet.Email.Repositories;
using CodeNet.Messaging.Builder;

namespace CodeNet.Email.Services;

internal class TemplateService(MailTemplateRepositories templateRepositories) : ITemplateService
{
    public async Task AddTemplate(TemplateCreateModel model) => await templateRepositories.CreateAsync(new MailTemplate
    {
        Bcc = model.Bcc,
        Builder = MessageBuilder.Compile(model.Content),
        Content = model.Content,
        Cc = model.Cc,
        Code = model.Code,
        From = model.From,
        IsBodyHtml = model.IsBodyHtml,
        To = model.To
    });

    public async Task UpdateTemplate(TemplateUpdateModel model) => await templateRepositories.UpdateAsync(c => c.Code == model.Code, new MailTemplate
    {
        Bcc = model.Bcc,
        Builder = MessageBuilder.Compile(model.Content),
        Content = model.Content,
        Cc = model.Cc,
        Code = model.Code,
        From = model.From,
        IsBodyHtml = model.IsBodyHtml,
        To = model.To
    });

    public Task RemoveTemplate(string code) => templateRepositories.DeleteAsync(c => c.Code == code);
}
