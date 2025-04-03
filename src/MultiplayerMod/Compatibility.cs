namespace System.Runtime.CompilerServices;

// ReSharper disable once UnusedType.Global
internal static class IsExternalInit { }

// Caller argument expression support for .NET < 5
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute
{
    public string ParameterName { get; } = parameterName;

}
