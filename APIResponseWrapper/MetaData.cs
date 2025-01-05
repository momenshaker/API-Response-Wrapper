namespace APIResponseWrapper;

/// <summary>
/// Represents pagination and other metadata for API responses.
/// </summary>
public class MetaData
{
    /// <summary>
    /// Gets or sets the total number of items available.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages available based on the <see cref="TotalCount"/> and <see cref="PageSize"/>.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the sort order (optional).
    /// </summary>
    public string SortOrder { get; set; }

    /// <summary>
    /// Gets or sets the filter query (optional).
    /// </summary>
    public string Filter { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MetaData"/> class.
    /// </summary>
    /// <param name="totalCount">The total number of items.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortOrder">The sort order (optional).</param>
    /// <param name="filter">The filter query (optional).</param>
    public MetaData(int totalCount, int page, int pageSize, string sortOrder = null, string filter = null)
    {
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
        SortOrder = sortOrder;
        Filter = filter;
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
    }
}
