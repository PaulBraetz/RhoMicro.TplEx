namespace RhoMicro.CancellableTasks;

using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.CodeAnalysis.Library.Models;

internal readonly record struct ParameterModel(String Name, String Syntax)
{
    public static ParameterModel Create(IParameterSymbol parameter, in ModelCreationContext ctx)
    {
        ctx.ThrowIfCancellationRequested();

        var name = parameter.Name;
        var syntax = parameter.DeclaringSyntaxReferences[0].GetSyntax(ctx.CancellationToken).ToFullString() ;

        var result = new ParameterModel(
            Name: name,
            Syntax:syntax);

        return result;
    }
}
