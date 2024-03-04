using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NetCore.Abstraction.Extension
{
    public static class GetMethodBaseExtension
    {
        public static MethodBase GetMethodBase(this MethodInfo methodBase) 
        {
            if (methodBase.DeclaringType.GetInterfaces().Any(i=> i.Equals(typeof(IAsyncStateMachine))))
            {
                var generatedType = methodBase.DeclaringType;
                var originalType = generatedType.DeclaringType;
                var foundMethod = originalType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                    .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType.Equals(generatedType) is true);
                return foundMethod;
            }

            return methodBase;
        }
    }
}
