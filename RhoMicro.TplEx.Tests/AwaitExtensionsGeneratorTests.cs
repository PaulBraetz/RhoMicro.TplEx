#pragma warning disable IDE0007 // Use implicit type
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RhoMicro.TplEx.Tests;

using System.Globalization;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.TplEx.PostfixAwait;
using RhoMicro.TplEx.FileInclusion;

public partial class PostfixAwaitGeneratorTests
{
    public static TheoryData<String, String, String, Action<INamedTypeSymbol>> WrapperTypeTestData =>
            new(){
                {
                    "complex constellation of implicit and explicit usage",
                    """
                    namespace ExploratoryTests;

                    using RhoMicro.TplEx;

                    [ExtendTpl]
                    internal static partial class Program
                    {
                        static async Task Main()
                        {
                            await NestedAwaits();
                            await ChainedAwaits();
                        }
                        public static async Task ChainedAwaits()
                        {
                            // Example of chained awaits
                            var result = await GetApiTokenAsync().Await()
                                .AuthenticateAsync().Await()
                                .FetchDataAsync().Await()
                                .TransformDataAsync();

                            Console.WriteLine(result);
                        }

                        public static async Task NestedAwaits()
                        {
                            // Example of deeply nested awaits
                            var result = await TransformDataAsync(
                                await FetchDataAsync(
                                    await AuthenticateAsync(
                                        await GetApiTokenAsync()
                                    )
                                )
                            );

                            Console.WriteLine(result);
                        }

                        private static async Task<String> GetApiTokenAsync()
                        {
                            await Task.Delay(100); // Simulating async work
                            return "token";
                        }

                        private static async Task<String> AuthenticateAsync(this String token)
                        {
                            await Task.Delay(100); // Simulating async work
                            return $"authenticated-{token}";
                        }

                        private static async Task<String> FetchDataAsync(this String authToken)
                        {
                            await Task.Delay(100); // Simulating async work
                            return $"data-for-{authToken}";
                        }

                        private static async Task<String> TransformDataAsync(this String data)
                        {
                            await Task.Delay(100); // Simulating async work
                            return $"transformed-{data}";
                        }
                    }
                    ""","ExploratoryTests.Program",t=>{}
                },
                {
                    "TaskWrapper from stub await has property of type ValueTask<> of wrapped property",
                    """
                    using System;
                    using System.Threading;
                    using System.Threading.Tasks;
                    using RhoMicro.TplEx;

                    [ExtendTpl]
                    partial class Foo
                    {
                        public Task<String> Bar() => Task.FromResult(new Bar()).Await().StringProperty;
                    }

                    class Bar
                    {
                        public string StringProperty { get; set; }
                    }
                    ""","Bar+TaskWrapper",t=>Assert.Single(t.GetMembers().OfType<IPropertySymbol>(),p=>p is { Name:"StringProperty", Type:INamedTypeSymbol{Name:"ValueTask", TypeParameters:[{SpecialType: SpecialType.System_String}]} })
                },
                {
                    "TaskWrapper from stub await has property of type ValueTask<> of wrapped property",
                    """
                    using System;
                    using System.Threading;
                    using System.Threading.Tasks;
                    using RhoMicro.TplEx;

                    [ExtendTpl]
                    partial class Foo
                    {
                        public Task<String> Bar() => Task.FromResult(new Bar()).Await().FooBarProperty.Await().StringProperty;
                    }

                    class Bar
                    {
                        public FooBar FooBarProperty { get; set; }
                    }
                    
                    class FooBar
                    {
                        public string StringProperty { get; set; }
                    }
                    ""","Bar+TaskWrapper",t=>Assert.Single(t.GetMembers().OfType<IPropertySymbol>(),p=>p is { Name:"FooBarProperty", Type.Name:"ValueTask" })
                },
                {
                    "ValueTaskWrapper from generated await has property of type ValueTask<> of wrapped property",
                    """
                    using System;
                    using System.Threading;
                    using System.Threading.Tasks;
                    using RhoMicro.TplEx;

                    [ExtendTpl]
                    partial class Foo
                    {
                        public Task<String> Bar() => Task.FromResult(new Bar()).Await().FooBarProperty.Await().StringProperty;
                    }

                    class Bar
                    {
                        public FooBar FooBarProperty { get; set; }
                    }
                    
                    class FooBar
                    {
                        public string StringProperty { get; set; }
                    }
                    ""","FooBar+ValueTaskWrapper",t=>Assert.Single(t.GetMembers().OfType<IPropertySymbol>(),p=>p is { Name:"StringProperty", Type.Name:"ValueTask" })
                },
                {
                    "TaskExtensions has generated Await for TaskWrapper with param Task",
                    """
                    using System;
                    using System.Threading;
                    using System.Threading.Tasks;
                    using RhoMicro.TplEx;

                    [ExtendTpl]
                    partial class Foo
                    {
                        public Task<String> Bar() => Task.FromResult(new Bar()).Await().StringProperty;
                    }

                    class Bar
                    {
                        public string StringProperty { get; set; }
                    }
                    ""","TaskExtensions",t=>Assert.Single(t.GetMembers().OfType<IMethodSymbol>(),m=>m is {Name: "Await", Parameters:[{Type.Name: "Task"}], ReturnType:{ContainingType.Name:"Foo", Name: "TaskWrapper"}})
                }};
    private static Int32 _testAssemblyIndex;
    [Theory]
    [MemberData(nameof(WrapperTypeTestData))]
    public void GeneratesExpectedNamedTypeSymbol(String description, String source, String typeSymbolName, Action<INamedTypeSymbol> assertion)
    {
        ArgumentNullException.ThrowIfNull(assertion);

        var syntaxTree = CSharpSyntaxTree.ParseText(
            source,
            new CSharpParseOptions(LanguageVersion.Latest));

        var compilation = CSharpCompilation.Create(
            $"TestAssembly_{Interlocked.Increment(ref _testAssemblyIndex)}",
            [syntaxTree],
            Basic.Reference.Assemblies.NetStandard20.References.All);

        // We ignore diagnostics of original compilation as we want to assert
        // generation upon encountering incomplete syntax (member access).
        //if(compilation.GetDiagnostics() is { IsEmpty: false } diagnostics)
        //    Assert.Fail($"Source compiled with diagnostics:{String.Concat(diagnostics.Select(d => $"\n{d.GetMessage(CultureInfo.InvariantCulture)}"))}");

        var generatorDriver = CSharpGeneratorDriver.Create(new PostfixAwaitGenerator(), new FileInclusionGenerator());

        _ = generatorDriver.RunGeneratorsAndUpdateCompilation(compilation, out var compilationWithGeneratedSources, out var generatedDiagnostics);

        if(generatedDiagnostics is { IsEmpty: false })
            Assert.Fail($"{description}\nSource compiled with diagnostics:{String.Concat(generatedDiagnostics.Select(d => $"\n{d}"))}");

        if(compilationWithGeneratedSources.GetDiagnostics().Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error).ToArray() is { Length: > 0 } d)
            Assert.Fail($"{description}\nSource compiled with diagnostics:{String.Concat(d.Select(d => $"\n{d}"))}");

        if(compilationWithGeneratedSources.Assembly.GetTypeByMetadataName(typeSymbolName) is not { } wrapperType)
        {
            Assert.Fail($"{description}\nUnable to locate named type '{typeSymbolName}'.");
            return;
        }

        try
        {
            assertion.Invoke(wrapperType);
        } catch(Exception ex)
        {
            Assert.Fail($"{description}\n{ex}");
        }
    }
}

file static class Extensions
{
    public static IEnumerable<ISymbol> GetDescendantMembersAndSelf(this INamedTypeSymbol symbol) => [symbol, .. symbol.GetMembers().OfType<INamedTypeSymbol>().SelectMany(m => m.GetDescendantMembersAndSelf())];
}