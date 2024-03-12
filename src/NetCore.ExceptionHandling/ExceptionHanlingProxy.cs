using NetCore.Abstraction;
using System;
using System.Linq;
using System.Reflection;

namespace NetCore.ExceptionHandling;

public class ExceptionHanlingProxy<TDecorated>(IIdentityContext identityContext) : DispatchProxy
{
    private TDecorated _decorated;
    private ILogRepository _logRepository;
    private IIdentityContext _identityContext = identityContext;

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        var aspect = (IExceptionAttribute)targetMethod.GetCustomAttributes(typeof(IExceptionAttribute), true).FirstOrDefault();

        if (aspect == null)
            return targetMethod.Invoke(_decorated, args);


        string username = _identityContext.GetUserName();

        object result = null;

        try
        {
            result = targetMethod.Invoke(_decorated, args);
        }
        catch (UserLevelException ex)
        {
            aspect?.OnException(targetMethod, args, _logRepository, ex, username);
            if (aspect.GetThrowException())
                throw;
        }
        catch (Exception ex)
        {
            aspect?.OnException(targetMethod, args, _logRepository, ex, username);
            if (aspect.GetThrowException())
                throw new Exception(ex.Message);
        }

        return result;
    }

    public static TDecorated Create(TDecorated decorated, ILogRepository logRepository, IIdentityContext identityContext)
    {
        object proxy = Create<TDecorated, ExceptionHanlingProxy<TDecorated>>();
        ((ExceptionHanlingProxy<TDecorated>)proxy).SetParameters(decorated, logRepository, identityContext);
        return (TDecorated)proxy;
    }

    private void SetParameters(TDecorated decorated, ILogRepository logRepository, IIdentityContext identityContext)
    {
        _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        _logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
        _identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
    }
}
