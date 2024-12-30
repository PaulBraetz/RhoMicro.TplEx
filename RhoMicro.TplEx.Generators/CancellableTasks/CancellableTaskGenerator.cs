namespace RhoMicro.TplEx.CancellableTasks;

using System.Linq.Expressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.CodeAnalysis.Generated;
using RhoMicro.CodeAnalysis.Library;
using RhoMicro.CodeAnalysis.Library.Text;
using RhoMicro.CodeAnalysis.Library.Models.Collections;
using RhoMicro.TplEx;
using System.Diagnostics.Contracts;
using Microsoft.CodeAnalysis.CSharp;
using RhoMicro.CodeAnalysis.Library.Models;

/// <summary>
/// Generates method overloads for cancellable task producing methods.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class CancellableTaskGenerator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //TODO: refactor to fawmn
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
            static (n, ct) => n is MethodDeclarationSyntax
            {
                ReturnType.RawKind: not (Int32)SyntaxKind.VoidKeyword,
                Parent: ClassDeclarationSyntax
                {
                    Modifiers: [.., { RawKind: (Int32)SyntaxKind.PartialKeyword }],
                    Keyword.RawKind: (Int32)SyntaxKind.ClassKeyword or (Int32)SyntaxKind.StructKeyword or (Int32)SyntaxKind.RecordKeyword
                }
            },
            static (ctx, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                if(ctx.SemanticModel.GetDeclaredSymbol(ctx.Node, ct) is not { } targetSymbol)
                    return null;

                using var modelCtx = ModelCreationContext.CreateDefault(ct);
                var result = CancellableMethodModel.Create(targetSymbol, in modelCtx);

                return result;
            }).Collect()
            .SelectMany(static (m, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                using var modelCtx = ModelCreationContext.CreateDefault(ct);
                var map = new Dictionary<String, EquatableList<CancellableMethodModel>>();
                var result = modelCtx.CollectionFactory.CreateList<EquatableList<CancellableMethodModel>>();

                foreach(var methodModel in m)
                {
                    ct.ThrowIfCancellationRequested();

                    if(methodModel is null)
                        continue;

                    var containingType = methodModel.ContainingType.GetDisplayString(ct);
                    if(!map.TryGetValue(containingType, out var methodsForType))
                    {
                        map[containingType] = methodsForType = modelCtx.CollectionFactory.CreateList<CancellableMethodModel>();
                        result.Add(methodsForType);
                    }

                    methodsForType.Add(methodModel);
                }

                return result;
            })
            .Select(static (m, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                var sourceBuilder = new IndentedStringBuilder(IndentedStringBuilderOptions.GeneratedFile with
                {
                    GeneratorName = typeof(CancellableTaskGenerator).FullName,
                    AmbientCancellationToken = ct
                });

                String? hintName = null;
                String? displayString = null;

                foreach(var method in m)
                {
                    ct.ThrowIfCancellationRequested();

                    if(hintName is null)
                        method.ContainingType.BuildStrings(sourceBuilder, out hintName, out displayString, ct);

                    AppendMethod(sourceBuilder, method, displayString!, ct);
                }

                var source = sourceBuilder.CloseAllBlocks().ToString();

                return (hintName, source);
            }).Where(static t => t.hintName is not null);

        context.RegisterSourceOutput(provider, (ctx, t) => ctx.AddSource(t.hintName!, t.source));
    }

    private static void AppendMethod(IndentedStringBuilder sourceBuilder, CancellableMethodModel model, String _ /*displayString*/, CancellationToken ct)
    {
        //TODO: param type tds to avoind variuous conflicts
        ct.ThrowIfCancellationRequested();

        sourceBuilder
            .Append(model.Accessibility)
            .AppendCore(' ');

        if(model.IsStatic)
            sourceBuilder.AppendCore("static ");

        sourceBuilder.Append(model.CancellableReturnType)
            .Append(' ')
            .Append(model.MethodName)
            .AppendCore('(');

        for(var i = 0; i < model.Parameters.Count; i++)
        {
            ct.ThrowIfCancellationRequested();

            if(i != 0)
                sourceBuilder.AppendCore(", ");

            sourceBuilder.AppendCore(model.Parameters[i].Syntax);
        }

        sourceBuilder
            .Append(')')
            .OpenBracesBlock()
            .AppendLine("var __cts = new global::System.Threading.CancellationTokenSource();")
            .AppendLine("var __ct = __cts.Token;")
            .Append("var __task = ").Append(model.MethodName).AppendCore('(');

        for(var i = 0; i < model.Parameters.Count; i++)
        {
            ct.ThrowIfCancellationRequested();

            var parameter = model.Parameters[i];
            sourceBuilder
                .Append(parameter.Name).Append(": ").Append(parameter.Name) //TODO: ref modifiers
                .AppendCore(", ");
        }

        sourceBuilder
            .Append(model.CtParameter.Name).AppendLine(": __ct);") //TODO: ref modifiers
            .Append("var __result = new ").Append(model.CancellableReturnType).AppendLine("(__task, __cts);")
            .Append("return __result;")
            .CloseBlockCore();
    }
}
