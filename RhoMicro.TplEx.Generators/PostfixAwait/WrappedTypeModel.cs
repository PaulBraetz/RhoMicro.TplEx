namespace RhoMicro.TplEx.PostfixAwait;
using System;

using Microsoft.CodeAnalysis;

using RhoMicro.CodeAnalysis.Library.Models.Collections;
using RhoMicro.CodeAnalysis.Library.Models;

internal sealed record WrappedTypeModel(
    String Name,
    String? Namespace,
    String DisplayString,
    EquatableList<PropertyModel> Properties,
    Accessibility Accessibility)
{
    public static WrappedTypeModel Create(INamedTypeSymbol type, in ModelCreationContext ctx)
    {
        ctx.ThrowIfCancellationRequested();

        var name = type.Name;
        var @namespace = type.ContainingNamespace is { IsGlobalNamespace: false } ns
            ? ns.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
            : null;
        var displayString = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var properties = ctx.CollectionFactory.CreateList<PropertyModel>();
        var accessibility = type.DeclaredAccessibility;

        foreach(var member in type.GetMembers())
        {
            ctx.ThrowIfCancellationRequested();

            if(member is not IPropertySymbol { ExplicitInterfaceImplementations: [], IsIndexer: false } property)
                continue;

            var model = PropertyModel.Create(property, in ctx);
            properties.Add(model);
        }

        var taskTypeArgument = new WrappedTypeModel(
            Name: name,
            Namespace: @namespace,
            DisplayString: displayString,
            properties,
            accessibility);

        return taskTypeArgument;
    }
}