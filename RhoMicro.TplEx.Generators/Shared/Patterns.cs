global using static RhoMicro.TplEx.Shared.Patterns;

namespace RhoMicro.TplEx.Shared;

using System;

using RhoMicro.CodeAnalysis;

using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

internal static partial class Patterns
{
    [TypeSymbolPattern(typeof(ValueTask))]
    public static partial Boolean IsValueTask(ITypeSymbol? type);
    
    [TypeSymbolPattern(typeof(ValueTask), CheckTypeArguments = false)]
    private static partial Boolean IsGenericValueTaskCore(ITypeSymbol? type);
    private static Boolean IsGenericValueTask(ITypeSymbol? type, [NotNullWhen(true)] out INamedTypeSymbol? outType) => IsGenericValueTaskCore(type, out outType) && ( (INamedTypeSymbol)type! ).TypeArguments.Length == 1;
    public static Boolean IsGenericValueTask(ITypeSymbol? type, [NotNullWhen(true)] out ITypeSymbol? parameter, [NotNullWhen(true)] out INamedTypeSymbol? outType)
    {
        var result = IsGenericValueTask(type, out outType);
        parameter = result ? ( (INamedTypeSymbol)type! ).TypeArguments[0] : null;
        return result;
    }

    [TypeSymbolPattern(typeof(Task))]
    public static partial Boolean IsTask(ITypeSymbol? type);

    [TypeSymbolPattern(typeof(Task), CheckTypeArguments = false)]
    private static partial Boolean IsGenericTaskCore(ITypeSymbol? type);
    private static Boolean IsGenericTask(ITypeSymbol? type, [NotNullWhen(true)] out INamedTypeSymbol? outType) => IsGenericTaskCore(type, out outType) && ( (INamedTypeSymbol)type! ).TypeArguments.Length == 1;
    public static Boolean IsGenericTask(ITypeSymbol? type, [NotNullWhen(true)] out ITypeSymbol? parameter, [NotNullWhen(true)] out INamedTypeSymbol? outType)
    {
        var result = IsGenericTask(type, out outType);
        parameter = result ? ( (INamedTypeSymbol)type! ).TypeArguments[0] : null;
        return result;
    }

    [TypeSymbolPattern(typeof(CancellationToken))]
    public static partial Boolean IsCancellationToken(ITypeSymbol? type);
}