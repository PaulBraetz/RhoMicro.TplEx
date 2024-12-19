namespace RhoMicro.CancellableTasks;

using System.Linq.Expressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.CodeAnalysis.Generated;
using RhoMicro.CodeAnalysis.Library;
using RhoMicro.CodeAnalysis.Library.Text;
using RhoMicro.CodeAnalysis.Library.Models.Collections;
using RhoMicro.CancellableTasks;
using System.Diagnostics.Contracts;
using Microsoft.CodeAnalysis.CSharp;

/// <summary>
/// Generates method overloads for cancellable task producing methods.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class CancellableTasksGenerator : IIncrementalGenerator
{
    private static readonly EquatableCollectionFactory _collectionFactory = EquatableCollectionFactory.Default;

    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
            static (n, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                var result = n is MethodDeclarationSyntax
                {
                    ReturnType.RawKind: not (Int32)SyntaxKind.VoidKeyword,
                    Parent: ClassDeclarationSyntax
                    {
                        Modifiers: [.., { RawKind: (Int32)SyntaxKind.PartialKeyword }],
                        Keyword.RawKind: (Int32)SyntaxKind.ClassKeyword or (Int32)SyntaxKind.StructKeyword or (Int32)SyntaxKind.RecordKeyword
                    }
                };

                return result;
            },
            static (ctx, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                if(ctx.SemanticModel.GetDeclaredSymbol(ctx.Node, ct) is not { } targetSymbol)
                    return null;

                var result = CancellableMethodModel.Create(targetSymbol, new(_collectionFactory, ct));

                return result;
            }).Collect()
            .SelectMany(static (m, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                var mutabilityContext = new MutabilityContext();

                var map = new Dictionary<String, EquatableList<CancellableMethodModel>>();
                var result = _collectionFactory.CreateList<EquatableList<CancellableMethodModel>>(mutabilityContext);

                foreach(var methodModel in m)
                {
                    ct.ThrowIfCancellationRequested();

                    if(methodModel is null)
                        continue;

                    var containingType = methodModel.ContainingType.GetDisplayString(ct);
                    if(!map.TryGetValue(containingType, out var methodsForType))
                    {
                        map[containingType] = methodsForType = _collectionFactory.CreateList<CancellableMethodModel>(mutabilityContext);
                        result.Add(methodsForType);
                    }

                    methodsForType.Add(methodModel);
                }

                mutabilityContext.SetImmutable();

                return result;
            })
            .Select(static (m, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                var sourceBuilder = new IndentedStringBuilder(IndentedStringBuilderOptions.GeneratedFile with
                {
                    GeneratorName = typeof(CancellableTasksGenerator).FullName,
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
