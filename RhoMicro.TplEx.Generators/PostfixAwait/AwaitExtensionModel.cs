namespace RhoMicro.TplEx.PostfixAwait;
using System;
using System.Collections.Immutable;

using RhoMicro.CodeAnalysis.Library.Models.Collections;
using RhoMicro.CodeAnalysis.Library.Models;

internal sealed record AwaitExtensionModel(
    String TaskTypeDisplayName,
    TaskKind TaskKind,
    WrappedTypeModel WrappedType,
    String WrapperTypeName)
{

    public static EquatableList<AwaitExtensionModel> Create(ImmutableArray<AwaitInvocationsModel> models, in ModelCreationContext ctx)
    {
        ctx.ThrowIfCancellationRequested();

        var result = ctx.CollectionFactory.CreateList<AwaitExtensionModel>();
        var added = new HashSet<String>();

        foreach(var invocationsModels in models)
        {
            ctx.ThrowIfCancellationRequested();

            foreach(var invocationModel in invocationsModels.Invocations)
            {
                ctx.ThrowIfCancellationRequested();

                if(added.Add(invocationModel.TaskTypeDisplayName))
                {
                    var wrapperTypeName = $"{invocationModel.TaskTypeArgument.Name}{invocationModel.TaskKind.ToStringFast()}";

                    var extensionModel = new AwaitExtensionModel(
                        invocationModel.TaskTypeDisplayName,
                        invocationModel.TaskKind,
                        invocationModel.TaskTypeArgument,
                        wrapperTypeName);

                    result.Add(extensionModel);
                }
            }
        }

        return result;
    }
}
