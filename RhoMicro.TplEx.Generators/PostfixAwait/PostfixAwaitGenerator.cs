namespace RhoMicro.TplEx.PostfixAwait;

using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using RhoMicro.CodeAnalysis.Library.Models;
using RhoMicro.CodeAnalysis.Library.Models.Collections;
using RhoMicro.CodeAnalysis.Library.Text;

/// <summary>
/// Generates types and extensions for chaining <see langword="await"/> calls.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class PostfixAwaitGenerator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.ForAttributeWithMetadataName(
            ExtendTplAttributeMetadataName,
            static (_, _) => true,
            static (ctx, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                using var modelCtx = ModelCreationContext.CreateDefault(ct);
                var result = AwaitInvocationsModel.Create(ctx.TargetNode, ctx.SemanticModel, in modelCtx);

                return result;
            })
            .Collect()
            .SelectMany((a, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                using var ctx = ModelCreationContext.CreateDefault(ct);
                var result = AwaitExtensionModel.Create(a, in ctx);

                return result;
            })
            .Select(static (m, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                var sourceBuilder = new IndentedStringBuilder(IndentedStringBuilderOptions.GeneratedFile with
                {
                    AmbientCancellationToken = ct,
                    GeneratorName = typeof(PostfixAwaitGenerator).FullName
                });

                var taskKind = m.TaskKind.ToStringFast();
                var accessibility = SyntaxFacts.GetText(m.WrappedType.Accessibility);

                if(m.WrappedType.Namespace is not null)
                    sourceBuilder.Append("namespace ").Append(m.WrappedType.Namespace).Append(';').AppendLineCore();

                sourceBuilder
                    .Append(accessibility).Append(" readonly struct ").Append(m.WrapperTypeName)
                    .OpenBracesBlock()
                    .Append("public ").Append(m.WrapperTypeName).Append('(').Append(m.TaskTypeDisplayName).AppendLine(" task) => _task = task;")
                    .Append("private readonly ").Append(m.TaskTypeDisplayName).Append(" _task;").AppendLineCore();

                foreach(var property in m.WrappedType.Properties)
                {
                    ct.ThrowIfCancellationRequested();

                    sourceBuilder
                        .Append("public global::System.Threading.Tasks.ValueTask<").Append(property.Type).Append("> ").Append(property.Name).Append(" => __Get").Append(property.Name).AppendLine("();")
                        .AppendLine("[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]")
                        .Append("private async global::System.Threading.Tasks.ValueTask<").Append(property.Type).Append("> __Get").Append(property.Name).Append("() => (await _task).").Append(property.Name).Append(';')
                        .AppendLineCore();
                }

                var source = sourceBuilder
                    .CloseBlock()
                    .Append("internal static partial class TaskExtensions")
                    .OpenBracesBlock()
                    .AppendLine("[global::System.Runtime.CompilerServices.OverloadResolutionPriority(1)]")
                    .Append("public static ").Append(m.WrapperTypeName).Append(" Await(this ").Append(m.TaskTypeDisplayName).Append(" task) => new(task);")
                    .CloseBlock()
                    .ToStringAndClear();

                var hintName = $"{m.WrappedType.Namespace?.Replace('.', '_')}{( m.WrappedType.Namespace is { Length: > 0 } ? "_" : String.Empty )}{m.WrapperTypeName}.g.cs";

                return (hintName, source);
            });

        context.RegisterSourceOutput(provider, (ctx, t) => ctx.AddSource(t.hintName, t.source));
    }
}
