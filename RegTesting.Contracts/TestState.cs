namespace RegTesting.Contracts
{

    /// <summary>
    /// The different possible TestStates
    /// </summary>
    public enum TestState
    {
        NotSet = 0,
        Canceled = 2,
        NotSupported = 3,
		NotAvailable = 4,
        Success = 5,
		KnownError=8,
        ErrorRepeat = 9,
        Error = 10,
        Pending = 15,
        Running = 18
    }
}
