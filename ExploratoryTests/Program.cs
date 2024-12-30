#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task

namespace ExploratoryTests;

using RhoMicro.TplEx;

using System;

[ExtendTpl]
internal static partial class Program
{
    static async Task Main()
    {
        await NestedAwaits();
        await ImperativeAwaits();
        await ChainedAwaits();
    }
    public static async Task ChainedAwaits()
    {
        // Example of fluent style awaits
        var result = StringWrapper
            .GetApiTokenAsync().Await().AuthenticateAsync

        Console.WriteLine(result);
    }

    public static async Task ImperativeAwaits()
    {
        // Example of imperative style awaits
        var token = await StringWrapper.GetApiTokenAsync();
        var authResult = token.AuthenticateAsync;
        var data = authResult.FetchDataAsync;
        var result = data.TransformDataAsync;

        Console.WriteLine(result);
    }
    public static async Task NestedAwaits()
    {
        // Example of nested awaits
        var result = ( await StringWrapper.GetApiTokenAsync() ).AuthenticateAsync.FetchDataAsync.TransformDataAsync;

        Console.WriteLine(result);
    }
}

internal readonly record struct StringWrapper(String Value)
{
    public static async Task<StringWrapper> GetApiTokenAsync()
    {
        await Task.Delay(100);
        return new StringWrapper("token");
    }
    public StringWrapper AuthenticateAsync => new($"authenticated-{Value}");
    public StringWrapper FetchDataAsync => new($"data-for-{Value}");
    public StringWrapper TransformDataAsync => new($"transformed-{Value}");

    public override String ToString() => Value;
}