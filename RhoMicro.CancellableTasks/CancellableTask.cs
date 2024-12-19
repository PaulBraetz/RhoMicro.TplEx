namespace RhoMicro.CancellableTasks;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Wraps a <see cref="System.Threading.Tasks.Task"/> instance and the <see
/// cref="System.Threading.CancellationTokenSource"/> to request its execution
/// to be cancelled.
/// </summary>
/// <param name="Task">
/// The <see cref="System.Threading.Tasks.Task"/> to provide cancellation for.
/// </param>
/// <param name="CancellationTokenSource">
/// The <see cref="System.Threading.CancellationTokenSource"/> via which to
/// request cancellation of <see cref="Task"/>.
/// </param>
public readonly record struct CancellableTask(
    Task Task,
    CancellationTokenSource CancellationTokenSource)
{
    /// <summary>
    /// Creates a new <see cref="CancellableTask"/> from a factory for a <see
    /// cref="System.Threading.Tasks.Task"/>. The factory will be invoked using
    /// <paramref name="state"/> and a <see cref="CancellationToken"/> sourced
    /// from the
    /// <see cref="System.Threading.CancellationTokenSource"/>
    /// wrapped by the result.
    /// </summary>
    /// <typeparam name="TState">
    /// The type of state to pass to the factory.
    /// </typeparam>
    /// <param name="factory">
    /// The factory to use when creating the <see
    /// cref="System.Threading.Tasks.Task"/>.
    /// </param>
    /// <param name="state">
    /// The state to pass to <paramref name="factory"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="CancellableTask"/> wrapping the <see
    /// cref="System.Threading.Tasks.Task"/> created by <paramref
    /// name="factory"/>.
    /// </returns>
    public static CancellableTask Create<TState>(Func<TState, CancellationToken, Task> factory, TState state)
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));

        var cts = new CancellationTokenSource();
        var task = factory.Invoke(state, cts.Token);
        var result = new CancellableTask(task, cts);

        return result;
    }
    /// <summary>
    /// Creates a new <see cref="CancellableTask"/> from a factory for a <see
    /// cref="System.Threading.Tasks.Task"/>. The factory will be invoked using
    /// a <see cref="CancellationToken"/> sourced from the
    /// <see cref="System.Threading.CancellationTokenSource"/>
    /// wrapped by the result.
    /// </summary>
    /// <param name="factory">
    /// The factory to use when creating the <see
    /// cref="System.Threading.Tasks.Task"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="CancellableTask"/> wrapping the <see
    /// cref="System.Threading.Tasks.Task"/> created by <paramref
    /// name="factory"/>.
    /// </returns>
    public static CancellableTask Create(Func<CancellationToken, Task> factory)
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));

        var cts = new CancellationTokenSource();
        var task = factory.Invoke(cts.Token);
        var result = new CancellableTask(task, cts);

        return result;
    }
    /// <summary>
    /// Creates a new <see cref="CancellableTask"/> from a factory for a <see
    /// cref="Task{TResult}"/>. The factory will be invoked using <paramref
    /// name="state"/> and a <see cref="CancellationToken"/> sourced from the
    /// <see cref="System.Threading.CancellationTokenSource"/>
    /// wrapped by the result.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the result produced by the wrapped <see
    /// cref="Task{TResult}"/>.
    /// </typeparam>
    /// <typeparam name="TState">
    /// The type of state to pass to the factory.
    /// </typeparam>
    /// <param name="factory">
    /// The factory to use when creating the <see cref="Task{TResult}"/>.
    /// </param>
    /// <param name="state">
    /// The state to pass to <paramref name="factory"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="CancellableTask"/> wrapping the <see
    /// cref="Task{TResult}"/> created by <paramref name="factory"/>.
    /// </returns>
    public static CancellableTask<T> Create<T, TState>(Func<TState, CancellationToken, Task<T>> factory, TState state)
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        
        var cts = new CancellationTokenSource();
        var task = factory.Invoke(state, cts.Token);
        var result = new CancellableTask<T>(task, cts);

        return result;
    }
    /// <summary>
    /// Creates a new <see cref="CancellableTask"/> from a factory for a <see
    /// cref="Task{TResult}"/>. The factory will be invoked using a <see
    /// cref="CancellationToken"/> sourced from the
    /// <see cref="System.Threading.CancellationTokenSource"/>
    /// wrapped by the result.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the result produced by the wrapped <see
    /// cref="Task{TResult}"/>.
    /// </typeparam>
    /// <param name="factory">
    /// The factory to use when creating the <see cref="Task{TResult}"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="CancellableTask"/> wrapping the <see
    /// cref="Task{TResult}"/> created by <paramref name="factory"/>.
    /// </returns>
    public static CancellableTask<T> Create<T>(Func<CancellationToken, Task<T>> factory)
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));

        var cts = new CancellationTokenSource();
        var task = factory.Invoke(cts.Token);
        var result = new CancellableTask<T>(task, cts);

        return result;
    }
    /// <summary>
    /// Gets an awaiter used to await this <see cref="CancellableTask"/>.
    /// </summary>
    /// <returns>
    /// A new awaiter instance.
    /// </returns>
    public TaskAwaiter GetAwaiter() => Task.GetAwaiter();
    /// <summary>
    /// Implicitly converts an instance of <see cref="CancellableTask"/> to the underlying <see cref="Task"/> instance.
    /// </summary>
    /// <param name="cancellableTask">
    /// The <see cref="CancellableTask"/> to convert.
    /// </param>
    public static implicit operator Task(CancellableTask cancellableTask) => cancellableTask.Task;
    /// <summary>
    /// Implicitly converts an instance of <see cref="CancellableTask"/> to the underlying <see cref="ValueTask"/> instance.
    /// </summary>
    /// <param name="cancellableTask">
    /// The <see cref="CancellableTask"/> to convert.
    /// </param>
    public static implicit operator ValueTask(CancellableTask cancellableTask) => new(cancellableTask.Task);
}

