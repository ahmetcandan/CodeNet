using NetCore.Abstraction;
using NetCore.Abstraction.Enum;
using System;
using System.Linq;
using System.Reflection;

namespace NetCore.Logging;

public class LogProxy<TDecorated> : DispatchProxy
{
    private TDecorated _decorated;
    private ILogRepository _logRepository;
    private IIdentityContext _identityContext;

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        var aspect = (ILogAttribute)targetMethod.GetCustomAttributes(typeof(ILogAttribute), true).FirstOrDefault();

        if (aspect == null)
            return targetMethod.Invoke(_decorated, args);


        string username = _identityContext.GetUserName();

        if (aspect.GetLogTime() == LogTime.Before || aspect.GetLogTime() == LogTime.BeforeAndAfter)
            aspect?.OnBefore(targetMethod, args, _logRepository, username);

        object result;
        try
        {
            result = targetMethod.Invoke(_decorated, args);
        }
        catch (Exception ex)
        {
            if (aspect.GetLogTime() == LogTime.Exception)
                aspect?.OnException(targetMethod, args, _logRepository, username, ex);
            throw;
        }

        if (aspect.GetLogTime() == LogTime.After || aspect.GetLogTime() == LogTime.BeforeAndAfter)
            aspect?.OnAfter(targetMethod, args, result, _logRepository, username);

        return result;
    }

    public static TDecorated Create(TDecorated decorated, ILogRepository logRepository, IIdentityContext identityContext)
    {
        object proxy = Create<TDecorated, LogProxy<TDecorated>>();
        ((LogProxy<TDecorated>)proxy).SetParameters(decorated, logRepository, identityContext);
        return (TDecorated)proxy;
    }

    private void SetParameters(TDecorated decorated, ILogRepository logRepository, IIdentityContext identityContext)
    {
        _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        _logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
        _identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
    }
}
