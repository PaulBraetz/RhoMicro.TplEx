namespace RhoMicro.TplEx.PostfixAwait;

using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

using RhoMicro.CodeAnalysis.Library.Models;
using RhoMicro.CodeAnalysis.Library.Models.Collections;

internal sealed record AwaitInvocationsModel(EquatableList<AwaitInvocationModel> Invocations)
{
    public static AwaitInvocationsModel Create(SyntaxNode target, SemanticModel semanticModel, in ModelCreationContext ctx)
    {
        ctx.ThrowIfCancellationRequested();

        var invocations = ctx.CollectionFactory.CreateList<AwaitInvocationModel>();
        var result = new AwaitInvocationsModel(invocations);

        if(target is not TypeDeclarationSyntax targetDecl)
            return result;

        foreach(var member in targetDecl.Members)
        {
            ctx.ThrowIfCancellationRequested();

            var memberOp = semanticModel.GetOperation(member, ctx.CancellationToken);

            if(memberOp is not IMethodBodyBaseOperation op)
                continue;

            foreach(var childOp in op.Descendants())
            {
                ctx.ThrowIfCancellationRequested();

                // check if marker stub is being invoked
                if(childOp.Syntax is not InvocationExpressionSyntax
                    {
                        // stub does not have parameters
                        ArgumentList.Arguments: [],
                        // extension method call is parsed as simple member access expr
                        Expression: MemberAccessExpressionSyntax
                        {
                            RawKind: (Int32)SyntaxKind.SimpleMemberAccessExpression,
                            // accessed member name
                            Name.Identifier.Text: "Await",
                            Expression: var awaitedInstanceExpr
                        }
                    })
                {
                    continue;
                }

                // we may recognize the following types on expressions that Await was called on:
                // stub:            stub:                 generated/unknown:
                // Task<TWrapped> | ValueTask<TWrapped> | TWrapped (ValueTask<TWrapped>) from property of wrapper task object not yet visible to us
                
                if(semanticModel.GetTypeInfo(awaitedInstanceExpr, ctx.CancellationToken).Type is not INamedTypeSymbol awaitedInstanceType)
                    continue; // unable to determine type
                
                var model = IsStubMethodInvocation(awaitedInstanceType, out var taskType, out var namedWrappedType)
                    ? AwaitInvocationModel.Create(taskType, namedWrappedType, in ctx) // stub method recognized
                    : AwaitInvocationModel.Create(awaitedInstanceType, in ctx); // non-stub method usage, we assume chained call onto ValueTask property of generated type

                invocations.Add(model);
            }
        }

        return result;
    }

    private static Boolean IsStubMethodInvocation(
        INamedTypeSymbol awaitedInstanceType,
        [NotNullWhen(true)] out INamedTypeSymbol? taskType,
        [NotNullWhen(true)] out INamedTypeSymbol? namedWrappedType)
    {
        if(IsGenericTask(awaitedInstanceType, out var wrappedType, out taskType) || IsGenericValueTask(awaitedInstanceType, out wrappedType, out taskType))
        {
            if(wrappedType is INamedTypeSymbol s)
            {
                namedWrappedType = s;

                return true;
            }
        }

        namedWrappedType = null;
        return false;
    }
}
