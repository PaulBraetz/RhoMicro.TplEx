namespace RhoMicro.TplEx.PostfixAwait;
using System;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

using RhoMicro.CodeAnalysis.Library.Models;
using RhoMicro.CodeAnalysis.Library.Models.Collections;

internal sealed record AwaitInvocationModel(String TaskTypeDisplayName, TaskKind TaskKind, WrappedTypeModel TaskTypeArgument)
{
    public static AwaitInvocationModel Create(INamedTypeSymbol wrappedType, in ModelCreationContext ctx)
    {
        ctx.ThrowIfCancellationRequested();

        var result = CreateCore(
            wrappedType,
            $"global::System.Threading.Tasks.ValueTask<{wrappedType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>",
            TaskKind.ValueTask,
            in ctx);

        return result;
    }
    public static AwaitInvocationModel Create(INamedTypeSymbol taskType, INamedTypeSymbol wrappedType, in ModelCreationContext ctx)
    {
        ctx.ThrowIfCancellationRequested();

        var taskTypeDisplayName = taskType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var taskKind = taskType.Name == "Task"
            ? TaskKind.Task
            : TaskKind.ValueTask;

        var result = CreateCore(wrappedType, taskTypeDisplayName, taskKind, in ctx);

        return result;
    }

    private static AwaitInvocationModel CreateCore(INamedTypeSymbol wrappedType, String taskTypeDisplayName, TaskKind taskKind, in ModelCreationContext ctx)
    {
        ctx.ThrowIfCancellationRequested();

        var taskTypeArgument = WrappedTypeModel.Create(wrappedType, in ctx);

        var result = new AwaitInvocationModel(
            taskTypeDisplayName,
            taskKind,
            taskTypeArgument);

        return result;
    }
}
