namespace KUSYS.Core.Utilities.Results
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult() : base(default, true) { }
        public SuccessDataResult(string message) : base(default, true, message) { }
        public SuccessDataResult(T data) : base(data, true) { }
        public SuccessDataResult(T data, string message) : base(data, true, message) { }
        public SuccessDataResult(T data, int totalCount) : base(data, true, totalCount) { }
        public SuccessDataResult(T data, string message, int totalCount) : base(data, true, message, totalCount) { }
    }
}
