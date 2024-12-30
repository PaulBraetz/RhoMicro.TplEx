namespace RhoMicro.TplEx.FileInclusion;

using Microsoft.CodeAnalysis;

using RhoMicro.CodeAnalysis.Generated;

/// <summary>
/// Generates the files included by generators in this project.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class FileInclusionGenerator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context) => IncludedFileSources.RegisterToContext(context);
}
