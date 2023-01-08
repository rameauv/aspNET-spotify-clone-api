using System.Reflection;
using Moq;

namespace Spotify.Shared.tools;

class CustomValueProvider : DefaultValueProvider
{
    protected override object GetDefaultValue(Type type, Mock mock)
    {
        throw new NotImplementedException();
    }

    protected override object GetDefaultParameterValue(ParameterInfo parameter, Mock mock)
    {
        throw new NotImplementedException();
    }

    protected override object GetDefaultReturnValue(MethodInfo method, Mock mock)
    {
        throw new NotImplementedException();
    }
}