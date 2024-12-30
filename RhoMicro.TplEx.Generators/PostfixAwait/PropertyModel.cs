namespace RhoMicro.TplEx.PostfixAwait;
using System;

using Microsoft.CodeAnalysis;

using RhoMicro.CodeAnalysis.Library.Models;

internal readonly record struct PropertyModel(String Type, String Name)
{
    public static PropertyModel Create(IPropertySymbol property, in ModelCreationContext ctx)
    {
        ctx.ThrowIfCancellationRequested();

        var type = property.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var name = property.Name;

        var result = new PropertyModel(type, name);

        return result;
    }
}
