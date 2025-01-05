`# APIResponseWrapper

`APIResponseWrapper` is a utility library designed to standardize and simplify API responses in .NET applications. It provides a common response format for API calls, ensuring that all responses---whether successful or not---follow a consistent structure. The library also includes extensions for handling common operations like pagination, filtering, and sorting in a way that makes API responses more uniform.

## Features

- **Standardized API Responses**: A consistent response format that includes success status, message, data, metadata, and headers.
- **Pagination Support**: Convert IQueryable into a paginated response with metadata, including total count and pages.
- **Sorting & Filtering**: Utility functions for sorting and filtering IQueryable data.
- **Error Handling**: Built-in error handling with appropriate HTTP status codes and failure messages.
- **Metadata**: Metadata is included for paginated responses to inform clients about the number of pages, the current page, and the page size.

## Installation

To install this library, you can add the project to your solution, or install via NuGet (if available).

### Using NuGet Package

If this project is hosted as a NuGet package, you can install it using the following command:

```bash
dotnet add package APIResponseWrapper `

Or via NuGet Package Manager in Visual Studio.

Usage
-----

### `ApiResponse<T>`

`ApiResponse<T>` is a generic class that represents the standard format for all API responses.

`public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public MetaData Meta { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public Dictionary<string, string> Headers { get; set; }
}`

### Example of Creating a Successful Response

You can create a successful response with data as follows:

`var response = ApiResponse<string>.SuccessResponse("Hello World!");`

If you want to include pagination metadata in the response:

`var data = new List<string> { "Item1", "Item2", "Item3" };
var metaData = new MetaData(100, 1, 10); // total count = 100, page = 1, pageSize = 10
var response = ApiResponse<string>.SuccessResponse(data, metaData);`

### Example of Creating a Failure Response

You can create a failure response with an error message as follows:

`var errorResponse = ApiResponse<string>.FailureResponse("An error occurred");`

You can also specify an HTTP status code:

`var notFoundResponse = ApiResponse<string>.NotFoundResponse("Data not found");`

### Extensions for IQueryable (Sorting, Filtering, Pagination)

`ApiResponseExtensions` provide helper methods to handle common operations like sorting, filtering, and pagination for `IQueryable` sources like `DbSet<T>`.

#### Example: Using `ToApiResponseAsync`

`public async Task<IActionResult> GetPagedData()
{
    var data = _dbContext.Items.AsQueryable();

    var response = await data.ToApiResponseAsync(
        page: 1,
        pageSize: 10,
        sortOrder: "Name asc",
        filter: "someFilter",
        sortingProperty: "Name"
    );

    return Ok(response);
}`

This will return a paginated response along with the metadata for the list of items.

### Error Handling

The library includes built-in error handling for common errors that can occur during pagination, sorting, or filtering:

-   **ArgumentError**: If invalid arguments are passed (e.g., invalid property name or filtering criteria).
-   **InvalidOperationError**: If the operation cannot be performed due to an invalid query.
-   **General Errors**: Any unhandled errors will result in a generic error message with an `InternalServerError` status code.

Example:

`try
{
    var response = await _dbContext.Items.ToApiResponseAsync();
}
catch (Exception ex)
{
    return BadRequest(new ApiResponse<string>
    {
        Success = false,
        Message = ex.Message,
        StatusCode = HttpStatusCode.BadRequest
    });
}`

Contributing
------------

Contributions to `APIResponseWrapper` are welcome! Please fork the repository, make changes, and submit a pull request with detailed explanations for your changes.

### How to Contribute

1.  Fork the repository.
2.  Create a new branch (`git checkout -b feature/your-feature`).
3.  Make your changes.
4.  Commit your changes (`git commit -am 'Add your feature'`).
5.  Push to your branch (`git push origin feature/your-feature`).
6.  Create a new Pull Request.

License
-------

This project is licensed under the MIT License - see the LICENSE file for details.

Acknowledgments
---------------

-   Special thanks to the .NET community for creating such a rich ecosystem for developing modern APIs.
-   Entity Framework Core for simplifying database access in .NET.