/// <summary>
/// Wraps a <see cref="Task{TResult}"/> instance and the <see
/// cref="System.Threading.CancellationTokenSource"/> to request its execution
/// to be cancelled.
/// </summary>
/// <typeparam name="TResult">
/// The type of result produced by this <see cref="CancellableTask{TResult}"/>.
/// </typeparam>
/// <param name="Task">
/// The <see cref="Task{TResult}"/> to provide cancellation for.
/// </param>
/// <param name="CancellationTokenSource">
/// The <see cref="System.Threading.CancellationTokenSource"/> via which to
/// request cancellation of <see cref="Task"/>.
/// </param>
public readonly record struct CancellableTask<TResult>(
    Task<TResult> Task,
    CancellationTokenSource CancellationTokenSource)
{
    /// <summary>
    /// Gets an awaiter used to await this <see cref="CancellableTask{T}"/>.
    /// </summary>
    /// <returns>
    /// A new awaiter instance.
    /// </returns>
    public TaskAwaiter<TResult> GetAwaiter() => Task.GetAwaiter();
    /// <summary>
    /// Implicitly converts an instance of <see cref="CancellableTask{TResult}"/> to the underlying <see cref="Task{TResult}"/> instance.
    /// </summary>
    /// <param name="cancellableTask">
    /// The <see cref="CancellableTask{TResult}"/> to convert.
    /// </param>
    public static implicit operator Task<TResult>(CancellableTask<TResult> cancellableTask) => cancellableTask.Task;
    /// <summary>
    /// Implicitly converts an instance of <see cref="CancellableTask{TResult}"/> to the underlying <see cref="ValueTask{TResult}"/> instance.
    /// </summary>
    /// <param name="cancellableTask">
    /// The <see cref="CancellableTask{TResult}"/> to convert.
    /// </param>
    public static implicit operator ValueTask<TResult>(CancellableTask<TResult> cancellableTask) => new(cancellableTask.Task);
}
