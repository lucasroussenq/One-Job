namespace OneJob.Reactor
{
    public enum ReactorState
    {
        Normal = 0,
        Warning = 1,
        Critical = 2,
        Exploded = 3
    }

    public enum RoundResult
    {
        InProgress = 0,
        Success = 1,
        Failure = 2
    }
}
