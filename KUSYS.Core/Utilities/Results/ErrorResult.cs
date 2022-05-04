namespace KUSYS.Core.Utilities.Results
{
    public class ErrorResult : Result
    {
        public string ErrorDetail { get; set; }
        public ErrorResult() : base(false) { }
        public ErrorResult(string message) : base(false, message) { }
        public ErrorResult(string message, string errorDetail) : base(false, message) 
        {
            ErrorDetail = errorDetail;
        }
    }
}
