namespace KUSYS.Core.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public T Data { get; }
        public int TotalCount { get; }
        public DataResult()
        {

        }

        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }

        public DataResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }

        public DataResult(T data, bool success, int totalCount) : base(success)
        {
            Data = data;
            TotalCount = totalCount;
        }

        public DataResult(T data, bool success, string message, int totalCount) : base(success, message)
        {
            Data = data;
            TotalCount = totalCount;
        }

    }
}
