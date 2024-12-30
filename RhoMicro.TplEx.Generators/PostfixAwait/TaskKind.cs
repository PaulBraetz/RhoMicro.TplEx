namespace RhoMicro.TplEx.PostfixAwait;

internal enum TaskKind
{
    Task,
    ValueTask
}

internal static class Extensions
{
    public static String ToStringFast(this TaskKind taskKind)=>
        taskKind switch
        {
            TaskKind.Task => "Task",
            TaskKind.ValueTask => "ValueTask",
            _ => throw new InvalidOperationException($"Unknown task kind: '{taskKind}'")
        };
}