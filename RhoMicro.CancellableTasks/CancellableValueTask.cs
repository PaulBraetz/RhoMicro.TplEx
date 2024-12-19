namespace RhoMicro.CancellableTasks;

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

/// <summary>
/// Wraps a <see cref="ValueTask"/> instance and the <see
/// cref="System.Threading.CancellationTokenSource"/> to request its execution
/// to be cancelled.
/// </summary>
/// <param name="Task">
/// The <see cref="ValueTask"/> to provide cancellation for.
/// </param>
/// <param name="CancellationTokenSource">
/// The <see cref="System.Threading.CancellationTokenSource"/> via which to
/// request cancellation of <see cref="Task"/>.
/// </param>
public readonly record struct CancellableValueTask(
    ValueTask Task,
    CancellationTokenSource CancellationTokenSource)
{
    /// <summary>
    /// Creates a new <see cref="CancellableValueTask"/> from a factory for a
    /// <see cref="ValueTask"/>. The factory will be invoked using <paramref
    /// name="state"/> and a <see cref="CancellationToken"/> sourced from the
    /// <see cref="System.Threading.CancellationTokenSource"/>
    /// wrapped by the result.
    /// </summary>
    /// <typeparam name="TState">
    /// The type of state to pass to the factory.
    /// </typeparam>
    /// <param name="factory">
    /// The factory to use when creating the <see cref="ValueTask"/>.
    /// </param>
    /// <param name="state">
    /// The state to pass to <paramref name="factory"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="CancellableValueTask"/> wrapping the <see
    /// cref="ValueTask"/> created by <paramref name="factory"/>.
    /// </returns>
    public static CancellableValueTask Create<TState>(Func<TState, CancellationToken, ValueTask> factory, TState state)
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));

        var cts = new CancellationTokenSource();
        var task = factory.Invoke(state, cts.Token);
        var result = new CancellableValueTask(task, cts);

        return result;
    }
    /// <summary>
    /// Creates a new <see cref="CancellableValueTask"/> from a factory for a
    /// <see cref="ValueTask"/>. The factory will be invoked using a <see
    /// cref="CancellationToken"/> sourced from the
    /// <see cref="System.Threading.CancellationTokenSource"/>
    /// wrapped by the result.
    /// </summary>
    /// <param name="factory">
    /// The factory to use when creating the <see cref="ValueTask"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="CancellableValueTask"/> wrapping the <see
    /// cref="ValueTask"/> created by <paramref name="factory"/>.
    /// </returns>
    public static CancellableValueTask Create(Func<CancellationToken, ValueTask> factory)
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));

        var cts = new CancellationTokenSource();
        var task = factory.Invoke(cts.Token);
        var result = new CancellableValueTask(task, cts);

        return result;
    }
    /// <summary>
    /// Creates a new <see cref="CancellableValueTask"/> from a factory for a
    /// <see cref="ValueTask{TResult}"/>. The factory will be invoked using
    /// <paramref name="state"/> and a <see cref="CancellationToken"/> sourced
    /// from the
    /// <see cref="System.Threading.CancellationTokenSource"/>
    /// wrapped by the result.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the result produced by the wrapped <see
    /// cref="ValueTask{TResult}"/>.
    /// </typeparam>
    /// <typeparam name="TState">
    /// The type of state to pass to the factory.
    /// </typeparam>
    /// <param name="factory">
    /// The factory to use when creating the <see cref="ValueTask{TResult}"/>.
    /// </param>
    /// <param name="state">
    /// The state to pass to <paramref name="factory"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="CancellableValueTask"/> wrapping the <see
    /// cref="ValueTask{TResult}"/> created by <paramref name="factory"/>.
    /// </returns>
    public static CancellableValueTask<T> Create<T, TState>(Func<TState, CancellationToken, ValueTask<T>> factory, TState state)
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));

        var cts = new CancellationTokenSource();
        var task = factory.Invoke(state, cts.Token);
        var result = new CancellableValueTask<T>(task, cts);

        return result;
    }
    /// <summary>
    /// Creates a new <see cref="CancellableValueTask"/> from a factory for a
    /// <see cref="ValueTask{TResult}"/>. The factory will be invoked using a
    /// <see cref="CancellationToken"/> sourced from the
    /// <see cref="System.Threading.CancellationTokenSource"/>
    /// wrapped by the result.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the result produced by the wrapped <see
    /// cref="ValueTask{TResult}"/>.
    /// </typeparam>
    /// <param name="factory">
    /// The factory to use when creating the <see cref="ValueTask{TResult}"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="CancellableValueTask"/> wrapping the <see
    /// cref="ValueTask{TResult}"/> created by <paramref name="factory"/>.
    /// </returns>
    public static CancellableValueTask<T> Create<T>(Func<CancellationToken, ValueTask<T>> factory)
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));

        var cts = new CancellationTokenSource();
        var task = factory.Invoke(cts.Token);
        var result = new CancellableValueTask<T>(task, cts);

        return result;
    }
    /// <summary>
    /// Gets an awaiter used to await this <see cref="CancellableValueTask"/>.
    /// </summary>
    /// <returns>
    /// A new awaiter instance.
    /// </returns>
    public ValueTaskAwaiter GetAwaiter() => Task.GetAwaiter();
    /// <summary>
    /// Implicitly converts an instance of <see cref="CancellableValueTask"/> to the underlying <see cref="Task"/> instance.
    /// </summary>
    /// <param name="cancellableValueTask">
    /// The <see cref="CancellableValueTask"/> to convert.
    /// </param>
    public static implicit operator Task(CancellableValueTask cancellableValueTask) => cancellableValueTask.Task.AsTask();
    /// <summary>
    /// Implicitly converts an instance of <see cref="CancellableValueTask"/> to the underlying <see cref="ValueTask"/> instance.
    /// </summary>
    /// <param name="cancellableValueTask">
    /// The <see cref="CancellableValueTask"/> to convert.
    /// </param>
    public static implicit operator ValueTask(CancellableValueTask cancellableValueTask) => cancellableValueTask.Task;
}

