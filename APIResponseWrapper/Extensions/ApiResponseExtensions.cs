using Microsoft.EntityFrameworkCore;

namespace APIResponseWrapper.Extensions;

public static class ApiResponseExtensions
{
    /// <summary>
    /// Converts an <see cref="IQueryable{T}"/> to a paginated <see cref="ApiResponse{T}"/> with metadata.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="query">The source queryable (e.g., a DbSet or IQueryable).</param>
    /// <param name="page">The page number, starting from 1.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortOrder">The sort order expression (optional, e.g., "Name desc").</param>
    /// <param name="filter">The filter expression (optional).</param>
    /// <param name="sortingProperty">The property to sort by (optional, for custom sorting).</param>
    /// <returns>A task that represents the asynchronous operation, containing the <see cref="ApiResponse{T}"/>.</returns>
    public static async Task<ApiResponse<T>> ToApiResponseAsync<T>(
        this IQueryable<T> query,
        int page = 1,
        int pageSize = 10,
        string sortOrder = null,
        string filter = null,
        string sortingProperty = null)
    {
        try
        {
            // Apply the filter if provided
            if (!string.IsNullOrEmpty(filter))
            {
                query = ApplyFilter(query, filter, sortingProperty);
            }

            // Apply sorting if provided
            if (!string.IsNullOrEmpty(sortOrder))
            {
                query = ApplySorting(query, sortOrder);
            }

            // Get the total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination and get the items for the current page
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Create metadata for pagination
            var metaData = new MetaData(totalCount, page, pageSize);

            // Return the paginated response
            return ApiResponse<T>.CreateSuccessResponse(items, metaData);
        }
        catch (ArgumentException ex)
        {
            // Handle specific argument exceptions
            return ApiResponse<T>.CreateFailureResponse($"Argument Error: {ex.Message}", System.Net.HttpStatusCode.BadRequest);
        }
        catch (InvalidOperationException ex)
        {
            // Handle specific invalid operation exceptions (e.g., invalid query)
            return ApiResponse<T>.CreateFailureResponse($"Invalid Operation: {ex.Message}", System.Net.HttpStatusCode.InternalServerError);
        }
        catch (Exception ex)
        {
            // General exception handling for any unanticipated errors
            // Log the exception here for troubleshooting (e.g., using a logging framework)
            return ApiResponse<T>.CreateFailureResponse($"An unexpected error occurred: {ex.Message}", System.Net.HttpStatusCode.InternalServerError);
        }
    }

    #region Private Helper Methods

    /// <summary>
    /// Applies a filter to the query based on the specified filter string and property name.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="query">The source queryable.</param>
    /// <param name="filter">The filter string.</param>
    /// <param name="propertyName">The property name to apply the filter to.</param>
    /// <returns>The filtered query.</returns>
    private static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, string filter, string propertyName)
    {
        try
        {
            if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(propertyName))
            {
                query = query.Where(item => EF.Property<string>(item, propertyName).Contains(filter));
            }
            return query;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Error applying filter: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Applies sorting to the query based on the specified sort order string.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="query">The source queryable.</param>
    /// <param name="sortOrder">The sort order string (e.g., "Name desc").</param>
    /// <returns>The sorted query.</returns>
    private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortOrder)
    {
        try
        {
            if (string.IsNullOrEmpty(sortOrder)) return query;

            // Split the sortOrder into property and direction (ascending or descending)
            var sortParts = sortOrder.Split(' ');
            var property = sortParts[0];
            var direction = sortParts.Length > 1 && sortParts[1].Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? "desc" : "asc";

            // Apply sorting dynamically
            if (direction == "desc")
            {
                query = query.OrderByDescending(x => EF.Property<object>(x, property));
            }
            else
            {
                query = query.OrderBy(x => EF.Property<object>(x, property));
            }

            return query;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Error applying sorting: {ex.Message}", ex);
        }
    }

    #endregion
}
