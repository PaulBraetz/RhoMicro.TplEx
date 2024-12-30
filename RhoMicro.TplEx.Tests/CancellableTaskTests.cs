#pragma warning disable IDE0007 // Use implicit type
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RhoMicro.TplEx.Tests;
public class CancellableTaskTests
{
    [Fact]
    public void CancellableTaskCreate_TaskIsNotCancelledImmediately()
    {
        CancellableTask task = CancellableTask.Create(ct =>
        {
            var tcs = new TaskCompletionSource();
            _ = ct.Register(tcs.SetCanceled);
            return tcs.Task;
        });

        Assert.False(task.Task.IsCanceled);
    }
    [Fact]
    public void CancellableTaskTCreate_TaskIsNotCancelledImmediately()
    {
        CancellableTask<Int32> task = CancellableTask.Create(ct =>
        {
            var tcs = new TaskCompletionSource<Int32>();
            _ = ct.Register(tcs.SetCanceled);
            return tcs.Task;
        });

        Assert.False(task.Task.IsCanceled);
    }
    [Fact]
    public void CancellableValueTaskCreate_TaskIsNotCancelledImmediately()
    {
        CancellableValueTask task = CancellableValueTask.Create(ct =>
        {
            var tcs = new TaskCompletionSource();
            _ = ct.Register(tcs.SetCanceled);
            return new(tcs.Task);
        });

        Assert.False(task.Task.IsCanceled);
    }
    [Fact]
    public void CancellableValueTaskTCreate_TaskIsNotCancelledImmediately()
    {
        CancellableValueTask<Int32> task = CancellableValueTask.Create(ct =>
        {
            var tcs = new TaskCompletionSource<Int32>();
            _ = ct.Register(tcs.SetCanceled);
            return new ValueTask<Int32>(tcs.Task);
        });

        Assert.False(task.Task.IsCanceled);
    }

    [Fact]
    public void CancellableTaskCreate_TaskIsCancelledViaCts()
    {
        CancellableTask task = CancellableTask.Create(ct =>
        {
            var tcs = new TaskCompletionSource();
            _ = ct.Register(tcs.SetCanceled);
            return tcs.Task;
        });

        task.CancellationTokenSource.Cancel();
        Assert.True(task.Task.IsCanceled);
    }
    [Fact]
    public void CancellableTaskTCreate_TaskIsCancelledViaCts()
    {
        CancellableTask<Int32> task = CancellableTask.Create(ct =>
        {
            var tcs = new TaskCompletionSource<Int32>();
            _ = ct.Register(tcs.SetCanceled);
            return tcs.Task;
        });

        task.CancellationTokenSource.Cancel();
        Assert.True(task.Task.IsCanceled);
    }
    [Fact]
    public void CancellableValueTaskCreate_TaskIsCancelledViaCts()
    {
        CancellableValueTask task = CancellableValueTask.Create(ct =>
        {
            var tcs = new TaskCompletionSource();
            _ = ct.Register(tcs.SetCanceled);
            return new(tcs.Task);
        });

        task.CancellationTokenSource.Cancel();
        Assert.True(task.Task.IsCanceled);
    }
    [Fact]
    public void CancellableValueTaskTCreate_TaskIsCancelledViaCts()
    {
        CancellableValueTask<Int32> task = CancellableValueTask.Create(ct =>
        {
            var tcs = new TaskCompletionSource<Int32>();
            _ = ct.Register(tcs.SetCanceled);
            return new ValueTask<Int32>(tcs.Task);
        });

        task.CancellationTokenSource.Cancel();
        Assert.True(task.Task.IsCanceled);
    }
}
