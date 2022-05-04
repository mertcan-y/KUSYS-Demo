namespace KUSYS.Core.Utilities.Results
{
    public interface IDataResult<out T> : IResult
    {
        T Data { get; }
        int TotalCount { get; }

    }
}
