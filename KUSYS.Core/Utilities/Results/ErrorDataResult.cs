namespace KUSYS.Core.Utilities.Results
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public string ErrorDetail { get; set; }

        public ErrorDataResult() : base(default, false) { }
        public ErrorDataResult(string message) : base(default, false, message) { }
        public ErrorDataResult(string message, string errorDetail) : base(default, false, message)
        {
            ErrorDetail = errorDetail;
        }
        public ErrorDataResult(T data) : base(data, false) { }
        public ErrorDataResult(T data, string message) : base(data, false, message) { }
        public ErrorDataResult(T data, string message, string errorDetail) : base(data, false, message)
        {
            ErrorDetail = errorDetail;
        }
    }
}
