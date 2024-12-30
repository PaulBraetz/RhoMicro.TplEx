namespace RhoMicro.TplEx;

/// <summary>
/// Provides extensions for <see cref="Task"/> and <see cref="ValueTask"/> types.
/// </summary>
#if RHOMICRO_TPLEX_GENERATORS
[CodeAnalysis.IncludeFile]
#endif
internal static partial class TaskExtensions
{
    /// <summary>
    /// Gets an awaitable wrapper for the result of the task provided. 
    /// </summary>
    /// <remarks>
    /// This method serves as a marker for generating dedicated overloads for
    /// invocations with specific arguments for <typeparamref name="T"/>.
    /// </remarks>
    /// <param name="task">
    /// The task whose result to get an awaitable wrapper for.
    /// </param>
    /// <returns>
    /// An awaitable wrapper for the result returned by <paramref name="task"/>.
    /// </returns>
    public static T Await<T>(this Task<T> task) => 
        throw new InvalidOperationException($"The stub (marker) method 'Await<T>(this Task<T> task)' for postfix await expressions may not be called. This indicates either a bug in the PostfixAwaitGenerator or an incorrect usage thereof. Make sure that any type containing members using this method are annotated with the '{typeof(ExtendTplAttribute).FullName}' attribute.");
    /// <summary>
    /// Gets an awaitable wrapper for the result of the task provided. 
    /// </summary>
    /// <remarks>
    /// This method serves as a marker for generating dedicated overloads for
    /// invocations with specific arguments for <typeparamref name="T"/>.
    /// </remarks>
    /// <param name="task">
    /// The task whose result to get an awaitable wrapper for.
    /// </param>
    /// <returns>
    /// An awaitable wrapper for the result returned by <paramref name="task"/>.
    /// </returns>
    public static T Await<T>(this ValueTask<T> task) =>
        throw new InvalidOperationException($"The stub (marker) method 'Await<T>(this ValueTask<T> task)' for postfix await expressions may not be called. This indicates either a bug in the PostfixAwaitGenerator or an incorrect usage thereof. Make sure that any type containing members using this method are annotated with the '{typeof(ExtendTplAttribute).FullName}' attribute.");
}
