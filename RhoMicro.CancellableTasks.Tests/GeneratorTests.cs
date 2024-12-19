#pragma warning disable IDE0007 // Use implicit type
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RhoMicro.CancellableTasks.Tests;

public partial class GeneratorTests
{
    public Task InstanceTaskEmpty(CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public Task InstanceTaskOneOrdered(Int32 a, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }
    public Task InstanceTaskMultipleOrdered(Int32 a, Object b, DateTime c, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public Task InstanceTaskOneUnordered(CancellationToken ct, Int32 a)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }
    public Task InstanceTaskMultipleUnordered(CancellationToken ct, Int32 a, Object b, DateTime c)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public static Task StaticTaskEmpty(CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public static Task StaticTaskOneOrdered(Int32 a, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }
    public static Task StaticTaskMultipleOrdered(Int32 a, Object b, DateTime c, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public static Task StaticTaskOneUnordered(CancellationToken ct, Int32 a)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }
    public static Task StaticTaskMultipleUnordered(CancellationToken ct, Int32 a, Object b, DateTime c)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public Task<Int32> InstanceTaskTEmpty(CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public Task<Int32> InstanceTaskTOneOrdered(Int32 a, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }
    public Task<Int32> InstanceTaskTMultipleOrdered(Int32 a, Object b, DateTime c, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public Task<Int32> InstanceTaskTOneUnordered(CancellationToken ct, Int32 a)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }
    public Task<Int32> InstanceTaskTMultipleUnordered(CancellationToken ct, Int32 a, Object b, DateTime c)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public static Task<Int32> StaticTaskTEmpty(CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public static Task<Int32> StaticTaskTOneOrdered(Int32 a, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }
    public static Task<Int32> StaticTaskTMultipleOrdered(Int32 a, Object b, DateTime c, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public static Task<Int32> StaticTaskTOneUnordered(CancellationToken ct, Int32 a)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }
    public static Task<Int32> StaticTaskTMultipleUnordered(CancellationToken ct, Int32 a, Object b, DateTime c)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return tcs.Task;
    }

    public ValueTask InstanceValueTaskEmpty(CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }

    public ValueTask InstanceValueTaskOneOrdered(Int32 a, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }
    public ValueTask InstanceValueTaskMultipleOrdered(Int32 a, Object b, DateTime c, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }

    public ValueTask InstanceValueTaskOneUnordered(CancellationToken ct, Int32 a)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }
    public ValueTask InstanceValueTaskMultipleUnordered(CancellationToken ct, Int32 a, Object b, DateTime c)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }

    public static ValueTask StaticValueTaskEmpty(CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }

    public static ValueTask StaticValueTaskOneOrdered(Int32 a, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }
    public static ValueTask StaticValueTaskMultipleOrdered(Int32 a, Object b, DateTime c, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }

    public static ValueTask StaticValueTaskOneUnordered(CancellationToken ct, Int32 a)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }
    public static ValueTask StaticValueTaskMultipleUnordered(CancellationToken ct, Int32 a, Object b, DateTime c)
    {
        var tcs = new TaskCompletionSource();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }

    public ValueTask<Int32> InstanceValueTaskTEmpty(CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }

    public ValueTask<Int32> InstanceValueTaskTOneOrdered(Int32 a, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }
    public ValueTask<Int32> InstanceValueTaskTMultipleOrdered(Int32 a, Object b, DateTime c, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }

    public ValueTask<Int32> InstanceValueTaskTOneUnordered(CancellationToken ct, Int32 a)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }
    public ValueTask<Int32> InstanceValueTaskTMultipleUnordered(CancellationToken ct, Int32 a, Object b, DateTime c)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }

    public static ValueTask<Int32> StaticValueTaskTEmpty(CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }

    public static ValueTask<Int32> StaticValueTaskTOneOrdered(Int32 a, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }
    public static ValueTask<Int32> StaticValueTaskTMultipleOrdered(Int32 a, Object b, DateTime c, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }

    public static ValueTask<Int32> StaticValueTaskTOneUnordered(CancellationToken ct, Int32 a)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }
    public static ValueTask<Int32> StaticValueTaskTMultipleUnordered(CancellationToken ct, Int32 a, Object b, DateTime c)
    {
        var tcs = new TaskCompletionSource<Int32>();
        _ = ct.Register(tcs.SetCanceled);
        return new(tcs.Task);
    }
}
