namespace PersonalSite.Domain.Common.Results;

public class PaginatedResult<T> : Result<List<T>>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;

    private PaginatedResult(
        List<T> value,
        int pageNumber,
        int pageSize,
        int totalCount)
        : base(true, value, null)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    private PaginatedResult(string error)
        : base(false, default, error)
    {
        PageNumber = 0;
        PageSize = 0;
        TotalCount = 0;
    }

    public static PaginatedResult<T> Success(
        List<T> value,
        int pageNumber,
        int pageSize,
        int totalCount)
    {
        return new PaginatedResult<T>(value, pageNumber, pageSize, totalCount);
    }

    public new static PaginatedResult<T> Failure(string error)
    {
        return new PaginatedResult<T>(error);
    }
}