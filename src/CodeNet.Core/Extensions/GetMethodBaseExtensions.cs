using System.Reflection;
using System.Runtime.CompilerServices;

namespace CodeNet.Core.Extensions;

public static class GetMethodBaseExtensions
{
    public static MethodBase GetMethodBase(this MethodBase methodBase)
    {
        if (methodBase!.DeclaringType!.GetInterfaces().Any(i => i.Equals(typeof(IAsyncStateMachine))) == true)
        {
            var generatedType = methodBase.DeclaringType;
            var originalType = generatedType.DeclaringType;
            var foundMethod = originalType!.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType.Equals(generatedType) is true);
            return foundMethod;
        }

        return methodBase;
    }
}
