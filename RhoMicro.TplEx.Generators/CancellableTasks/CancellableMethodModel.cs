namespace RhoMicro.TplEx.CancellableTasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using RhoMicro.CodeAnalysis;
using RhoMicro.CodeAnalysis.Library.Models;
using RhoMicro.CodeAnalysis.Library.Models.Collections;

//TODO: copy type param constraints
internal sealed partial record CancellableMethodModel(
    NamedTypeModel ContainingType,
    String Accessibility,
    Boolean IsStatic,
    String MethodName,
    EquatableList<String> MethodTypeParameters,
    String CancellableReturnType,
    String? TypeArgumentName,
    EquatableList<ParameterModel> Parameters,
    ParameterModel CtParameter)
{
    public static CancellableMethodModel? Create(ISymbol targetSymbol, in ModelCreationContext ctx)
    {
        ctx.ThrowIfCancellationRequested();

        if(targetSymbol is not IMethodSymbol
            {
                ExplicitInterfaceImplementations: [],
                Parameters.Length: > 0,
                ReturnType: INamedTypeSymbol returnType,
                ContainingType: var containingType
            } target || !IsValueTask(returnType) && !IsTask(returnType))
        {
            return null;
        }

        var parameters = ctx.CollectionFactory.CreateList<ParameterModel>();
        var methodTypeParameters = ctx.CollectionFactory.CreateList<String>();

        var ctParamIndex = -1;

        for(var i = 0; i < target.Parameters.Length; i++)
        {
            ctx.ThrowIfCancellationRequested();

            var parameter = target.Parameters[i];

            if(IsCancellationToken(parameter.Type))
                ctParamIndex = i;

            parameters.Add(ParameterModel.Create(parameter, in ctx));
        }

        if(ctParamIndex is -1)
            return null;

        foreach(var param in target.TypeParameters)
        {
            ctx.ThrowIfCancellationRequested();
            methodTypeParameters.Add(param.Name);
        }

        var ctParameterName = parameters[ctParamIndex];

        parameters.RemoveAt(ctParamIndex);

        var typeArgumentName = target.ReturnType is INamedTypeSymbol { TypeArguments: [{ } arg] }
            ? arg.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
            : null;
        var containingTypeModel = NamedTypeModel.Create(containingType, in ctx);
        var accessibility = SyntaxFacts.GetText(target.DeclaredAccessibility);
        var isStatic = target.IsStatic;
        var methodName = target.Name;
        var returnTypeArgString = target.ReturnType is INamedTypeSymbol { TypeArguments: [{ } returnTypeTypeArg] }
            ? $"<{returnTypeTypeArg.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>"
            : String.Empty;
        var cancellableReturnType = $"global::RhoMicro.TplEx.Cancellable{target.ReturnType.Name}{returnTypeArgString}";

        var result = new CancellableMethodModel(
            containingTypeModel,
            accessibility,
            isStatic,
            methodName,
            methodTypeParameters,
            cancellableReturnType,
            typeArgumentName,
            parameters,
            ctParameterName);

        return result;
    }
}