/// <summary>
/// Wraps a <see cref="ValueTask{TResult}"/> instance and the <see
/// cref="System.Threading.CancellationTokenSource"/> to request its execution
/// to be cancelled.
/// </summary>
/// <typeparam name="TResult">
/// The type of result produced by this <see
/// cref="CancellableValueTask{TResult}"/>.
/// </typeparam>
/// <param name="Task">
/// The <see cref="ValueTask{TResult}"/> to provide cancellation for.
/// </param>
/// <param name="CancellationTokenSource">
/// The <see cref="System.Threading.CancellationTokenSource"/> via which to
/// request cancellation of <see cref="Task"/>.
/// </param>
public readonly record struct CancellableValueTask<TResult>(
    ValueTask<TResult> Task,
    CancellationTokenSource CancellationTokenSource)
{
    /// <summary>
    /// Gets an awaiter used to await this <see
    /// cref="CancellableValueTask{TResult}"/>.
    /// </summary>
    /// <returns>
    /// A new awaiter instance.
    /// </returns>
    public ValueTaskAwaiter<TResult> GetAwaiter() => Task.GetAwaiter();
    /// <summary>
    /// Implicitly converts an instance of <see cref="CancellableValueTask{TResult}"/> to the underlying <see cref="Task{TResult}"/> instance.
    /// </summary>
    /// <param name="cancellableValueTask">
    /// The <see cref="CancellableValueTask{TResult}"/> to convert.
    /// </param>
    public static implicit operator Task<TResult>(CancellableValueTask<TResult> cancellableValueTask) => cancellableValueTask.Task.AsTask();
    /// <summary>
    /// Implicitly converts an instance of <see cref="CancellableValueTask{TResult}"/> to the underlying <see cref="ValueTask{TResult}"/> instance.
    /// </summary>
    /// <param name="cancellableValueTask">
    /// The <see cref="CancellableValueTask{TResult}"/> to convert.
    /// </param>
    public static implicit operator ValueTask<TResult>(CancellableValueTask<TResult> cancellableValueTask) => cancellableValueTask.Task;
}
